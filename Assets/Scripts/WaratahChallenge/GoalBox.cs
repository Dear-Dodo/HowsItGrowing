/*
 * Author: Alex Manning
 * Date: 3/11/2021
 * Folder Location: Assets/Scripts/WaratahChallenge
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalBox : MonoBehaviour
{
    public Spawner Spawner;                             //Spawner object
    public Animation Pulse;                             //Score animation
    public UnityEvent OnSeedEnter = new UnityEvent();   //Seed entry event
    private List<Seed> triggerList = new List<Seed>();  //List of seeds in trigger area

    // Update is called once per frame
    void Update()
    {
        //pull seeds offscreen
        foreach (Seed obj in triggerList)
        {
            obj.push(Vector3.right);
        }
    }
    /// <summary>
    /// Adds seeds to list and scores
    /// </summary>
    /// <param name="other">seed to add</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("seed (wind)"))
        {
            if (!triggerList.Contains(other.gameObject.GetComponent<Seed>()))
            {
                triggerList.Add(other.gameObject.GetComponent<Seed>());
                Spawner.addBonusTime();
                Spawner.Score++;
                Pulse.Play();
                OnSeedEnter.Invoke();
            }
        }
    }

    /// <summary>
    /// Removes seeds that leave the collider
    /// </summary>
    /// <param name="other">seed to remove</param>
    private void OnTriggerExit(Collider other)
    {
        if (triggerList.Contains(other.gameObject.GetComponent<Seed>()))
        {
            triggerList.Remove(other.gameObject.GetComponent<Seed>());
        }
    }
}
