using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
  
   
    public Transform playerTrans;

    public float avoid=0;
    private float _sensorAngle=10;
    private float _sideSensorStart = .5f;
    private float _sensorFrontStart=1f;
    private float _sensorLength = 40;

    
    public float DetectAspect(  )
    {
        RaycastHit hit;

        Vector3 frontPos = transform.position + (transform.forward * _sensorFrontStart);
      
            if(DrawSensors(frontPos,Vector3.forward, _sensorLength, out hit))
            {
              
               // Debug.Log(hit.normal);
           

            if (hit.normal.x<0)
            {
                avoid = .25f;
            }
            else
            {
                avoid = - 0.25f;
            }

            avoid -= Sensors(frontPos, out hit, -1);
            avoid += Sensors(frontPos, out hit, 1);
        }
       



        return avoid;
    }

    bool DrawSensors(Vector3 sensorPos,Vector3 dir,float length,out RaycastHit hit)
    {
        if(Physics.Raycast(sensorPos,dir,out hit,length))
        {
            Debug.DrawLine(sensorPos, hit.point, Color.black);
            return true;
        }
        return false;
    }

    float Sensors(Vector3 frontPos,out RaycastHit hit,float dir)
    {
        float avoidDir = 0;

        Vector3 sensorPos = frontPos + (transform.right * _sideSensorStart * dir);
        Vector3 sensorAngle = Quaternion.AngleAxis(_sensorAngle * dir, transform.up) * transform.forward;
        if (Physics.Raycast(sensorPos, transform.forward, out hit, _sensorLength*2))
        {
            avoidDir = 1;
           
        }
        if (Physics.Raycast(sensorPos, sensorAngle, out hit, _sensorLength))
        {
            avoidDir = 0.5f;
          
            Debug.DrawLine(sensorPos,hit.point,Color.black);
        }
       
        return avoidDir;
    }

}
