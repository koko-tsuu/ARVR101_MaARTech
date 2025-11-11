using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ARScale : MonoBehaviour
{

    UnityEngine.Vector3 scale;
    float startDistance;

    public GameObject objectToScale; // this is just used to transfer local scale 

    // Update is called once per frame
    void Update()
    {
        // Raycast test
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("hit");
                Debug.Log(hit.transform.name + " : " + hit.transform.tag);



            }
        }

        // scale object to user needs
        ObjectScale();
    }
    
    void ObjectScale()
    {
        if (Input.touchCount > 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                objectToScale = hit.transform.gameObject;
            }
            if (Input.touchCount >= 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    startDistance = UnityEngine.Vector2.Distance(touch0.position, touch1.position);
                    scale = objectToScale.transform.localScale;
                }
                else
                {
                    UnityEngine.Vector2 v1 = touch0.position;
                    UnityEngine.Vector2 v2 = touch1.position;

                    float distance = UnityEngine.Vector2.Distance(v1, v2);

                    float factor = distance / startDistance;
                    objectToScale.transform.localScale = scale * factor;
                    
                }
            }
        }
        
    }
}