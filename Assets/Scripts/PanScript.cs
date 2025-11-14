using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanScript : MonoBehaviour
{
    [Header("Edit Pan Variables")]
    bool isEmittingWords = true;
    public string word = "Hello World!";

    public int fontSelectedIndex = 0;
    public static bool isMove = true;

    public Color panColor;
    
    bool isTracking;
    float minRange = -1f;
    float maxRange = 1f;
    UnityEngine.Vector3 touchPosition;

    private float torque = 1.0f;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        panColor = gameObject.GetComponent<Renderer>().material.color;
       
    }
    void OnEnable()
    {
        isTracking = true;
        StartCoroutine(EmitText());
         
    }

    void OnDisable()
    {
        isTracking = false;
    }

    void Update()
    {
        transform.position = new UnityEngine.Vector3(Mathf.Clamp(transform.position.x, minRange, maxRange), Mathf.Clamp(transform.position.y, 0f, maxRange), Mathf.Clamp(transform.position.z, minRange, maxRange));
    }

    IEnumerator EmitText()
    {
        while (isEmittingWords && isTracking)
        {
            yield return new WaitForSeconds(0.2f);
            DynamicTextManager.CreateText(RandomizeVector(this.transform), word, DynamicTextManager.defaultData[fontSelectedIndex]);

        }
    }

    UnityEngine.Vector3 RandomizeVector(Transform transform)
    {
        return new UnityEngine.Vector3(transform.position.x + UnityEngine.Random.Range(0f, 0.5f), transform.position.y, transform.position.z + UnityEngine.Random.Range(0f, 0.5f));
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
