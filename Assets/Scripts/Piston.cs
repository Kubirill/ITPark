using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    public float minAngle;
    public float maxAngle;
    public float speed;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (transform.localEulerAngles.z<maxAngle) 
            {
                transform.RotateAround(transform.position, transform.forward, speed * Time.deltaTime);
            }
        }
        else
        {
            if ((transform.localEulerAngles.z > minAngle)&&(transform.localEulerAngles.z < minAngle+90))
            {
                transform.RotateAround(transform.position, transform.forward, -speed * Time.deltaTime);
            }
        }
    }
}
