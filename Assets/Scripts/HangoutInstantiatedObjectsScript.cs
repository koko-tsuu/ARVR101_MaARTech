using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangoutInstantiatedObjectsScript : MonoBehaviour
{
    float minRange = -1f;
    float maxRange = 1f;
    Vector3 touchPosition;
    Vector3 originalPos;

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

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - touchPosition);

        this.gameObject.GetComponent<Rigidbody>().velocity = UnityEngine.Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = UnityEngine.Vector3.zero;
    
    }
}
