/*
 * Author: Ronan Richardson
 * Date: 16/11/2021
 * Folder Location: Assets/Scripts/BanksiaChallenge
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Disable the "never assigned to, default value of null" warning as it is bugged (all members are assigned a value)
#pragma warning disable 0649

namespace BanksiaChallenge
{
    /// <summary>
    /// Cone is a gameplay script that is attached to the Banksia cone prefab, and implements functionality for the 
    /// movement and rotation of the cone during gameplay, as well as the management and activation of the seedpod's 
    /// attached to it. OnAwake() the script calculates the amount to rotate between each of the seedpod column prefabs
    /// attached to the cone, moves the cone from it's starting position off screen to it's arrival position in the middle
    /// of the camera, and activates the first column of seedpods, allowing their seeds to be grabbed. The script maintains
    /// a list of seedpods attached to it, storing them as a list of seepod arrays, where each element in the list represents
    /// a column of seedpods. Every time a seed is destroyed (either when it is collected by the plate or destroyed elsewhere),
    /// the CheckActiveColumn() function is triggered, which checks to see if all of the seedpods in the current active column
    /// have been removed, and if so it attempts to activate the next column of seedpods (or ends the game if there are no more
    /// columns remaining).
    /// </summary>
    public class Cone : MonoBehaviour
    {
        // Variables for moving the banksia back and forth between its start and end position
        [Header("Variables that affect the cones arrival movement")]
        [SerializeField] private float m_arrivalTime = 0.3f;
        private Vector3 m_velocity = Vector3.zero;
        private Transform m_startingTarget;
        private Transform m_arrivalTarget;

        // Variables for attaching and tracking the seedpod column prefabs on the banksia cone
        [Header("Attach the seedpod column objects here in their correct named order")]
        [SerializeField] private GameObject[] m_seedpodColumnPrefabs;
        private List<Seedpod[]> m_seedpodColumns = new List<Seedpod[]>();
        private int m_activeColumnIndex = 0;
        private static int m_emptySeedpodCount = 0;

        // Variables for rotating the banksia cone to the next column of seedpods
        [Header("Cone Rotation Parameters")]
        [SerializeField] private float m_rotationSpeed = 10f;
        private Vector3 m_rotationAmount = new Vector3(0, 0, 0);
        private Quaternion m_rotateTo;

        // Used to call GameManager.EndGame() when there are no columns remaining
        [SerializeField] private GameManager m_gameManager;

        /// <summary>
        /// Awake() first initialises the starting and arrival targets using their respective tags to find them in the scene.
        /// The function will then go through each attached seedpodColumn prefab to collate their respective seedpods into
        /// arrays of seedpod scripts, each is then added as an element of the seedpodColumns list. Finally, the function
        /// calculates the amount the banksia should rotate each time new seedpods need to be rotated to using 
        /// (360/number of seedpocolumns) and will then move the banksia to it's arrival target, activating the first active
        /// column. Because of the way the function caluclates the rotation amount around the z-axis, it assumes that each
        /// column is equally spaced around the banksia cone.
        /// </summary>
        private void Awake()
        {
            if (m_startingTarget == null)
            {
                m_startingTarget = GameObject.FindGameObjectWithTag("BanksiaStartPosition").transform;
            }
            if (m_arrivalTarget == null)
            {
                m_arrivalTarget = GameObject.FindGameObjectWithTag("BanksiaEndPosition").transform;
            }

            foreach (var seedpodColumnPrefab in m_seedpodColumnPrefabs)
            {
                Seedpod[] seedpodColumn = seedpodColumnPrefab.gameObject.GetComponentsInChildren<Seedpod>();
                m_seedpodColumns.Add(seedpodColumn);
            }
            // The angle between each column will be 360/number of columns, as they will always be evenly spaced
            m_rotationAmount.z = -(360 / m_seedpodColumns.Count);
        }

        /// <summary>
        /// MoveOntoScreen() simply moves the Banksia Cone from it's offscreen starting position to it's onscreen position,
        /// and Activates it's first column of seedpods. This function is called by the GameManager's OnGameStart event.
        /// </summary>
        public void MoveOntoScreen()
        {
            StartCoroutine(MoveTo(m_arrivalTarget));
            ActivateColumn();
        }

        /// <summary>
        /// Reseed() uses the MoveTo coroutine to move the banksia off screen back to it's starting positon, it then
        /// waits 2 seconds and will then completely reseed the banksia by calling Deactivate() and SpawnSeed() on
        /// every seedpod on the banksia, effectively resetting the entire object. The function will then move the
        /// banksia back on screen and activate the first column again, like at the start of play.
        /// </summary>
        private IEnumerator Reseed()
        {
            //yield return new WaitForSeconds(2);
            m_activeColumnIndex = 0;
            StartCoroutine(MoveTo(m_startingTarget));

            yield return new WaitForSeconds(2);

            // For each seedpod in every column of seedpods
            foreach (var seedpodColumn in m_seedpodColumns)
            {
                foreach (var seedpod in seedpodColumn)
                {
                    // Spawn a new seed
                    seedpod.SpawnSeed();
                }
            }

            // Move to the arrival target on screen and rotate to + activate the next (first) column of seedpods
            StartCoroutine(MoveTo(m_arrivalTarget));
            StartCoroutine(RotateToNextActive());
            ActivateColumn();
            yield return null;
        }

        /// <summary>
        /// CheckActiveColumn() first increases the static emptySeedpod counter by 1, as this function is only called each time a seed is destroyed.
        /// If the emptySeedpod counter is now equal to the number of seedpods in the current column, then the function will attempt to rotate to the 
        /// next active column of seedpods if there are still columns to rotate to. If there are no columns to rotate to, the function will either 
        /// reseed the banksia offscreen or trigger the EndGame() state.
        /// </summary>
        public void CheckActiveColumn()
        {
            // Increase the static emptySeedpod counter as this function is only called when a seed is destroyed (and therefore has been removed from a seedpod)
            m_emptySeedpodCount++;

            // If the current column is missing all its seeds, rotate to the next column or (end the game/reseed) depending on how many column are left
            if (m_emptySeedpodCount == m_seedpodColumns[m_activeColumnIndex].Length)
            {
                // Reset the emptySeedpod counter as we are now changing columns
                m_emptySeedpodCount = 0;
                // Increase the activeColumnIndex to make the next column the active one
                m_activeColumnIndex++;

                // If all seedpod columns have already been cycled through, end the game or reseed the banksia depending on design choice
                if (m_activeColumnIndex == m_seedpodColumns.Count)
                {
                    m_gameManager.EndGame();
                    return;
                }

                // Rotate the cone to it's next active column, and activate it
                StartCoroutine(RotateToNextActive());
            }
        }

        /// <summary>
        /// ActivateColumn() goes through each seedpod in the currently active seedpod column and
        /// calls Activate() on each of them, which causes them to trigger their Open animation
        /// and trigger their spawned seed's movement lock
        /// </summary>
        private void ActivateColumn()
        {
            foreach (var seedpod in m_seedpodColumns[m_activeColumnIndex])
            {
                seedpod.Activate();
            }
        }

        /// <summary>
        /// MoveTo() is run as a coroutine that takes an input of a transform target to move to, and
        /// will lerp the position of this gameObject to that target using Vector3.SmoothDamp(), over 
        /// a time determined by the member variable arrivalTime. This function is called on start, and
        /// called at the end of every RotateToNextActive() coroutine.
        /// </summary>
        /// <param name="moveToTarget">The transform of the target to move this gameObject towards.</param>
        IEnumerator MoveTo(Transform moveToTarget)
        {
            // While the distance between the current position and the target position is greater than 0.01units, SmoothDamp() to it
            while (Vector3.Distance(transform.position, moveToTarget.position) > 0.01f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, moveToTarget.position, ref m_velocity, m_arrivalTime);
                yield return null;
            }
            // Set the transform position to equal the targets after movement has finished to stop over/undershooting
            transform.position = moveToTarget.position;

            yield return null;
        }

        /// <summary>
        /// RotateToNextActive() takes the current local rotation of this Cone object, and will rotate it by an amount
        /// of (360/number of seedpod columns), assuming each of the seedpod columns are equally spaced on the banksia cone.
        /// The function runs as a coroutine that uses Quaternian.Lerp to lerp the rotation towards the desired amount over
        /// a time determined by the member variable rotationSpeed. After finishing the cone's rotation, the function calls
        /// ActivateColumn() to trigger the animation and activation of the next seedpod column.
        /// </summary>
        IEnumerator RotateToNextActive()
        {
            // Calculate the Quaternian that would result from rotating the current local rotation by rotationAmount
            m_rotateTo = transform.localRotation * Quaternion.Euler(m_rotationAmount);

            // While the banksia has not reached the desired next rotation, Lerp to it
            while (transform.localRotation != m_rotateTo)
            {
                transform.rotation = Quaternion.Lerp(transform.localRotation, m_rotateTo, m_rotationSpeed * Time.deltaTime);
                yield return null;
            }

            // Activate the next column now that it has been rotated to fully
            ActivateColumn();
        }
    }
}