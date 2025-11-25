using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangoutInstantiatedObjectsHolder : MonoBehaviour
{

    [SerializeField] private GameObject outlineButton;
    [SerializeField] private GameObject onText;
    [SerializeField] private GameObject offText;
    bool isHighlightsOn = false;
    public static HangoutInstantiatedObjectsHolder instance {get; private set;}

    private List<GameObject> instantiatedObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
          if (instance != null)
        {
            Debug.Log("Found more than one HangoutScript in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }

    void Update()
    {
        if (instantiatedObjects.Count > 0)
        {
            outlineButton.SetActive(true);
        }
        else
            outlineButton.SetActive(false);



    }

    public void SwapOffOnHighlights()
    {
        isHighlightsOn = !isHighlightsOn;

        if (isHighlightsOn)
        {
            HighlightAllObjects();
            onText.SetActive(true);
            offText.SetActive(false);

        }

        else
        {
            DisableHighlightAllObjects();
            onText.SetActive(false);
            offText.SetActive(true);
        }
    }

    public void RemoveAllInstantiatedObjects()
    {
        foreach (GameObject obj in instantiatedObjects)
            Destroy(obj);
        
        instantiatedObjects.Clear();

    }

     public void HighlightAllObjects()
    {
        
        foreach (GameObject obj in instantiatedObjects)
        {
           HighlightObject(obj);
        }
      


    }

    private void HighlightObject(GameObject obj)
    {
         if (obj.GetComponent<Outline>() != null)
            {
                obj.GetComponent<Outline>().enabled = true;
            }
            else
            {
                Outline outline = obj.AddComponent<Outline>();
                outline.enabled = true;
                obj.GetComponent<Outline>().OutlineColor = Color.white;
                obj.GetComponent<Outline>().OutlineWidth = 30.0f;
            }

    }

    public void DisableHighlightAllObjects()
    {
         foreach (GameObject obj in instantiatedObjects)
        {
            obj.GetComponent<Outline>().enabled = false;
        }
    }
    
    public void AddNewObject(GameObject gameObject)
    {
        instantiatedObjects.Add(gameObject);

        if (isHighlightsOn)
        {
            HighlightObject(gameObject);
        }
    }
}
