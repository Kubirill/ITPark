using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    protected float speed;
    public float maxSpeed;
    public bool active;
    public float accelerate;
    delegate float Compare(float x, float y);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoRotate();
    }
    protected void  DoRotate()
    {
        Compare Min = Mathf.Min;
        Compare Max = Mathf.Max;
        if (accelerate < 0)
        {
            Min = Mathf.Max;
            Max = Mathf.Min;
        }

        if (active)
        {
            speed = Min(speed + accelerate * Time.deltaTime, maxSpeed);
        }
        else
        {
            speed = Max(speed - accelerate * Time.deltaTime, 0);
        }
        transform.RotateAround(transform.position, transform.up, speed * Time.deltaTime);
    }

}
