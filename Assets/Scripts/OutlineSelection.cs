using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{

    public static OutlineSelection instance { get; private set; }
    private Transform highlight;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one OutlineSelection Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }
    
    void Update()
    {
         if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit, 100))
                    RemoveHighlight();

        
            }
    }

    public void Highlight(Transform transform)
    {

        if (transform.gameObject.GetComponent<Outline>() != null)
        {
            transform.gameObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            Outline outline = transform.gameObject.AddComponent<Outline>();
            outline.enabled = true;
            transform.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
            transform.gameObject.GetComponent<Outline>().OutlineWidth = 10.0f;
        }

        if (highlight != null)
            highlight.gameObject.GetComponent<Outline>().enabled = false;


        highlight = transform;
        transform.gameObject.GetComponent<Outline>().enabled = true;

    }

    public void RemoveHighlight()
    {
        highlight.gameObject.GetComponent<Outline>().enabled = false;
        highlight = null;
    }
    

}