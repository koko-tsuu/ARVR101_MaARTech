using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StairsScript : MonoBehaviour
{

    [SerializeField] private TeethScript[] upperTeethArray;
    [SerializeField] private TeethScript[] lowerTeethArray;
    [SerializeField] private GameObject ideationSite;
    [SerializeField] private GameObject judgementChamber;
    private Animator mAnimator;
    private int randomIndex;
    private int remainingUpperTeeth;
    private String inputtedCode = "";
    private String secretCode = "012345";
    private bool isAnimationPlaying = false;

    private GameObject ideationHolderObject;
    private GameObject judgementChamberHolderObject;
    

    private Material mat;
    void Start()
    {
        ideationHolderObject = Instantiate(ideationSite, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z + 1f), ideationSite.transform.rotation);
        judgementChamberHolderObject = Instantiate(judgementChamber, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y -1.5f, this.gameObject.transform.position.z), judgementChamber.transform.rotation);
        mAnimator = GetComponent<Animator>();
        randomIndex = UnityEngine.Random.Range(0, 6);
        remainingUpperTeeth = 6;
    
        Debug.Log("random index picked: " + randomIndex);

        for (int i = 0; i < 6; i++)
        {
            upperTeethArray[i].index = i;
            lowerTeethArray[i].index = i;
        }

        mat = upperTeethArray[0].GetComponent<Renderer>().sharedMaterial;
        mat.EnableKeyword("_EMISSION");

        StartCoroutine(PulseEmission());
    }

    void OnDestroy()
    {
        Destroy(ideationHolderObject);
        Destroy(judgementChamberHolderObject);
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

     private IEnumerator PulseEmission()
    {
        float duration = 1f;   // speed of the pulse

        Color black = Color.black;
        Color white = Color.white;

        while (!isAnimationPlaying)
        {
            // BLACK → WHITE
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float smooth = Mathf.SmoothStep(0f, 1f, t / duration);

                mat.SetColor("_EmissionColor", Color.Lerp(black, white, smooth));
                yield return null;
            }

            // WHITE → BLACK
            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float smooth = Mathf.SmoothStep(0f, 1f, t / duration);

                mat.SetColor("_EmissionColor", Color.Lerp(white, black, smooth));
                yield return null;
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
                    //judgementParticleSystem.SetActive(true);
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
                //judgementParticleSystem.SetActive(true);
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
        StartCoroutine(PulseEmission());
        

    }
    

}
