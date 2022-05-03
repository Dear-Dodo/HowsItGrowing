using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedHitPod : MonoBehaviour
{//Assign this script to each seed, tag them 'seed'
    
    int count = 3; //this is how many times the seed gets hit on the pods before it is set unactive
    Vector3 originalPosition; //the position the seeds start in everytime the cone is replaced/rotated
    
   
    // Start is called before the first frame update
    void Start()
    {

        this.gameObject.transform.localScale = new Vector3(1.2f, 0.77f, 0.40f); //set this to the original scale the seeds are, as they need to be set back to this scale after being destroyed. Not neccessary when we implement the seed breaking
        originalPosition = this.gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "pod") // there should be pod tagged objects on the pods, in this iteration they are a lighter colour so the player can see those are the 'danger' spots
            
        {
            if (count > 0)
            {
                this.gameObject.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                count--;
            }
            if (count == 0)//if it's been destroyed it's been picked up so it must be set back to kinematic, original scale and position, then setinactive until it's 'replaced'
            {
                this.gameObject.transform.position = originalPosition;
                this.gameObject.transform.localScale = new Vector3(1.2f, 0.77f, 0.40f);
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.gameObject.SetActive(false);
               
            }
            
        }
        if (collision.gameObject.tag == "Finish") //Finish object is the plate, must be reset the same way as above

        {
            this.gameObject.transform.localScale = new Vector3(1.2f, 0.77f, 0.40f);
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.transform.position = originalPosition;
                this.gameObject.SetActive(false);
            
        }
    }
 
}
