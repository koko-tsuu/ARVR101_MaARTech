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
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject[] arPrefabs;
    private int currentObjectIndex;
    private GameObject arCurrentActiveObject;

    private Transform originalPos;

    private int modelIndexToSwitchTo;

    [SerializeField] private ARScale aRScale;

    void Awake()
    {
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
            Debug.Log("got here");
            originalPos = eventArgs.added[0].transform;
            arCurrentActiveObject = Instantiate(arPrefabs[0], eventArgs.added[0].transform);


        }

        //Update tracking position
        if (eventArgs.updated.Count != 0)
        {
            foreach (Transform child in arCurrentActiveObject.transform)
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    // using enabled instead to not reset object progress
                    child.GetComponent<Renderer>().enabled = eventArgs.updated[0].trackingState == TrackingState.Tracking;
                }
            }
            // we don't want to use setActive to false because it resets the current object if the user fails to track it
            //arCurrentActiveObject.SetActive(eventArgs.updated[0].trackingState == TrackingState.Tracking);


        }

    }

    public void ResetObjectProgress()
    {
        arCurrentActiveObject.SetActive(false);
        arCurrentActiveObject.SetActive(true);

        StaticUIHandler.instance.ShowResetWarningPanel(false);

        if (currentObjectIndex == 0)
        {
            StaticUIHandler.instance.ShowStairsResetButton(false);
            arCurrentActiveObject.GetComponent<StairsScript>().Reset();
        }


    }

    public void StoreIndexAndDisplaySwitchWarningMessage(int index)
    {
        StaticUIHandler.instance.ShowSwitchModelWarningPanel(true);
        modelIndexToSwitchTo = index;
    }
    
    public void ChangeActiveObject()
    {
        //StaticUIHandler.instance.s
        Destroy(arCurrentActiveObject);

        arCurrentActiveObject = Instantiate(arPrefabs[modelIndexToSwitchTo], originalPos);
        currentObjectIndex = modelIndexToSwitchTo;

        StaticUIHandler.instance.ShowResetWarningPanel(false);
        StaticUIHandler.instance.ShowSwitchModelWarningPanel(false);

        if (currentObjectIndex == 4)
        {
            StaticUIHandler.instance.ShowSanctuaryAddButton(true);
        }
        else if (currentObjectIndex != 4)
        {
            StaticUIHandler.instance.ShowSanctuaryAddButton(false);
        }

        if (currentObjectIndex == 0)
        {
            StaticUIHandler.instance.ShowStairsResetButton(false);
        }

        if (aRScale.objectToScale != null)
            arCurrentActiveObject.transform.localScale = aRScale.objectToScale.transform.localScale; // save previous scale

    }
}
