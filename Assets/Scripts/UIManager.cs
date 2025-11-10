using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject menuPanel;

    public void ToggleMenu()
    {
        if (menuPanel.activeInHierarchy)
            menuPanel.SetActive(false);
        else
            menuPanel.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.selectedObject != menuPanel && menuPanel.activeInHierarchy)
            menuPanel.SetActive(false);
    }
    

}
