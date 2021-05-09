using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public float speed = 20;
    public Transform target;
    public CreatePath path;
    public Rigidbody rb;
    private Sight _sight;
    private int _curP=0;
    private float _avoidSpeed=50;
    private float _DistToChange=5;
    List<Transform> _points { get { return path.points; } }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _sight = GetComponent<Sight>();
    }

    private void FixedUpdate()
    {
        float avoid = _sight.DetectAspect();

        if(avoid<=.24)
        {
            Steer();
        }
         else
        {
            Avoid(avoid);
        }
        Move();
        ChangePoint();
          
        
        ChangePoint();
        _sight.DetectAspect();
    }
    void Move()
    {
        rb.MovePosition(rb.position + (transform.forward * speed * Time.deltaTime));
    }
    void ChangePoint()
    {
        if(Vector3.Distance(rb.position,_points[_curP].position)<_DistToChange)
        {
            _curP++;
            if(_curP == _points.Count)
            {
                _curP = 0;
            }
        }
    }
    void Steer()
    {
        Vector3 tarDir = _points[_curP].position - rb.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, tarDir, Time.fixedDeltaTime,0);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void Avoid(float avoid)
    {
        transform.RotateAround(transform.position, transform.up, _avoidSpeed * Time.fixedDeltaTime*avoid );
    }
}
