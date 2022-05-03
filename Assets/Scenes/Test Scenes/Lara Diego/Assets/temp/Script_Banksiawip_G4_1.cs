using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Banksiawip_G4_1 : MonoBehaviour
{
    public Animator animator;

    
    //Not a programmer obviously

    void Update()
    {
        //left alt i think lol, makes it do a lil dance
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("trigger1");
            animator.SetTrigger("idleToDance");
        }
        
        //left ctrl probably makes it go back to idle
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("trigger2");
            animator.SetTrigger("danceToIdle");
        }
            
    }
}
