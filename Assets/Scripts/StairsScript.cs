using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsScript : MonoBehaviour
{

    [SerializeField] private TeethScript[] upperTeethArray;
    [SerializeField] private TeethScript[] lowerTeethArray;
    private Animator mAnimator;
    private int randomIndex;
    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        randomIndex = Random.Range(0, 6);
        Debug.Log("random index picked: " + randomIndex);

        for (int i = 0; i < 6; i++)
        {
            upperTeethArray[i].index = i;
            lowerTeethArray[i].index = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("hit");
                Debug.Log(hit.transform.parent.name + " : " + hit.transform.parent.tag);

                if (hit.transform.parent.tag == "lower_teeth")
                {
                   
                    ClickLowerTeeth(hit.transform.parent);
                }



            }
        }
    }

    void ClickUpperTeeth()
    {

    }

    void ClickLowerTeeth(Transform transform)
    {
        TeethScript teethScript = transform.GetComponent<TeethScript>();
        if (!teethScript.hasHit)
        {
            if (teethScript.index == randomIndex)
            {
                // oops you picked the bonk option
                Debug.Log("oops! index picked was the bonk option");
                ResetLowerTeeth();
                mAnimator.SetTrigger("Monch");
                StaticUIHandler.instance.ShowStairsResetButton();
            }
            else
            {
                 Debug.Log("Got here");
                transform.GetComponent<TeethScript>().gameObject.SetActive(false);
                teethScript.hasHit = true;
            }
           
            
        }
    }

    void ResetLowerTeeth()
    {
        foreach (TeethScript teeth in lowerTeethArray)
        {
            teeth.hasHit = false;
            teeth.gameObject.SetActive(true);

        }
    }

    public void Reset()
    {
        mAnimator.SetTrigger("Idle");
        randomIndex = Random.Range(0, 6);
        Debug.Log("random index picked: " + randomIndex);

    }
    

    

    // mAnimator.SetTrigger("Success")
    // mAnimator.SetTrigger("Monch")
}
