using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MenuClickOutsideWindow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject menuPanel;

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.selectedObject != menuPanel && menuPanel.activeInHierarchy)
            menuPanel.SetActive(false);
    }
    

}
