using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HangoutScript : MonoBehaviour
{
    
    [SerializeField] private GameObject figures;
    [SerializeField] private GameObject particleSystem;

    
    
  
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

                    else if (hit.transform.name == "idea_collision")
                    {
                        figures.SetActive(true);
                        particleSystem.SetActive(true);
                    }



                }
            }
        
    }

    




}
