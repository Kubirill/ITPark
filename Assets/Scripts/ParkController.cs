using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public delegate void Method(int num,float x, float y, float z);
[Serializable]
public class ProgramStep
{
    public String Step;
    public Method StepDel;
    public int num;
    public float x;
    public float y;
    public float z;
    public float longStep;
    public ProgramStep()
    {
        Step= "Empty";
        StepDel = Empty;
        x =0;
        y=0;
        z=0;
        longStep=0;
    }
    
    public void Empty(int num, float empty, float empty1, float empty2)
    {
        
    }
}
[Serializable]
public class ParkMechanism <T>
{
    public string groupName;
    public float speed;
    public T[] objects;
}

[Serializable]
public class RotateGroups: ParkMechanism<Rotate>
{
    public float accel;

    public void SetChanges()
    {
        foreach (var rotObject in objects)
        {
            rotObject.maxSpeed = speed;
            rotObject.accelerate = speed;
        }
    }
}

[Serializable]
public class RandomRotateGroups : ParkMechanism<RandomRotate>
{
    
    public float accel;
    public float wait;

    public void SetChanges()
    {
        foreach (var rotObject in objects)
        {
            rotObject.maxSpeed = speed;
            rotObject.accelerate = speed;
            rotObject.midleWaitTime = wait;
        }
    }
}

[Serializable]
public class PistonGroups : ParkMechanism<Piston>
{
    
    public float maxAngle;
    public float minAngle;
    public bool active = false;

    public void SetChanges()
    {
        foreach (var rotObject in objects)
        {
            rotObject.maxAngle = maxAngle;
            rotObject.minAngle = minAngle;
            rotObject.speed = speed;
            rotObject.active = active;
        }
    }
}

[Serializable]
public class HorizontalPistonGroups : ParkMechanism<HorizontalPiston>
{
    
    public float minDistance;
    public float maxDistance;
    public bool active;

    public void SetChanges()
    {
        foreach (var rotObject in objects)
        {
            rotObject.minDistance = minDistance;
            rotObject.maxDistance = maxDistance;
            rotObject.speed = speed;
            rotObject.active = active;
        }
    }

}

public class ParkController : MonoBehaviour
{
    public List<RotateGroups> rotateGroups;
    public List<RandomRotateGroups> randomRotateGroups;
    public List<PistonGroups> pistonGroups;
    public List<HorizontalPistonGroups> horizontalPistonGroups;

    //

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
    //
    public bool newChange;
    public List<ProgramStep> programs;

    // Start is called before the first frame update
    void Start()
    {
        SetSpeed();
        SetDelegate();
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

    public void ChangeRotate(int numberGroup,float speed,float accel,float empty)
    {
        rotateGroups[numberGroup].speed = speed;
        rotateGroups[numberGroup].accel = accel;
        rotateGroups[numberGroup].SetChanges();
    }
    public void ChangeSeets(int numberGroup, float speed, float accel, float wait)
    {
        randomRotateGroups[numberGroup].speed = speed;
        randomRotateGroups[numberGroup].accel = accel;
        randomRotateGroups[numberGroup].wait = wait;
        randomRotateGroups[numberGroup].SetChanges();
    }
    public void ChangePistons(int numberGroup, float BigAngl, float LitAngl, float speed)
    {
        pistonGroups[numberGroup].maxAngle = BigAngl;
        pistonGroups[numberGroup].minAngle = LitAngl;
        pistonGroups[numberGroup].speed = speed;
        pistonGroups[numberGroup].SetChanges();
    }
    public void ChangePistonsActive(int numberGroup, float empty, float empty1, float empty2)
    {
        pistonGroups[numberGroup].active = !pistonGroups[numberGroup].active;
        pistonGroups[numberGroup].SetChanges();
    }

    public void ChangeHorizontalPistons(int numberGroup, float maxDistance, float minDistance, float speed)
    {
        horizontalPistonGroups[numberGroup].maxDistance = maxDistance;
        horizontalPistonGroups[numberGroup].minDistance = minDistance;
        horizontalPistonGroups[numberGroup].speed = speed;
        horizontalPistonGroups[numberGroup].SetChanges();
    }
    public void ChangeHorizontalPistonsActive(int numberGroup, float empty, float empty1, float empty2)
    {
        horizontalPistonGroups[numberGroup].active = !horizontalPistonGroups[numberGroup].active;
        horizontalPistonGroups[numberGroup].SetChanges();
    }

    IEnumerator ParkProgram()
    {
        foreach (ProgramStep step in programs)
        {
            step.StepDel(step.num, step.x, step.y, step.z);
            yield return new WaitForSeconds(step.longStep);
            StopCoroutine(ParkProgram());
        }
    }
    void SetDelegate()
    {
        foreach (ProgramStep step in programs)
        {
            if (step.Step == "Change Rotate")
            {
                step.StepDel = ChangeRotate;


            }
            if (step.Step == "Change Random Rotate")
            {
                step.StepDel = ChangeSeets;
            }
            if (step.Step == "Change Piston")
            {
                step.StepDel = ChangePistons;
            }
            if (step.Step == "Activate or Diactivate Piston")
            {
                step.StepDel = ChangePistonsActive;

            }
            if (step.Step == "Change Horizontal Piston")
            {
                step.StepDel = ChangeHorizontalPistons;
            }
            if (step.Step == "Activate or Diactivate Horizontal Piston")
            {
                step.StepDel = ChangeHorizontalPistonsActive;

            }
        }
    }
}
