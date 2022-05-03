/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/Gravity Challenge
 */


using System;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Checks for collision with Seed Object and invokes OnSeedCollided event.
/// </summary>
public class ScoreBox : MonoBehaviour
{
    public int ScoreToAdd;

    public UnityEvent OnSeedCollided;

    private void OnCollisionEnter(Collision collision)
    {
        if (string.Equals(collision.gameObject.tag, "RanunculusSeed", StringComparison.CurrentCultureIgnoreCase))
        {
            RanunculusChallenge.GameManager.Instance.AddScore(ScoreToAdd);
            OnSeedCollided?.Invoke();
            Destroy(collision.gameObject);
        }
    }
}