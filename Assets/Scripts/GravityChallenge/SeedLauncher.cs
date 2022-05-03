/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/Gravity Challenge
 */

using RanunculusChallenge;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Launched seed in desired direction using physics.
/// </summary>
public class SeedLauncher : MonoBehaviour
{
    public RanunculusChallenge.Seed SeedPrefab;

    public UnityEvent OnSeedLaunched;
    public UnityEvent OnMouseDownEvent;
    public UnityEvent OnMouseUpEvent;

    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;

    private void OnMouseDown()
    {
        if (!GameManager.Instance.Running)
        {
            return;
        }
        if (GameManager.Instance.SeedCount <= 0)
        {
            return;
        }

        m_StartPosition = Input.mousePosition;
        OnMouseDownEvent?.Invoke();
    }

    private void OnMouseUp()
    {
        if (!GameManager.Instance.Running)
            return;
        if (GameManager.Instance.SeedCount <= 0)
        {
            return;
        }

        m_EndPosition = Input.mousePosition;
        SpawnSeed();
        OnMouseUpEvent?.Invoke();

        GameManager.Instance.FadeProjection();
    }

    private void OnMouseDrag()
    {
        if (!GameManager.Instance.Running)
            return;
        if (GameManager.Instance.SeedCount <= 0)
        {
            return;
        }
        GameManager.Instance.ProjectionDrag();
    }

    private void SpawnSeed()
    {
        if (!GameManager.Instance.Running)
            return;

        if (GameManager.Instance.SeedCount <= 0)
        {
            return;
        }

        // Spawn seed in,
        RanunculusChallenge.Seed
            seed = Instantiate(SeedPrefab.gameObject, this.transform.position, Quaternion.identity).GetComponent<RanunculusChallenge.Seed>();

        seed.CalculateForce(m_StartPosition, m_EndPosition);

        GameManager.Instance.SeedCount--;

        OnSeedLaunched?.Invoke();
    }
}