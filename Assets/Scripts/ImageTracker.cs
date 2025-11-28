using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    public static ImageTracker instance { get; private set; }

    private ARTrackedImageManager trackedImages;

    [SerializeField] private GameObject selectPan;
    [SerializeField] private GameObject hangoutInstantiatedObjectsHolder;
    [SerializeField] private GameObject aRScale;
    
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject[] arPrefabs;
    private int currentObjectIndex = 0;
    private GameObject arCurrentActiveObject;


    public Transform originalPos;

    private int modelIndexToSwitchTo;

    public float panOffset = 0.5f;

    // [SerializeField] private ARScale aRScale;

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
        /*
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
                
                List<GameObject> sanctuaryPansList = selectPan.GetComponent<SelectPan>().sanctuaryPansList;
                for (int i = 0; i < sanctuaryPansList.Count; i++)
                {
                    sanctuaryPansList[i].SetActive(eventArgs.updated[0].trackingState == TrackingState.Tracking);
                }
                
            }

        
       

        }
        */

    }

    public void ResetObjectProgress()
    {

        if (currentObjectIndex == 0)
        {

            arCurrentActiveObject.GetComponent<StairsScript>().Reset();

            // i don't want to deal with why the clock goes up when you reset it in the wrong time
            // but this method fixes it even if it is inefficient :">
            Destroy(arCurrentActiveObject);
            arCurrentActiveObject = Instantiate(arPrefabs[0], originalPos);

            StaticUIHandler.instance.ShowStairsResetButton(false);
            
        }
        else if (currentObjectIndex == 1)
        {
            Destroy(arCurrentActiveObject);
            arCurrentActiveObject = Instantiate(arPrefabs[1], originalPos);
            
        }
        else if (currentObjectIndex == 2)
        {
            Destroy(arCurrentActiveObject);
            arCurrentActiveObject = Instantiate(arPrefabs[2], originalPos);
            StaticUIHandler.instance.HideStairsText();

        }
        else if (currentObjectIndex == 3)
        {
            HangoutInstantiatedObjectsHolder.instance.RemoveAllInstantiatedObjects();
            Destroy(arCurrentActiveObject);
            arCurrentActiveObject = Instantiate(arPrefabs[3], originalPos);
            
        }
        else if (currentObjectIndex == 4)
        {
            selectPan.GetComponent<SelectPan>().ClearSanctuaryItems();
            selectPan.GetComponent<SelectPan>().SpawnRandomObjects();
            selectPan.GetComponent<SelectPan>().Initialize(arPrefabs[4], new UnityEngine.Vector3(originalPos.position.x,originalPos.position.y, originalPos.position.z));
            selectPan.GetComponent<SelectPan>().Initialize(arPrefabs[4], new UnityEngine.Vector3(originalPos.transform.position.x + panOffset, originalPos.transform.position.y, originalPos.transform.position.z));
            selectPan.GetComponent<SelectPan>().Initialize(arPrefabs[4], new UnityEngine.Vector3(originalPos.transform.position.x - panOffset, originalPos.transform.position.y, originalPos.transform.position.z));
        }
       
       

        StaticUIHandler.instance.ShowResetWarningPanel(false);



    }

    public void StoreIndexAndDisplaySwitchWarningMessage(int index)
    {
        StaticUIHandler.instance.ShowSwitchModelWarningPanel(true);
        modelIndexToSwitchTo = index;
    }

    public GameObject GetPrefab(int index)
    {
        return arPrefabs[index];
    }

    public void ChangeActiveObject()
    {

        if (currentObjectIndex == 3)
        {
            
             HangoutInstantiatedObjectsHolder.instance.RemoveAllInstantiatedObjects();
             hangoutInstantiatedObjectsHolder.SetActive(false);
             
        }

        else if (currentObjectIndex == 4)
            selectPan.GetComponent<SelectPan>().ClearSanctuaryItems();

        if (currentObjectIndex != 4)
            Destroy(arCurrentActiveObject);
            
       


        

        currentObjectIndex = modelIndexToSwitchTo;
        
        StaticUIHandler.instance.HideMenu();
        StaticUIHandler.instance.ShowResetWarningPanel(false);
        StaticUIHandler.instance.ShowSwitchModelWarningPanel(false);
        StaticUIHandler.instance.ShowSanctuaryAddButton(false);
        StaticUIHandler.instance.HideHangoutUI();
        StaticUIHandler.instance.HideStairsText();
        StaticUIHandler.instance.ShowStairsResetButton(false);
        aRScale.SetActive(false);

        if (currentObjectIndex == 4)
        {
            StaticUIHandler.instance.ShowSanctuaryAddButton(true);
            StaticUIHandler.instance.ShowSanctuaryMoveButton(true);
            selectPan.SetActive(true);
            selectPan.GetComponent<SelectPan>().Initialize(arPrefabs[4], new UnityEngine.Vector3(originalPos.position.x,originalPos.position.y, originalPos.position.z));
            selectPan.GetComponent<SelectPan>().AddNewPan(panOffset);
            selectPan.GetComponent<SelectPan>().AddNewPan(panOffset*-1);
            aRScale.SetActive(true);
        }
        else if (currentObjectIndex != 4)
        {
            selectPan.SetActive(false);
            StaticUIHandler.instance.ShowSanctuaryAddButton(false);
            StaticUIHandler.instance.ShowSanctuaryEditButton(false);
            StaticUIHandler.instance.ShowSanctuaryEditPanel(false);
            StaticUIHandler.instance.ShowSanctuaryMoveButton(false);
            arCurrentActiveObject = Instantiate(arPrefabs[modelIndexToSwitchTo], originalPos);
            
            if (currentObjectIndex == 3)
            {
                hangoutInstantiatedObjectsHolder.SetActive(true);
                StaticUIHandler.instance.ShowHangoutUI();
                aRScale.SetActive(true);
            }
        }

        /*
        if (aRScale.objectToScale != null)
            arCurrentActiveObject.transform.localScale = aRScale.objectToScale.transform.localScale; // save previous scale
        */
    }

 
    
}
