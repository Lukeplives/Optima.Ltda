using System.Collections.Generic;
using UnityEngine;


public class DetectorCano : MonoBehaviour
{
   public string tagTarget = "Belt";

    public List<Collider2D> detectedObjs = new List<Collider2D>();
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == tagTarget)
        {
            detectedObjs.Add(collider);
            
            
        }
    }
    

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == tagTarget)
        {
            detectedObjs.Remove(collider);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
