using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : Rotate
{
    // Start is called before the first frame update
    float waitTime=0;
    public float midleWaitTime;
    // Update is called once per frame
    void Update()
    {
        if (active) 
        {
            if (waitTime <= 0)
            {

                waitTime = Random.Range(0, midleWaitTime) + Random.Range(0, midleWaitTime);
                accelerate = -accelerate;
                maxSpeed = -maxSpeed;
            }
            if (maxSpeed == speed)
            {
                waitTime = waitTime - Time.deltaTime;
            }
        }
        else
        {
            if(maxSpeed!=0) maxSpeed = (Mathf.Abs(maxSpeed) - Time.deltaTime)* maxSpeed/ Mathf.Abs(maxSpeed);
            if (waitTime <= 0)
            {

                waitTime = Random.Range(0, midleWaitTime) + Random.Range(0, midleWaitTime);
                accelerate = -accelerate;
                maxSpeed = -maxSpeed;
            }
            if (maxSpeed == speed)
            {
                waitTime = waitTime - Time.deltaTime;
            }
        }
        DoRotate();
    }
}
