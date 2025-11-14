using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MenuClickOutsideWindow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private GameObject flexibleColorPicker_PanColor;
    [SerializeField] private GameObject flexibleColorPicker_TextColor;

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.selectedObject != menuPanel && menuPanel.activeInHierarchy)
            menuPanel.SetActive(false);

        if (eventData.selectedObject != flexibleColorPicker_PanColor && flexibleColorPicker_PanColor.activeInHierarchy)
            flexibleColorPicker_PanColor.SetActive(false);

        if (eventData.selectedObject != flexibleColorPicker_TextColor && flexibleColorPicker_TextColor.activeInHierarchy)
            flexibleColorPicker_TextColor.SetActive(false);
    }
    
    

}
