using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class FSM : MonoBehaviour
{
    

    protected Vector3 dest; // where the ai will go

    [SerializeField]
    protected GameObject [] _points; //Our points

   

   

    public virtual void Init() 
    { 
    
    }
    private void Start()
    {
        Init();
    }

    public virtual void FSMUpdate()
    { 
    
    }
    private void Update()
    {
        FSMUpdate();
    }

    public virtual void FSMFixedUpdate() 
    { 
    
    }




   
}
