/*
 * Author: Ronan Richardson
 * Date: 14/11/2021
 * Folder Location: Assets/Scripts/BanksiaChallenge
 */

using UnityEngine;
using UnityEngine.Events;

// Disable the "never assigned to, default value of null" warning as it is bugged (all members are assigned a value)
#pragma warning disable 0649

namespace BanksiaChallenge
{
    /// <summary>
    /// Seed is the primary logic class attached to the Seed prefab that handles the constraining of movement while within a seedpod,
    /// the snapping to the tweezer object once released from it's parent seedpod, as well as the OnCollect and OnDestroy events which
    /// are called by external colliders such as the table and the plate. One seed is spawned by every seedpod on the banksia cone.
    /// </summary>
    public class Seed : MonoBehaviour
    {
        // Public events that are invoked by external colliding objects (e.g. the table calls OnDestroy, the plate calls OnCollect)
        public UnityEvent OnDestroy;
        public UnityEvent OnCollect;

        // Variables for locking the initial local position of the seed
        [SerializeField] private float m_axialLockDistance;
        private Vector3 m_localStartPos;
        private bool m_isMovementLocked;

        // A reference to the parent pod this seed was spawned by to track when the seed should be released, this is set by the seedpod
        public Seedpod ParentPod;

        // A reference to the DragInteractable script on this object to StorePostiion() once the seed has been snapped to the tweezers
        DragInteractable m_dragScript;

        // A reference to the seeds rigid body to apply rotational force when it is dropped
        private Rigidbody m_rig;
        // Stores the starting rotation to reset to each time a rotating seeds is grabbed
        Quaternion m_startingRotation;

        [SerializeField] private ParticleSystem m_seedPuffPrefab;

        // Reference
        private GameManager m_gameManager;
        private SoundManager m_soundManager;

        // Uses the lazy-initialisation pattern to store the Cone object
        private Cone m_cone;
        private Cone Cone
        {
            get
            {
                if (m_cone == null)
                {
                    m_cone = FindObjectOfType<Cone>();
                }
                return m_cone;
            }
        }

        // Uses the lazy-initialisation pattern to store the Tweezer object
        private Tweezers tweezers;
        private Tweezers Tweezers
        {
            get
            {
                if (tweezers == null)
                {
                    tweezers = FindObjectOfType<Tweezers>();
                }
                return tweezers;
            }
        }

        /// <summary>
        /// Start() simply adds the DestroySeed() function as a listener to both the OnCollect and
        /// OnDestroy events, as it is relevant to both event sequences, and caches the DragInteractable
        /// script attached to the seed prefab.
        /// </summary>
        void Start()
        {
            // Cache the rigid body, dragscript, game manager and sound manager
            m_dragScript = GetComponent<DragInteractable>();
            m_rig = GetComponent<Rigidbody>();
            m_gameManager = FindObjectOfType<GameManager>();
            m_soundManager = FindObjectOfType<SoundManager>();

            OnCollect.AddListener(() => 
            {
                m_soundManager.Play("BanksiaSeedSave");
                DestroySeed();
            });
            
            OnDestroy.AddListener(() => 
            {
                m_soundManager.Play("WaratahWind");
                SpawnPuff();
                DestroySeed();
            });
            
            // Store the starting rotation to reset to
            m_startingRotation = transform.localRotation;
        }

        /// <summary>
        /// OnCollisionEnter() simply checks to see if the colliding object is tagged with the plate or destructionObstacle
        /// tags. If the former, the function invokes OnCollect on this seed, while also activating the next active seed prefab
        /// on the plate and invoking its OnSeedCollected event. If the latter, then the function simply invokes OnDestroy on
        /// this seed to trigger destruction logic.
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("plate"))
            {
                OnCollect?.Invoke();
                
                // Cache the plate to call ActivateNextSeed() and invoke it's OnSeedCollected event
                Plate plate = collision.gameObject.GetComponent<Plate>();
                plate.ActivateNextSeed();
                plate.SparkleParticles.Play();

                // Increase the player's score in the GameManager
                m_gameManager.IncreaseScore();
            }
            else if (collision.gameObject.CompareTag("destructionObstacle"))
            {
                OnDestroy?.Invoke();
            }
        }

        /// <summary>
        /// Update simply calls ConstrainMovement() on this seed object if isMovementLocked is
        /// currently true, which stops the seed from being able to move out from its
        /// starting orientation.
        /// </summary>
        private void Update()
        {
            if (m_isMovementLocked)
            {
                ConstrainMovement();
            }
        }

        /// <summary>
        /// InitMovementLock() is called by this seeds parent seedpod when it spawns this seed
        /// object, and simply caches the initial local position of this seed in respect to the
        /// parent pod, and sets isMovementLocked to true so that movement is constrained every
        /// frame.
        /// </summary>
        public void InitMovementLock()
        {
            m_localStartPos = transform.localPosition;
            m_isMovementLocked = true;
        }

        /// <summary>
        /// ConstrainMovement() makes it so that the seed object can only move upwards along it's local y-axis,
        /// if the user has attempted to move the seed anywhere other than in the positive direction of its y-axis
        /// with respect to it's starting local position, then it's local position will be reset back to along
        /// it's y-axis. This function is called every frame if isMovementLocked is true, and stops the user
        /// from being able to move the seed up or down (wrt the scene camera orientation), as well as back down
        /// into the banksia cone. The function also checks to see whether the seed is now more than the axial
        /// lock distance away from it's parent pod, and unlocks seed movement if this is true.
        /// </summary>
        private void ConstrainMovement()
        {
            // If the user has tried to move the seed down into the banksia cone, lock all position movement
            if (transform.localPosition.y < m_localStartPos.y)
            {
                transform.localPosition = new Vector3(m_localStartPos.x, m_localStartPos.y, m_localStartPos.z);
            }
            // Otherwise lock everything apart from the y-axis
            else
            {
                transform.localPosition = new Vector3(m_localStartPos.x, transform.localPosition.y, m_localStartPos.z);
            }
            
            // If the seed is now past the axial lock distance set in the inspector, unlock it's movement
            if (Vector3.Distance(transform.position, ParentPod.transform.position) > m_axialLockDistance)
            {
                m_isMovementLocked = false;

                // Set this seeds parent reference to null as it has now been released from the pod
                ParentPod = null;
                SnapToTweezers();
            }
        }

        /// <summary>
        /// SnapToTweezers() simply sets this seeds position to be that of the Tweezer object attached to the player's mouse,
        /// to make it appear as though the seed is being held properly within the tweezers. The function also calls StorePosition()
        /// on this seed objects DragInteractable script so that the seed is dragged from the right position. This function is called
        /// whenever the seed has been released from it's parent pod in the ConstrainMovement() sequence, and then any time the seed
        /// is clicked upon after that.
        /// </summary>
        public void SnapToTweezers()
        {
            if (!m_isMovementLocked)
            {
                transform.position = Tweezers.transform.position;
                m_dragScript.StorePosition();
            }
        }

        /// <summary>
        /// RotateSeed() simply adds a random rotational torque to the seed with an impulse force magnitude of 1, this
        /// function is called whenever a seed is dropped (apart from when it is still attached to it's parent seedpod.
        /// </summary>
        public void RotateSeed()
        {
            if (!m_isMovementLocked)
            {
                m_rig.AddTorque(Random.onUnitSphere, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// FixRotation() simply sets the seeds current rotation to be that of it's starting orientation. This is called whenever
        /// a seed is clicked on, and is used so that seeds that have been rolling around still look like they're being held
        /// properly by the tweezers.
        /// </summary>
        public void FixRotation()
        {
            transform.localRotation = m_startingRotation;
        }

        /// <summary>
        /// SpawnPuff() simply instantiates a new m_seedPuffPrefab particle object, sets its position to be that of this
        /// seed, and calls Play() on the particle system. The function then sets the particle object to destroy itself after
        /// 1 second. This function is added as a listener to the OnDestroy event called when colliding with destructionObstacles.
        /// </summary>
        private void SpawnPuff()
        {
            // Instantiate a new puff particle system, set it's position to that of the seed and play it
            ParticleSystem particle = Instantiate(m_seedPuffPrefab);
            particle.transform.position = transform.position;
            particle.Play();

            // Set the particle system to destroy itself after it's duration
            Destroy(particle.gameObject, 1f);
        }

        /// <summary>
        /// DestroySeed() simply calls CheckActiveColumn() on the cone object to check if the current active column of seeds on the cone
        /// is now empty, and then destroys this seed object. This function is called by both the OnDestroy event AND the OnCollect event,
        /// as this logic is relevent to both events.
        /// </summary>
        private void DestroySeed()
        {
            // Check the current active column now that a seed has been dropped and destroy it
            Cone.CheckActiveColumn();
            Destroy(gameObject);
        }
    }
}