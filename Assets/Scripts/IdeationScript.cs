using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class IdeationScript : MonoBehaviour
{

    [SerializeField] private List<GameObject> initialClickables;
    [SerializeField] private GameObject firstPart;
    [SerializeField] private List<GameObject> firstPartClickables;
    [SerializeField] private GameObject secondPart;
    [SerializeField] private List<GameObject> secondPartClickables;
    [SerializeField] private GameObject thirdPart;
    [SerializeField] private List<GameObject> thirdPartClickables;
    [SerializeField] private ParticleSystem destroyedParticleSystem;

    [SerializeField] private ParticleSystem completeParticleSystem;
    [SerializeField] private ParticleSystem finishedParticleSystem;
    [SerializeField] private GameObject stairs;
    int currentPart;

    Vector3 originalPosFirstPart;

    int maxShifts = 4;
    int stepVertical = 0;
    int stepHorizontal = 0;

    float snapThreshold = 0.6f;
    float snapRotateThreshold = 0.3f;
    float increment = 0.1f;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        //material = new Material(stairs.GetComponent<Renderer>().material);
        currentPart = -1;
        Shuffle(firstPartClickables);
        Shuffle(secondPartClickables);
        Shuffle(thirdPartClickables);
        StartCoroutine(InitialPulseEmission());


        //RandomizeFirstPartProblem();
        originalPosFirstPart = firstPart.transform.position;
        RandomizeThirdPartProblem();
        
        destroyedParticleSystem.gameObject.SetActive(false);
        completeParticleSystem.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        //stairs.GetComponent<Renderer>().sharedMaterial = material;
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

                if (hit.transform.parent.tag == "idea_blocks")
                {
                   Debug.Log(hit.transform.parent.name);

                   GameObject hitObject = hit.transform.gameObject;

                  if (currentPart == -1)
                    {
                        firstPart.SetActive(true);
                        currentPart++;
                        Debug.Log(firstPart.transform.position);
                        StartCoroutine(PulseEmission());
                        
                    }

                  if (currentPart == 0)
                    {
                        // move, set maxUp/right/down/left

                        if (hitObject == firstPartClickables[0])
                        {
                            // up?
                            if (stepVertical < maxShifts)
                            {
                                Debug.Log("up");
                                firstPart.transform.position += new Vector3(0, increment, 0);
                                stepVertical++;
                            }
                        }
                        else if (hitObject == firstPartClickables[1])
                        {
                            // down
                            if (stepVertical > maxShifts * -1)
                            {
                                Debug.Log("down");
                                 firstPart.transform.position += new Vector3(0, increment*-1, 0);
                                stepVertical--;
                            }
                        }
                        
                        else if (hitObject == firstPartClickables[2])
                        {
                            // left
                            if (stepHorizontal > maxShifts * -1)
                            {
                                firstPart.transform.position += new Vector3(increment * -1, 0, 0);
                                stepHorizontal--;
                            }
                        }
                        else if (hitObject == firstPartClickables[3])
                        {
                            // right
                             if (stepHorizontal < maxShifts)
                            {
                                firstPart.transform.position += new Vector3(increment, 0, 0);
                                stepHorizontal++;
                            }
                        }


                    }

                  if (currentPart == 1)
                    {

                        // random thing that changes the color of the whole stairs because why not
                        if (hitObject == secondPartClickables[0])
                        {
                            stairs.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.Range(0F,1F), UnityEngine.Random.Range(0, 1F), UnityEngine.Random.Range(0, 1F));
                        }
                        // this opens the next part
                        else if (hitObject == secondPartClickables[1])
                        {
                            currentPart++;

                            thirdPart.SetActive(true);
                            StartCoroutine(PulseEmission());
                        }
                        // oops you chose wrong, it destroyed the path
                        else if (hitObject == secondPartClickables[2])
                        {
                            // the thing that destroys the path...
                            firstPart.transform.position = originalPosFirstPart;
                            currentPart--;
                            secondPart.SetActive(false);
                            
                            StartCoroutine(PulseEmission());
                            StartCoroutine(PlayThenPauseParticleSystem(1f));
                            // reset position
                        }
                       
                    }

                  if (currentPart == 2)
                    {
                        // z rotation 30
                        if (hitObject == thirdPartClickables[0])
                        {
                            thirdPart.transform.Rotate(new Vector3(0, 0, 30));
                        }
                        // -90
                        else if (hitObject == thirdPartClickables[1])
                        {
                            thirdPart.transform.Rotate(new Vector3(0, 0, -30));
                        }
                      
                    }
                }

            }
        }

        // check condition for first part and third part
        CheckPartCompletion();

    }

    private void RandomizeFirstPartProblem()
    {
        float randomInt1 = UnityEngine.Random.Range(-2, 2f);
        float randomInt2 = UnityEngine.Random.Range(-2, 2f);
        
        Debug.Log(randomInt1);
        Debug.Log(randomInt2);
        
        Vector3 newPos = new Vector3(randomInt1/10, randomInt2/10, 0f);
        Debug.Log(newPos);
        originalPosFirstPart = newPos;
        firstPart.transform.position = newPos;
        Debug.Log(firstPart.transform.position = newPos);
    }


    private void RandomizeThirdPartProblem()
    {
        int randomMultiplier = UnityEngine.Random.Range(1, 10);

        thirdPart.transform.rotation = Quaternion.Euler(0, 0, 30 * randomMultiplier);
    }

    void Shuffle<T>(List<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    private bool isFirstPartComplete()
    {
        float distance = Vector3.Distance(firstPart.transform.position, new Vector3(0,0,0));
        Debug.Log("distance: " + distance);

        Debug.Log("firstPart: " + firstPart.transform.position);
       if (distance < snapThreshold)
        {
            currentPart++;
            secondPart.SetActive(true);
            StartCoroutine(PulseEmission());
            return true;
        }
        return false;
    }


    private bool isThirdPartComplete()
    {
        Debug.Log(thirdPart.transform.rotation.z);
        Debug.Log(thirdPart.transform.rotation.z % 360);
        if (thirdPart.transform.rotation.z % 360 < snapRotateThreshold)
        {
            currentPart++;
            StaticUIHandler.instance.ShowIdeationWinText();
            completeParticleSystem.gameObject.SetActive(true);
            finishedParticleSystem.gameObject.SetActive(true);
            return true;
        }

        return false;
    }

     IEnumerator PlayThenPauseParticleSystem(float duration)
    {
        float elapsed = 0f;
        destroyedParticleSystem.gameObject.SetActive(true);
        destroyedParticleSystem.Play();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            yield return null; // wait for next frame
        }

        destroyedParticleSystem.Stop();
        destroyedParticleSystem.gameObject.SetActive(false);

    }

    private bool CheckPartCompletion()
    {
        if (currentPart == 0)
        {
            return isFirstPartComplete();
        }
        else if (currentPart == 2)
        {
            return isThirdPartComplete();
        }

        return false;
    }


    private IEnumerator PulseEmission()
    {
        float duration = 1f;   // speed of the pulse
        int getCurrentPart = currentPart;
        List<GameObject> clickables = GetClickables(getCurrentPart);

        Color black = Color.black;
        Color white = Color.white;

        while (currentPart == getCurrentPart)
        {
            // BLACK → WHITE
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float smooth = Mathf.SmoothStep(0f, 1f, t / duration);
                
                foreach(GameObject obj in clickables)
                {
                    obj.transform.parent.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(black, white, smooth));
                    
                }
                yield return null;
            }

            // WHITE → BLACK
            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float smooth = Mathf.SmoothStep(0f, 1f, t / duration);
                foreach(GameObject obj in clickables)
                {
                 obj.transform.parent.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(white, black, smooth));
                
                }
                yield return null;
            }
        }
    }

     private IEnumerator InitialPulseEmission()
    {
        float duration = 1f;   // speed of the pulse
        int getCurrentPart = currentPart;
        List<GameObject> clickables = initialClickables;

        Color black = Color.black;
        Color white = Color.white;

        while (currentPart == getCurrentPart)
        {
            Debug.Log("test");
            // BLACK → WHITE
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float smooth = Mathf.SmoothStep(0f, 1f, t / duration);
                
                foreach(GameObject obj in clickables)
                {
                    obj.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(black, white, smooth));
                    
                }
                yield return null;
            }

            // WHITE → BLACK
            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float smooth = Mathf.SmoothStep(0f, 1f, t / duration);
                foreach(GameObject obj in clickables)
                {
                 obj.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(white, black, smooth));
                
                }
                yield return null;
            }
        }
    }

    private List<GameObject> GetClickables(int index)
    {
        switch(index){
            case 0:
                return firstPartClickables;
            case 1:
                return secondPartClickables;
            case 2:
                return thirdPartClickables;
            
        }
        return null;
    }

   



}
