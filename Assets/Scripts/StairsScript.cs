using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StairsScript : MonoBehaviour
{

    [SerializeField] private TeethScript[] upperTeethArray;
    [SerializeField] private TeethScript[] lowerTeethArray;
    private Animator mAnimator;
    private int randomIndex;
    private int remainingUpperTeeth;
    private String inputtedCode = "";
    private String secretCode = "012345";
    private bool isAnimationPlaying = false;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        randomIndex = UnityEngine.Random.Range(0, 6);
        remainingUpperTeeth = 6;
    
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
        if (!isAnimationPlaying && !StaticUIHandler.instance.CheckIfAnyPanelIsActive())
        {
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.Log("hit");
                    Debug.Log(hit.transform.parent.name + " : " + hit.transform.parent.tag);

                    if (hit.transform.parent.tag == "lower_teeth")
                        ClickLowerTeeth(hit.transform.parent);


                    else if (hit.transform.parent.tag == "upper_teeth")
                        ClickUpperTeeth(hit.transform.parent);

                }
            }
        }
    }

    void ClickUpperTeeth(Transform transform)
    {
        // this is the code with a secret code, for simplicity sake, we have it 0 -> 5
        TeethScript teethScript = transform.GetComponent<TeethScript>();
        if (!teethScript.hasHit)
        {

            remainingUpperTeeth--;
            transform.GetComponent<TeethScript>().gameObject.SetActive(false);
            inputtedCode += teethScript.index.ToString();

            if (remainingUpperTeeth == 0)
            {

                ResetTeeth();
                if (inputtedCode == secretCode)
                {
                    Debug.Log("you win! you may pass");
                    isAnimationPlaying = true;
                    mAnimator.SetTrigger("Success");
                    StaticUIHandler.instance.ShowStairsWinText();
                    StaticUIHandler.instance.ShowStairsResetButton(true);
                    // put 

                }
                else
                {

                    Debug.Log("oops! you inputted the wrong code");
                    ResetTeeth();
                    mAnimator.SetTrigger("Monch");
                    isAnimationPlaying = true;
                    StaticUIHandler.instance.ShowStairsLoseText();
                    StaticUIHandler.instance.ShowStairsResetButton(true);

                }
            }
            

        }
    }

    void ClickLowerTeeth(Transform transform)
    {

        // mechanics: crocodile game; don't tap on the tooth that triggers the monch
        // in actuality, there's no win condition here
        TeethScript teethScript = transform.GetComponent<TeethScript>();
        if (!teethScript.hasHit)
        {
            if (teethScript.index == randomIndex)
            {
                // oops you picked the bonk option
                Debug.Log("oops! index picked was the bonk option");
                ResetTeeth();
                 isAnimationPlaying = true;
                mAnimator.SetTrigger("Monch");
                StaticUIHandler.instance.ShowStairsLoseText();
                StaticUIHandler.instance.ShowStairsResetButton(true);
            }
            else
            {
                // remove tooth and take note it has been hit
                transform.GetComponent<TeethScript>().gameObject.SetActive(false);
                teethScript.hasHit = true;
            }


        }
    }
    
    void ResetTeeth()
    {
        foreach (TeethScript teeth in lowerTeethArray)
        {
            teeth.hasHit = false;
            teeth.gameObject.SetActive(true);

        }
          foreach (TeethScript teeth in upperTeethArray)
        {
            teeth.hasHit = false;
            teeth.gameObject.SetActive(true);
        }
    }

    public void Reset()
    {

        // no ResetTeeth() here since before the monching happens, teeth are already reset.
        mAnimator.CrossFade("Idle", 0f);
        inputtedCode = "";
        isAnimationPlaying = false;
        randomIndex = UnityEngine.Random.Range(0, 6);
        StaticUIHandler.instance.HideStairsText();
        remainingUpperTeeth = 6;

        Debug.Log("random index picked: " + randomIndex);
        

    }
    

}
