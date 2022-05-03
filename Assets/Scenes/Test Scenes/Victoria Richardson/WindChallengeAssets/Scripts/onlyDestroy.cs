using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onlyDestroy : MonoBehaviour
{
   //Placed on 'walls' around camera scene to destroy off camera seeds
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "seed")
        {         
            Destroy(collision.gameObject);
        }
    }
}
