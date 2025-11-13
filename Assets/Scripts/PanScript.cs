using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanScript : MonoBehaviour
{
    public static bool isMove = true;
    float minRange = -1.5f;
    float maxRange = 1.5f;
    UnityEngine.Vector3 touchPosition;

    private float torque = 1.0f;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position = new UnityEngine.Vector3(Mathf.Clamp(transform.position.x, minRange, maxRange), Mathf.Clamp(transform.position.y, 0f, maxRange), Mathf.Clamp(transform.position.z, minRange, maxRange));
    }

    private UnityEngine.Vector3 GetTouchPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        if (isMove)
            touchPosition = Input.mousePosition - GetTouchPos();
        
        SelectPan.instance.Highlight(this.gameObject.transform);
        
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
    }
}
