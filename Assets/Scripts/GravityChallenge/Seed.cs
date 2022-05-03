/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/Gravity Challenge
 */

using UnityEngine;

namespace RanunculusChallenge
{
    [RequireComponent(typeof(Rigidbody))]
    public class Seed : MonoBehaviour
    {
        private Rigidbody m_Rb;

        private Renderer m_Renderer;

        private void Awake()
        {
            // Caches the Rigidbody
            if (m_Rb == null)
                m_Rb = GetComponent<Rigidbody>();
            // Caches the Renderer
            if (m_Renderer == null)
                m_Renderer = GetComponent<Renderer>();
        }

        private void FixedUpdate()
        {
            // Determines whether the seed is visible.
            if (!m_Renderer.isVisible && transform.position.y < 8)
            {
                Destroy(this.gameObject, 1);
            }
        }

        // Calculates the force to be applies to the seed.
        public void CalculateForce(Vector3 startPosition, Vector3 endPosition)
        {
            m_Rb.AddForce(startPosition - endPosition);
        }

        // Destroys the seed.
        private void OnDestroy()
        {
            GameManager.Instance.CollectedSeedCount++;
        }
    }
}