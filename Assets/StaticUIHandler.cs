using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public  class StaticUIHandler : MonoBehaviour
{

    public static StaticUIHandler instance { get; private set; }

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject stairsResetButton;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one LowOrderAnalyticsManager Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public void ToggleMenu()
    {
        if (menuPanel.activeInHierarchy)
            menuPanel.SetActive(false);
        else
            menuPanel.SetActive(true);
    }

    public void ShowStairsResetButton()
    {
        stairsResetButton.SetActive(true);
    }

    public void HideStairsResetButton()
    {
        stairsResetButton.SetActive(false);
    }
    

}
