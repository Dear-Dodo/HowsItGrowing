using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySphere : MonoBehaviour
{//Place on windObjectprefab, so that it destorys a fraction second after being created
    
    void Start()
    {
        StartCoroutine(destroy());
    }

    
    void Update()
    {
        
    }
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
