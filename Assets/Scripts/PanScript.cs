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
    public bool isEmittingWords = true;
    public string word = "Hello World!";

    public int fontSelectedIndex = 0;
    public static bool isMove = true;

    public Color panColor;

    public Sprite previewSprite;

    public float spawnRate = 0f;
    public float startSize = 0f;
    
    bool isTracking;

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

        if (isEmittingWords)
            StartEmittingText();
        else
            StartEmittingParticles();
         
    }

    void OnDisable()
    {
        isTracking = false;
    }

   

    IEnumerator EmitText()
    {
        while (isEmittingWords && isTracking)
        {
            yield return new WaitForSeconds(0.2f);
            DynamicTextManager.CreateText(RandomizeVector(this.transform), word, DynamicTextManager.defaultData[fontSelectedIndex]);

        }
    }

    public void StartEmittingText()
    {
        StartCoroutine(EmitText());
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void StartEmittingParticles()
    {
        // setting isEmittingText to false auto finishes the coroutine of emitting text
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
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
        
        // I'm doing this because I noticed that accidentally touching a pan while in an edit panel can happen
        if (!StaticUIHandler.instance.CheckIfSanctuaryEditPanelIsActive())
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
        
        // rb.velocity = UnityEngine.Vector3.zero;
        // rb.angularVelocity = UnityEngine.Vector3.zero;
    }
}
