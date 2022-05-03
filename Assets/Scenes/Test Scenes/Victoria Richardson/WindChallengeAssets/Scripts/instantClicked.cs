using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantClicked : MonoBehaviour
{//Script placed on the camera

    public GameObject windObject; //gameobject that displays player wind tap action
    public GameObject cam;
    public GameObject seedObject;
    
    void Start()
    {
        Vector3 seedpos = new Vector3(-5.28f, 8.74f, -0.39f);
        Instantiate(seedObject, seedpos, Quaternion.identity);
        StartCoroutine(newSeed()); //randomly continously creating seeds
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1.35f);
        Vector3 wordPos;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            wordPos = hit.point;
        }
        else
        {
            wordPos = Camera.main.ScreenToWorldPoint(mousePos);
        }
        Instantiate(windObject, wordPos, Quaternion.identity);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    IEnumerator newSeed()
    {
        Vector3 seedpos = new Vector3(Random.Range(2.66f, -5.28f), 8.74f, -0.39f);

        yield return new WaitForSeconds(Random.Range(4f, 8f));
        Instantiate(seedObject, seedpos, Quaternion.identity);
        StartCoroutine(newSeed());
    }


}
