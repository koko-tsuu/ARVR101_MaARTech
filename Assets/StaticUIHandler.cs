using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public  class StaticUIHandler : MonoBehaviour
{

    public static StaticUIHandler instance { get; private set; }

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject resetWarningPanel;
    [SerializeField] private GameObject switchModelWarningPanel;

    [SerializeField] private GameObject stairsResetButton;
    [SerializeField] private GameObject stairsText;
    [SerializeField] private GameObject sanctuaryAddButton;
    [SerializeField] private GameObject sanctuaryEditButton;
    [SerializeField] private GameObject sanctuaryEditPanel;
    [SerializeField] private GameObject sanctuaryMoveButton;
    [SerializeField] private GameObject sanctuaryRotateButton;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one StaticUIHandler Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public bool CheckIfAnyPanelIsActive()
    {
        if (menuPanel.activeInHierarchy || resetWarningPanel.activeInHierarchy || switchModelWarningPanel.activeInHierarchy)
            return true;
        return false;
    }

    public void ToggleMenu()
    {
        if (menuPanel.activeInHierarchy)
            menuPanel.SetActive(false);
        else
            menuPanel.SetActive(true);
    }

    public void ShowStairsResetButton(bool value)
    {
        stairsResetButton.SetActive(value);
    }

    public void ShowResetWarningPanel(bool value)
    {
        resetWarningPanel.SetActive(value);
    }

    public void ShowSwitchModelWarningPanel(bool value)
    {
        switchModelWarningPanel.SetActive(value);
    }

    public void ShowSanctuaryAddButton(bool value)
    {
        sanctuaryAddButton.SetActive(value);
    }

    public void ShowSanctuaryEditButton(bool value)
    {
        sanctuaryEditButton.SetActive(value);
    }

     public void ShowSanctuaryMoveButton(bool value)
    {
        sanctuaryMoveButton.SetActive(value);
    }

    public void ShowSanctuaryEditPanel(bool value)
    {
        sanctuaryEditPanel.SetActive(value);
    }

    public void HideAllSanctuaryUI()
    {
        sanctuaryAddButton.SetActive(false);
        sanctuaryEditButton.SetActive(false);
        sanctuaryMoveButton.SetActive(false);
        sanctuaryRotateButton.SetActive(false);
        sanctuaryEditPanel.SetActive(false);
    }

    public void ShowStairsWinText()
    {
        stairsText.GetComponent<TextMeshProUGUI>().text = "The strange being has granted you access!";
        stairsText.SetActive(true);
    }

    public void ShowIdeationWinText()
    {
        stairsText.GetComponent<TextMeshProUGUI>().text = "You've come up with an idea!";
        stairsText.SetActive(true);
    }
    
    public void ShowStairsLoseText()
    {
        stairsText.GetComponent<TextMeshProUGUI>().text = "Oops! You got monched.";
        stairsText.SetActive(true);
    }

    public void HideStairsText()
    {
        stairsText.SetActive(false);
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
    }

    public void SwapMoveRotateButtons()
    {
        if (sanctuaryMoveButton.activeInHierarchy)
        {
            sanctuaryMoveButton.SetActive(false);
            sanctuaryRotateButton.SetActive(true);
        }
        else
        {
            sanctuaryMoveButton.SetActive(true);
            sanctuaryRotateButton.SetActive(false);
        }
    }

    
    


}
