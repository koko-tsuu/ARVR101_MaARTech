using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HangoutScript : MonoBehaviour
{
    [SerializeField] private GameObject monitor;
    
    void Update()
    {
         
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Pressed primary button.");

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                   if (hit.transform.tag == "figure")
                    {
                        hit.transform.GetComponent<HangoutFigure>().AnimateComingUpWithIdea();
                    }



                }
            }
        
    }

    void OnEnable()
    {
        // StartCoroutine(PulseEmission());
    }
    private void OnMouseDown()
    {
        Debug.Log("hangout clicked");
        
    }


    private IEnumerator PulseEmission()
    {
        float duration = 1f;   // speed of the pulse

        Color black = Color.black;
        Color blue = Color.blue;

        monitor.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        while (true)
        {
            // BLACK → WHITE
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float smooth = Mathf.SmoothStep(0f, 1f, t / duration);

               // monitor.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                // monitor.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(black, blue, smooth));
                yield return null;
            }

            // WHITE → BLACK
            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float smooth = Mathf.SmoothStep(0f, 1f, t / duration);

                // monitor.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(blue, black, smooth));
                yield return null;
            }
        }
    }

}
