using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class Sense : MonoBehaviour
{
    public Aspect.aspect aspectName = Aspect.aspect.Enenmy;
    public float detectionRate = 1;
    public float elapsedTime;

    public virtual void Init() { }
    public virtual void UpdateSense() { }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSense();
    }
}
