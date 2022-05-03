/*
 * Author: Alex Manning
 * Date: 3/11/2021
 * Folder Location: Assets/Scripts/WaratahChallenge
 */

using System.Collections.Generic;
using UnityEngine;

public class WindBox : MonoBehaviour
{
    private List<Seed> triggerList = new List<Seed>(); //list of seeds within the trigger

    /// <summary>
    /// Adds force to the seeds within the trigger
    /// </summary>
    private void Update()
    {
        // Game Running Safety Check (james)

        if (!Spawner.Instance.Running)
            gameObject.SetActive(false);

        foreach (Seed obj in triggerList)
        {
            obj.push(Vector3.right * 10.0f);
        }
    }

    /// <summary>
    /// Adds seeds entering the trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // Game Running Safety Check (james)

        if (!Spawner.Instance.Running)
            return;
        if (other.gameObject.CompareTag("seed (wind)"))
        {
            if (!triggerList.Contains(other.gameObject.GetComponent<Seed>()))
            {
                triggerList.Add(other.gameObject.GetComponent<Seed>());
            }
        }
    }

    /// <summary>
    /// Removes seeds leaving the trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // Game Running Safety Check (james)

        if (!Spawner.Instance.Running)
            return;
        if (triggerList.Contains(other.gameObject.GetComponent<Seed>()))
        {
            triggerList.Remove(other.gameObject.GetComponent<Seed>());
        }
    }
}