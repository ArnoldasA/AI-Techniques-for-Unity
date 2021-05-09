using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePath : MonoBehaviour
{
    public List<Transform> points;
    // Start is called before the first frame update
  
    void Path()
    {
        points = new List<Transform>();
        points.AddRange(GetComponentsInChildren<Transform>());
        points.Remove(this.transform);//we don't want our ai as a path
    }
}
