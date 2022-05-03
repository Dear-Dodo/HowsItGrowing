using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tweezers : MonoBehaviour
{
    // this script should be attached to the tweezers parent object, that contains tweezersOpen and tweezersClosed

    public GameObject tweezerOpen; //start with tweezersOpen object active in scene
    public GameObject tweezerClosed; //start with tweezersClosed object UNactive in scene
    public GameObject holdSpot; //tweezersClosed should have an empty gameObject child where the seed will be held while moving
    public GameObject timeCount; //Canvas text object that shows time
    public GameObject panel; //Canvas panel that pops up when game ends. Set inactive for start
    public GameObject target; //This is where the tweezers are 'looking at' should be centered on the far left of the banksia
    public GameObject endScoreUI; //Canvas text object that shows end score. Set inactive for start
    Text text;
    Text text2;

    public bool open = true; //checking if tweezers are open or closed. start with True as the open object should be set active on start
    bool end = false; //endgame starts as false
    int time = 0;

   
    void Start()
    {
        Cursor.visible = false;
        StartCoroutine(timeRoutine()); //there are better ways to count time but I was lazy
        text = timeCount.GetComponent<Text>();
        text2 = endScoreUI.GetComponent<Text>();
    }

  
    void Update()
    {
        if (end == false )
        {
            this.transform.position += new Vector3(Input.GetAxis("Mouse X"), 0f,Input.GetAxis("Mouse Y")); 
            this.transform.LookAt(new Vector3(target.transform.position.x, tweezerOpen.transform.position.y, target.transform.position.z));
          
        }

      

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit(); // just for testing builds
            Cursor.visible = true;
        }
        if (Input.GetMouseButtonDown(0) && end == false)
        {
            if (open == true)
            {
                open = false;

                tweezerOpen.SetActive(false);
                tweezerClosed.SetActive(true);
            }
            else
            {
                open = true;
                tweezerOpen.SetActive(true);
                tweezerClosed.SetActive(false);
            }
          


        }
        if (Input.GetMouseButtonUp(0) && end == false)
        {
         
            if (open == true)
            {
                open = false;
                tweezerOpen.SetActive(false);
                tweezerClosed.SetActive(true);
            }
            else
            {
                open = true;
                tweezerOpen.SetActive(true);
                tweezerClosed.SetActive(false);
            }



        }
        



    }

    void endGame()
    {
        panel.SetActive(true);
        text2.text = time.ToString();
        end = true;
    }
    
    
    IEnumerator timeRoutine()
        {
           if (end == false)
        {
            yield return new WaitForSeconds(1f);
                time++;
            text.text = "Time: " + time.ToString();
            StartCoroutine(timeRoutine());
        }
                
  
        }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "seed" && open == false)
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false; //Seeds should start as kinematic to allow them to stay in the pod. on grabbing them they become nonkinematic so that they can fall with gravity onto the plate
            other.gameObject.transform.position = holdSpot.transform.position;

        }
        
    }
   

   
}
