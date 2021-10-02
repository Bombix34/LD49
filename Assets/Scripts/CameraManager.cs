using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    public float travelingSpeed = 1f;
    private float posZ;

    private void Awake()
    {
        posZ = this.transform.position.z;
    }

    private void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, target.position, Time.deltaTime* travelingSpeed);
        this.transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
    }
}
