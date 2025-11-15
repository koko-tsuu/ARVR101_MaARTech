using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HangoutScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    
    private void OnMouseDown()
    {
        Debug.Log("hangout clicked");
       instantiatedObjects.Add(Instantiate(prefabs[Random.Range(0, prefabs.Count)], this.transform));
        
    }

}
