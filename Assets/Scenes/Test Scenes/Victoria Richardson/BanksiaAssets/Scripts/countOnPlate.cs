using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countOnPlate : MonoBehaviour
{ //Assign this script to the plate object with a collider, tag it 'Finish'
    int points = 1; 
    public int totalScore;
    public GameObject textUI; //top right text keeping score of seeds on plate
    public GameObject endScoreUI; //UI that exists on the INactive canvas
    public GameObject tweezers; //tweezers parent that has the tweezers script attached
    public GameObject cone; //this should be the cone parent that has the rotating/replacing animation
    public int scoreToFinish; //customisable amount, seeds to collect on the plate before game ends

    public GameObject seed1; //there are 5 seeds in total so I just assigned and counted them this way, there are probably better ways to count the seeds and check if they're active or not
    public GameObject seed2;
    public GameObject seed3;
    public GameObject seed4;
    public GameObject seed5;


    Text text;
    Text text2;
    
    void Start()
    {

        text = textUI.GetComponent<Text>();
        text2 = endScoreUI.GetComponent<Text>();
        totalScore = int.Parse(text.text);


    }

   
    void Update()
    {
        totalScore = int.Parse(text.text);
        if (totalScore == scoreToFinish)
        {
            tweezers.GetComponent<tweezers>().SendMessage("endGame"); //already have a few endgame stats assigned in the tweezer script so thought it easier to call endgame
        }

        if (!seed1.activeSelf && !seed2.activeSelf && !seed3.activeSelf && !seed4.activeSelf && !seed5.activeSelf)
        {
            StartCoroutine(timeRoutine()); //all the seeds are gone, so set active the new ones.
        }
    }
    IEnumerator timeRoutine() //the waitForSeconds should be timed with the anim, the anim in this instance goes for 1 second
    {
        cone.GetComponent<Animator>().enabled = true;
          yield return new WaitForSeconds(0.5f); 
        seed1.SetActive(true);
        seed2.SetActive(true);
        seed3.SetActive(true);
        seed4.SetActive(true);
        seed5.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        cone.GetComponent<Animator>().enabled = false;


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "seed") //could instantiate a prefab that isn't tagged 'seed' so that they can 'visually' accumulate on the plate?
        {
           
            totalScore += points;
            text.text = totalScore.ToString();
            text2.text = totalScore.ToString();
            //Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
           // rb.isKinematic = true; ------could leave these two lines in and allow the seeds to roll around on the plate for difficulty?
            
             

        }
    }
   
}
