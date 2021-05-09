using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : Sense
{
    public GameObject child;
    private void OnTriggerEnter(Collider other)
    {
        Aspect aspect = other.GetComponent<Aspect>();
        if (aspect != null)
        {
            //check
            if (aspect.aspectName == aspectName)
            {
                Debug.Log("Hit");
               
            }
        }
    }
}
