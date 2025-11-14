using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectPan : MonoBehaviour
{

    public static SelectPan instance { get; private set; }
    private Transform highlight;
    public List<GameObject> sanctuaryPansList = new List<GameObject>();

    [SerializeField] private TMP_Dropdown fontDropdown;
    [SerializeField] private TMP_InputField textInputField;
    [SerializeField] private TMP_InputField fontSizeInputField;

    [SerializeField] private Button panColorButton;
    [SerializeField] private Button fontColorButton;

    [SerializeField] private FlexibleColorPicker flexibleColorPicker_PanColor;
    [SerializeField] private FlexibleColorPicker flexibleColorPicker_TextColor;

    private GameObject panPrefab;
    private Transform originalPos;


    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one SelectPan Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }
    
    public void Highlight(Transform transform)
    {

        if (transform.gameObject.GetComponent<Outline>() != null)
        {
            transform.gameObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            Outline outline = transform.gameObject.AddComponent<Outline>();
            outline.enabled = true;
            transform.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
            transform.gameObject.GetComponent<Outline>().OutlineWidth = 10.0f;
        }

        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            
        }


        highlight = transform;
        transform.gameObject.GetComponent<Outline>().enabled = true;

        StaticUIHandler.instance.ShowSanctuaryEditButton(true);

    }


    public void RemoveHighlight()
    {
        if (highlight != null)
            highlight.gameObject.GetComponent<Outline>().enabled = false;
        highlight = null;
        StaticUIHandler.instance.ShowSanctuaryEditButton(false);
    }


    public void ClearSanctuaryItems()
    {
        for (int i = 0; i < sanctuaryPansList.Count; i++)
            Destroy(sanctuaryPansList[i]);

        sanctuaryPansList.Clear();
    }

    public void Initialize(GameObject obj, Transform transform)
    {
        panPrefab = obj;
        originalPos = transform;
        sanctuaryPansList.Add(Instantiate(obj, transform));
    }

    public void AddNewPan()
    {
        sanctuaryPansList.Add(Instantiate(panPrefab, originalPos));
    }

    public void RemovePan()
    {
        for (int i = 0; i < sanctuaryPansList.Count; i++)
        {
            Debug.Log("got here1");
            Debug.Log(highlight.parent.gameObject.name);
            Debug.Log(sanctuaryPansList[i].name);
            if (highlight.parent.gameObject == sanctuaryPansList[i])
            {
                Debug.Log("got here");
                Destroy(sanctuaryPansList[i]);
                sanctuaryPansList.RemoveAt(i);
                highlight = null;
                break;
            }
        }

        RemoveEditPanPanel();

        
    }
    
    public void EditPan()
    {
        StaticUIHandler.instance.ShowSanctuaryEditPanel(true);

        // load everything
        // to put the emitting words here
        PanScript loadPanEdit =  highlight.gameObject.GetComponent<PanScript>();

        fontDropdown.value = loadPanEdit.fontSelectedIndex;
        textInputField.text = loadPanEdit.word;
        fontSizeInputField.text = DynamicTextManager.defaultData[loadPanEdit.fontSelectedIndex].sizes[0].ToString();

        panColorButton.GetComponent<Image>().color =  loadPanEdit.panColor;
        fontColorButton.GetComponent<Image>().color = DynamicTextManager.defaultData[loadPanEdit.fontSelectedIndex].colours[0];

        flexibleColorPicker_PanColor.color = loadPanEdit.panColor;
        flexibleColorPicker_TextColor.color = DynamicTextManager.defaultData[loadPanEdit.fontSelectedIndex].colours[0];
    }

    public void RemoveEditPanPanel()
    {
        StaticUIHandler.instance.ShowSanctuaryEditPanel(false);
    }

    public void ChangeFont()
    {
        highlight.gameObject.GetComponent<PanScript>().fontSelectedIndex = fontDropdown.value;
    }

    public void ChangeFontColor()
    {
        DynamicTextManager.defaultData[highlight.gameObject.GetComponent<PanScript>().fontSelectedIndex].colours[0] = flexibleColorPicker_TextColor.color;
        fontColorButton.GetComponent<Image>().color = flexibleColorPicker_TextColor.color;
    }

    public void ChangeFontSize()
    {
        if (float.TryParse(fontSizeInputField.text, out float result))
            DynamicTextManager.defaultData[highlight.gameObject.GetComponent<PanScript>().fontSelectedIndex].sizes[0] = result;
    }

    public void ChangeText()
    {
        highlight.gameObject.GetComponent<PanScript>().word = textInputField.text;
    }

    public void ChangePanColor()
    {
        highlight.gameObject.GetComponent<Renderer>().material.color = flexibleColorPicker_PanColor.color;
        highlight.gameObject.GetComponent<PanScript>().panColor = flexibleColorPicker_PanColor.color;
        panColorButton.GetComponent<Image>().color =  flexibleColorPicker_PanColor.color;
        
    }

    public void SwapMoveAndRotate()
    {
        PanScript.isMove = !PanScript.isMove;
        StaticUIHandler.instance.SwapMoveRotateButtons();
    }
    
    

}