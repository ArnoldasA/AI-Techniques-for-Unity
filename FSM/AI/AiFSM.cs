using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFSM : FSM
{
    public enum States
    {
        None,
        Patrol,
        Chase,
        Die,
    }

   //Finite state machines 
    
    [SerializeField]
    private float _speed=10;
    [SerializeField]
    float radius;
    [SerializeField]
    Transform _player;
    [SerializeField]
    GameObject DeathCube;
   

    public States curState;
    private bool dead;


    public override void Init()
    {

       
      
        curState = States.Patrol;
        dead = false;
        _points = GameObject.FindGameObjectsWithTag("Point");
        NextPos();
       
        
    }

    public override void FSMUpdate()
    {
        switch(curState)
        {
            case States.Patrol: Patrolling(); break;
            case States.Chase: Chasing(); break;
            case States.Die: Dying(); break;
        }
        //Enemy dies if it hits our death cube
        if (dead == true)
        {
            curState = States.Die;
        }
    }

    
    private void NextPos() 
    {
        int RandomPoint = Random.Range(0, _points.Length);
       
        Vector3 NextPos = Vector3.zero;
        dest = _points[RandomPoint].transform.position + NextPos;

        if(RangeFromPoint(dest))
        {
            NextPos = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)); //Our next point based on the radius of the points near us
            dest = _points[RandomPoint].transform.position + NextPos;
        }
        Debug.Log(NextPos);
    }

    private bool RangeFromPoint(Vector3 pos) // A check to ensure we dont go to the same point
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);

        if(xPos <=100 && zPos <= 100)
        {
            return true;
        }
       
            return false;
        

    }

    private void Patrolling() 
    {
        if (Vector3.Distance(transform.position, dest) <= 180f)
        {
            NextPos();
        }
         if (Vector3.Distance(transform.position, _player.position) <= 100f)
        {
            curState = States.Chase;
        }

        Quaternion tarRot = Quaternion.LookRotation(dest - transform.position);
       transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, Time.deltaTime * 2);
       transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }
    private void Chasing()
    {
        dest = _player.transform.position;

        float dist = Vector3.Distance(transform.position, dest);

        if (dist <= 200)
        {
            Quaternion tarRot = Quaternion.LookRotation(dest - transform.position);
       transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, Time.deltaTime * 2);
        }
         if (dist >= 250)
        {
            curState = States.Patrol;
        }

        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }
    private void Dying()
    {
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="DeathCube")
        {
            dead = true;
        }
    }
}
