using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void Method(float x, float y, float z);
public class ProgramStep
{
    public Method Step;
    public float x;
    public float y;
    public float z;
    public float longStep;
    public ProgramStep()
    {
        Step= Empty;
        x=0;
        y=0;
        z=0;
        longStep=0;
    }
    
    public void Empty(float empty, float empty1, float empty2)
    {
        
    }
}
public class ParkController : MonoBehaviour
{
    public Rotate mainDisk;
    public float mDspeed;
    public float mDaccel;

    public Piston[] pistons;
    public float maxAngle;
    public float minAngle;
    public float speedPiston;
    public bool pistonActive = false;

    public Rotate[] xPanels;
    public float xPspeed;
    public float xPaccel;


    public RandomRotate[] seets;
    public float seetSpeed;
    public float seetAccel;
    public float seetWait;

    public bool newChange;
    public List<ProgramStep> programs;
    public List<Vector4> editVariable;
    // Start is called before the first frame update
    void Start()
    {
        SetSpeed();
       
        StartCoroutine(ParkProgram());
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
            piston.speed = speedPiston;
            piston.active = pistonActive;
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

    public void ChangeRotate(float speed,float accel,float empty)
    {
        xPspeed = speed;
        xPaccel = accel;
        newChange = true;
    }
    public void ChangeSeets(float speed, float accel, float wait)
    {
        seetSpeed= speed;
        seetAccel= accel;
        seetWait= wait;
        newChange = true;
    }
    public void ChangePistons(float BigAngl, float LitAngl, float speed)
    {
        maxAngle=BigAngl;
        minAngle=LitAngl;
        speedPiston = speed;
        newChange = true;
    }
    public void ChangePistonsActive(float empty, float empty1, float empty2)
    {
        pistonActive = !pistonActive;
        newChange = true;
    }

    IEnumerator ParkProgram()
    {
        foreach (ProgramStep step in programs)
        {
            step.Step(step.x, step.y, step.z);
            yield return new WaitForSeconds(step.longStep);
            StopCoroutine(ParkProgram());
        }
    }
}
