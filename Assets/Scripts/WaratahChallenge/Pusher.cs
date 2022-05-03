/*
 * Author: Alex Manning
 * Date: 3/11/2021
 * Folder Location: Assets/Scripts/WaratahChallenge
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pusher : MonoBehaviour
{
    
    public Camera Camera;

    public float PushSpeed = 10.0f;                     //Speed to push seeds away
    public float Cooldown = 1.0f;                       //Cooldown between pushes
    private float zdist = 10.0f;                        //Distance from the camera
    public float Timer;                                 //Cooldown timer
    public bool HasObject;                              //Boolean if there is an object in the radius
    public ParticleSystem WindPuff;                     //Wind particle
    public UnityEvent OnClickDown = new UnityEvent();   //Click event
    private List<Seed> triggerList = new List<Seed>();  //List of seeds within the trigger

    // Start is called before the first frame update
    private void Start()
    {
        Timer = 0;

        // Turn Object off when game ends. (James)
        Spawner.Instance.OnGameOver.AddListener(() => { this.gameObject.SetActive(false); });
    }

    // Update is called once per frame
    private void Update()
    {
        //move the object to the mouse position
        Vector3 mousePos = Input.mousePosition;

        this.transform.position = Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zdist));

        HasObject = triggerList.Count > 0;

        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// push seeds away when mouse clicked
    /// </summary>
    private void OnMouseDown()
    {
        if (Timer <= 0)
        {
            foreach (Seed obj in triggerList)
            {
                Vector3 dir = obj.transform.position - this.transform.position;
                dir.Normalize();
                dir *= PushSpeed;
                obj.push(dir);
                
            }
            WindPuff.Play();
            OnClickDown.Invoke();
            Timer = Cooldown;
        }
    }

    /// <summary>
    /// Adds seeds entering the trigger
    /// </summary>
    /// <param name="other">Seed entering collider</param>
    private void OnTriggerEnter(Collider other)
    {
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
    /// <param name="other">Seed leaving collider</param>
    private void OnTriggerExit(Collider other)
    {
        if (triggerList.Contains(other.gameObject.GetComponent<Seed>()))
        {
            triggerList.Remove(other.gameObject.GetComponent<Seed>());
        }
    }
}