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

    public void ShowStairsWinText()
    {
        stairsText.GetComponent<TextMeshProUGUI>().text = "The strange being has granted you access!";
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


}
