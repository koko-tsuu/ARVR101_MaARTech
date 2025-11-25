using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctuarySpawnableObjects : MonoBehaviour
{
    public static bool isMove = true;
    float minRange = -1.5f;
    float maxRange = 1.5f;

    Rigidbody rb;

    UnityEngine.Vector3 touchPosition;
    private float torque = 5.0f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
     void Update()
    {
        transform.position = new UnityEngine.Vector3(Mathf.Clamp(transform.position.x, minRange, maxRange), Mathf.Clamp(transform.position.y, minRange, maxRange), Mathf.Clamp(transform.position.z, minRange, maxRange));
    }

    private void OnMouseDown()
    {
            touchPosition = Input.mousePosition - GetTouchPos();
        
    
    }

    private UnityEngine.Vector3 GetTouchPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDrag()
    {
        if (isMove)
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - touchPosition);
        else
        {
            rb.AddTorque(UnityEngine.Vector3.up * torque * -Input.GetAxis("Mouse X"));
            rb.AddTorque(UnityEngine.Vector3.right * torque * Input.GetAxis("Mouse Y"));
        }
        
        // rb.velocity = UnityEngine.Vector3.zero;
        // rb.angularVelocity = UnityEngine.Vector3.zero;
    }
}
