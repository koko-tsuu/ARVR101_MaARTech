using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject[] arPrefabs;
    GameObject arFloor;
    GameObject arCurrentActiveObject;

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
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
            arFloor = Instantiate(groundPrefab, eventArgs.added[0].transform);
        }
       
        //Update tracking position
        if (eventArgs.updated.Count != 0)
        {
             arFloor.SetActive(eventArgs.updated[0].trackingState == TrackingState.Tracking);
        }
        
    }
}
