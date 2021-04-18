using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public delegate void Method(int num,float x, float y, float z, bool change);
[Serializable]
public class ProgramStep
{
    public String Step;
    public Method StepDel;
    public int num;
    public float x;
    public float y;
    public float z;
    public bool change;
    public float longStep;
    public ProgramStep()
    {
        Step= "Empty";
        StepDel = Empty;
        x =0;
        y=0;
        z=0;
        change = false;
        longStep =0;
    }
    
    public void Empty(int num, float empty, float empty1, float empty2,bool change)
    {
        
    }
}
[Serializable]
public class ParkMechanism <T>
{
    public string groupName;
    public bool active = false;
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
            rotObject.accelerate = accel;
            rotObject.active = active;
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
            rotObject.accelerate = accel;
            rotObject.midleWaitTime = wait;
            rotObject.active = active;
        }
    }
}

[Serializable]
public class PistonGroups : ParkMechanism<Piston>
{
    
    public float maxAngle;
    public float minAngle;
    

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

    public List<ProgramStep> programs;

    // Start is called before the first frame update
    void Start()
    {
        
        SetDelegate();
        StartCoroutine(ParkProgram());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void ChangeRotate(int numberGroup,float speed,float accel,float empty,bool changeActive)
    {
        rotateGroups[numberGroup].speed = speed;
        rotateGroups[numberGroup].accel = accel;
        if (changeActive) rotateGroups[numberGroup].active = !rotateGroups[numberGroup].active;
       rotateGroups[numberGroup].SetChanges();
        
    }
    public void ChangeSeets(int numberGroup, float speed, float accel, float wait, bool changeActive)
    {
        randomRotateGroups[numberGroup].speed = speed;
        randomRotateGroups[numberGroup].accel = accel;
        randomRotateGroups[numberGroup].wait = wait;
        if (changeActive) randomRotateGroups[numberGroup].active = !randomRotateGroups[numberGroup].active;
        randomRotateGroups[numberGroup].SetChanges();
    }
    public void ChangePistons(int numberGroup, float BigAngl, float LitAngl, float speed, bool changeActive)
    {
        pistonGroups[numberGroup].maxAngle = BigAngl;
        pistonGroups[numberGroup].minAngle = LitAngl;
        pistonGroups[numberGroup].speed = speed;
        if (changeActive) pistonGroups[numberGroup].active = !pistonGroups[numberGroup].active;
        pistonGroups[numberGroup].SetChanges();
    }
    public void ChangePistonsActive(int numberGroup, float empty, float empty1, float empty2)
    {
        pistonGroups[numberGroup].active = !pistonGroups[numberGroup].active;
        pistonGroups[numberGroup].SetChanges();
    }

    public void ChangeHorizontalPistons(int numberGroup, float maxDistance, float minDistance, float speed, bool changeActive)
    {
        horizontalPistonGroups[numberGroup].maxDistance = maxDistance;
        horizontalPistonGroups[numberGroup].minDistance = minDistance;
        horizontalPistonGroups[numberGroup].speed = speed;
        if (changeActive) horizontalPistonGroups[numberGroup].active = !horizontalPistonGroups[numberGroup].active;
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
            step.StepDel(step.num, step.x, step.y, step.z,step.change);
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
                //step.StepDel = ChangePistonsActive;

            }
            if (step.Step == "Change Horizontal Piston")
            {
                step.StepDel = ChangeHorizontalPistons;
            }
            if (step.Step == "Activate or Diactivate Horizontal Piston")
            {
                //step.StepDel = ChangeHorizontalPistonsActive;

            }
        }
    }
}
