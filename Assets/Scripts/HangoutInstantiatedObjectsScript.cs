using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HangoutInstantiatedObjectsScript : MonoBehaviour
{
    public static bool isMove = true;
    float minRange = -3f;
    float maxRange = 3f;
    Vector3 touchPosition;
    Vector3 originalPos;

    Rigidbody rb;
    float torque = 1000.0f;

    void Update()
    {
        if (this.transform.position.x <= minRange || this.transform.position.x >= maxRange 
        || this.transform.position.y <= minRange || this.transform.position.y >= maxRange
        || this.transform.position.z <= minRange || this.transform.position.z >= maxRange)
        {
            this.transform.position = originalPos;
        }

    }

    void Start()
    {
        originalPos = this.transform.position;
        rb = this.GetComponent<Rigidbody>();
    }



    private Vector3 GetTouchPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    private void OnMouseDown()
    {
        touchPosition = Input.mousePosition - GetTouchPos();
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

         rb.velocity = UnityEngine.Vector3.zero;
         rb.angularVelocity = UnityEngine.Vector3.zero;
         
    }
}
