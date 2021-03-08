using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyDanceController : MonoBehaviour
{
    public Rotate mainDisk;
    public float mDspeed;
    public float mDaccel;

    public Piston[] pistons;
    public float maxAngle;
    public float minAngle;

    public Rotate[] xPanels;
    public float xPspeed;
    public float xPaccel;


    public RandomRotate[] seets;
    public float seetSpeed;
    public float seetAccel;
    public float seetWait;

    public bool newChange;
    // Start is called before the first frame update
    void Start()
    {
        SetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (newChange)
        {
            newChange = false;
            SetSpeed();
        }
    }
    void SetSpeed()
    {
        mainDisk.accelerate = mDaccel;
        mainDisk.maxSpeed = mDspeed;

        foreach (Piston piston in pistons)
        {
            piston.maxAngle = maxAngle;
            piston.minAngle = minAngle;
        }

        foreach (Rotate panel in xPanels)
        {
            panel.accelerate = xPaccel;
            panel.maxSpeed = xPspeed;
        }
        foreach (RandomRotate seet in seets)
        {
            seet.accelerate = seetAccel;
            seet.maxSpeed = seetSpeed;
            seet.midleWaitTime = seetWait;
        }
    }
}
