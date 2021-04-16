using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPiston : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;
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
            if (transform.localPosition.x < maxDistance)
            {
                transform.localPosition =new Vector3 (transform.localPosition.x + speed*Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
            }
        }
        else
        {
            if (transform.localPosition.x > minDistance) 
            {
                transform.localPosition = new Vector3(transform.localPosition.x - speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
            }
        }
    }
}
