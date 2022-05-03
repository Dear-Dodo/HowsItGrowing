using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class seedSaved : MonoBehaviour
{
    int seedCount;
    public GameObject textUI;
    public GameObject seedObject;
    Text text;
    
    void Start()
    {
        seedCount = 0;
        text = textUI.GetComponent<Text>();

    }

    
    void Update()
    {
        text.text = "Seeds Dispersed: " + seedCount.ToString();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "seed")
        {
            seedCount++;
            Destroy(collision.gameObject);
            StartCoroutine(newSeed()); //creating new seeds when you get a point
        }
    }

    IEnumerator newSeed()
    {
        Vector3 seedpos = new Vector3(Random.Range(2.66f, -5.28f), 8.74f, -0.39f);

        yield return new WaitForSeconds(Random.Range(0.1f, 3f));
        Instantiate(seedObject, seedpos, Quaternion.identity);
    }
}
