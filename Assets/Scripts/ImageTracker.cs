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
    GameObject arCurrentActiveObject;

    Vector3 objectPosVector;

    [SerializeField] private ARScale aRScale;

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
        arCurrentActiveObject = arPrefabs[0];
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
            objectPosVector = new Vector3(eventArgs.added[0].transform.position.x, eventArgs.added[0].transform.position.y, eventArgs.added[0].transform.position.z);
            arCurrentActiveObject = Instantiate(arPrefabs[0], objectPosVector, Quaternion.identity);
           

            
            
        }

        //Update tracking position
        if (eventArgs.updated.Count != 0)
        {
            arCurrentActiveObject.SetActive(eventArgs.updated[0].trackingState == TrackingState.Tracking);
        }

    }

    public void ChangeActiveObject(int index)
    {
        Destroy(arCurrentActiveObject);

        arCurrentActiveObject = Instantiate(arPrefabs[index], objectPosVector, Quaternion.identity);

        if (aRScale.objectToScale != null)
            arCurrentActiveObject.transform.localScale = aRScale.objectToScale.transform.localScale; // save previous scale

    }
}
