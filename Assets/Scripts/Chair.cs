using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChairControl.ChairWork;
using ChairControl.ChairWork.Options;


public class Chair : MonoBehaviour
{
    // Start is called before the first frame update
    public UdpOptions udpOptions;
    public Transform cameraParent;
   // public Transform cameraParentParent;

    private FutuRiftController futuRiftController;

    public void OnEnable()
    {
        futuRiftController = new FutuRiftController(udpOptions);
        futuRiftController.Start();
    }
    public void OnDisable()
    {
        futuRiftController.Stop();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var eurles = transform.eulerAngles;
        futuRiftController.Pitch = eurles.x > 180 ? eurles.x - 360 : eurles.x;
        
        futuRiftController.Roll = eurles.z > 180 ? eurles.z - 360 : eurles.z;
        cameraParent.localEulerAngles = new Vector3(-futuRiftController.Pitch, cameraParent.localEulerAngles.y,- futuRiftController.Roll);
        //cameraParentParent.eulerAngles = new Vector3(-cameraParentParent.eulerAngles.x, -cameraParent.eulerAngles.y, -cameraParentParent.eulerAngles.z);
    }
}
