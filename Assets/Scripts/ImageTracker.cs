using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    public static ImageTracker instance { get; private set; }

    private ARTrackedImageManager trackedImages;
    [SerializeField] private GameObject outlineSelection;
    
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject[] arPrefabs;
    private int currentObjectIndex = 0;
    private GameObject arCurrentActiveObject;
    private List<GameObject> sanctuaryPansList = new List<GameObject>();

    private Transform originalPos;

    private int modelIndexToSwitchTo;

    [SerializeField] private ARScale aRScale;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one ImageTracker Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        trackedImages = GetComponent<ARTrackedImageManager>();     
      
    
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
        arCurrentActiveObject = arPrefabs[0];
        currentObjectIndex = 0;
    }
    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //Create object based on image tracked
        // note: we'll be only using one image to track for all objects so [0]
        if (eventArgs.added.Count != 0)
        {
            originalPos = eventArgs.added[0].transform;
            arCurrentActiveObject = Instantiate(arPrefabs[0], eventArgs.added[0].transform);


        }

        //Update tracking position
        if (eventArgs.updated.Count != 0)
        {

            if (currentObjectIndex != 4)
            {
                foreach (Transform child in arCurrentActiveObject.transform)
                {
                    if (child.GetComponent<Renderer>() != null)
                    {
                        // using enabled instead to not reset object progress
                        child.GetComponent<Renderer>().enabled = eventArgs.updated[0].trackingState == TrackingState.Tracking;
                    }
                }
            }
            
            else
            {
                foreach (GameObject child in sanctuaryPansList)
                {
                    if (child.GetComponent<Renderer>() != null)
                    {
                        // using enabled instead to not reset object progress
                        child.GetComponent<Renderer>().enabled = eventArgs.updated[0].trackingState == TrackingState.Tracking;
                    }
                }
            }
            
            // we don't want to use setActive to false because it resets the current object if the user fails to track it
            //arCurrentActiveObject.SetActive(eventArgs.updated[0].trackingState == TrackingState.Tracking);


        }

    }

    public void ResetObjectProgress()
    {

        if (currentObjectIndex != 4)
        {
            arCurrentActiveObject.SetActive(false);
            arCurrentActiveObject.SetActive(true);

        }

        else
            ClearSanctuaryItems();

        StaticUIHandler.instance.ShowResetWarningPanel(false);

        if (currentObjectIndex == 0)
        {
            StaticUIHandler.instance.ShowStairsResetButton(false);
            arCurrentActiveObject.GetComponent<StairsScript>().Reset();
        }


    }
    
    private void ClearSanctuaryItems()
    {
        for (int i = 0; i < sanctuaryPansList.Count; i++)
            Destroy(sanctuaryPansList[i]);

        sanctuaryPansList.Clear();
    }

    public void StoreIndexAndDisplaySwitchWarningMessage(int index)
    {
        StaticUIHandler.instance.ShowSwitchModelWarningPanel(true);
        modelIndexToSwitchTo = index;
    }

    public void ChangeActiveObject()
    {
        if (currentObjectIndex != 4)
            Destroy(arCurrentActiveObject);

        else
            ClearSanctuaryItems();

        currentObjectIndex = modelIndexToSwitchTo;
        
        StaticUIHandler.instance.HideMenu();
        StaticUIHandler.instance.ShowResetWarningPanel(false);
        StaticUIHandler.instance.ShowSwitchModelWarningPanel(false);
        StaticUIHandler.instance.ShowSanctuaryAddButton(false);
        StaticUIHandler.instance.ShowStairsResetButton(false);

        if (currentObjectIndex == 4)
        {
            StaticUIHandler.instance.ShowSanctuaryAddButton(true);
            outlineSelection.SetActive(true);
            SanctuaryAddNewPan();

        }
        else if (currentObjectIndex != 4)
        {
            outlineSelection.SetActive(false);
            arCurrentActiveObject = Instantiate(arPrefabs[modelIndexToSwitchTo], originalPos);
        }


        if (aRScale.objectToScale != null)
            arCurrentActiveObject.transform.localScale = aRScale.objectToScale.transform.localScale; // save previous scale

    }

    private void SanctuaryAddNewPan()
    {
        sanctuaryPansList.Add(Instantiate(arPrefabs[4], originalPos));
    }

    public void SwapMoveAndRotate()
    {
        PanScript.isMove = !PanScript.isMove;
    }
    
    
}
