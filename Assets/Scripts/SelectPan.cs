using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class SelectPan : MonoBehaviour
{

    public static SelectPan instance { get; private set; }
    private Transform highlight;
    public List<GameObject> sanctuaryPansList = new List<GameObject>();
    [SerializeField] private TMP_Dropdown emissionDropdown;
    [SerializeField] private TMP_Dropdown fontDropdown;
    [SerializeField] private TMP_InputField textInputField;
    [SerializeField] private TMP_InputField fontSizeInputField;

    [SerializeField] private TMP_InputField particleSizeInputField;
    
    [SerializeField] private TMP_InputField spawnRateInputField;
   
    
    [SerializeField] private GameObject previewImage;
    [SerializeField] private GameObject noImagePresentText;
    [SerializeField] private GameObject customizeParticleTextPanel;
    [SerializeField] private GameObject customizeParticleImagePanel;
  
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

    void Update()
    {
        if (highlight == null)
        {
            StaticUIHandler.instance.ShowSanctuaryEditButton(false);
            StaticUIHandler.instance.ShowSanctuaryEditPanel(false);
        }
        else
        {
            StaticUIHandler.instance.ShowSanctuaryEditButton(true);
        }
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

        
        ReloadEditPanel();

    }


    public void RemoveHighlight()
    {
        if (highlight != null)
            highlight.gameObject.GetComponent<Outline>().enabled = false;
        highlight = null;
        
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

            if (highlight.parent.gameObject == sanctuaryPansList[i])
            {
    
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
        
    }

    private void ReloadEditPanel()
    {
        // load everything
        // to put the emitting words here
        PanScript loadPanEdit =  highlight.gameObject.GetComponent<PanScript>();
        panColorButton.GetComponent<Image>().color =  loadPanEdit.panColor;

        flexibleColorPicker_PanColor.color = loadPanEdit.panColor;

        spawnRateInputField.text = loadPanEdit.spawnRate.ToString();
        particleSizeInputField.text = loadPanEdit.startSize.ToString();


        if (loadPanEdit.isEmittingWords)
        {
            emissionDropdown.value = 0;
            customizeParticleTextPanel.SetActive(true);
            customizeParticleImagePanel.SetActive(false);
            fontDropdown.value = loadPanEdit.fontSelectedIndex;
            textInputField.text = loadPanEdit.word;
            fontSizeInputField.text = DynamicTextManager.defaultData[loadPanEdit.fontSelectedIndex].sizes[0].ToString();

            fontColorButton.GetComponent<Image>().color = DynamicTextManager.defaultData[loadPanEdit.fontSelectedIndex].colours[0];
            flexibleColorPicker_TextColor.color = DynamicTextManager.defaultData[loadPanEdit.fontSelectedIndex].colours[0];

        }
        else {
            emissionDropdown.value = 1;
            customizeParticleTextPanel.SetActive(false);
            customizeParticleImagePanel.SetActive(true);
            
            spawnRateInputField.text = loadPanEdit.spawnRate.ToString();
            particleSizeInputField.text = loadPanEdit.startSize.ToString();

            


            if (loadPanEdit.previewSprite == null)
            {
                previewImage.SetActive(false);
                noImagePresentText.SetActive(true);
            }
        
            else
            {
                previewImage.SetActive(true);
                noImagePresentText.SetActive(false);
                previewImage.GetComponent<Image>().sprite = loadPanEdit.previewSprite;
            }
        }

        
        
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

    public void ChangeEmission()
    {
        PanScript panScript = highlight.GetComponent<PanScript>();

        if (emissionDropdown.value == 0)
        {
            customizeParticleTextPanel.SetActive(true);
            customizeParticleImagePanel.SetActive(false);
            panScript.isEmittingWords = true;
            panScript.StartEmittingText();     
            
        }
        else
        {
            customizeParticleTextPanel.SetActive(false);
            customizeParticleImagePanel.SetActive(true);
            panScript.isEmittingWords = false;
            panScript.StartEmittingParticles();
        }

        ReloadEditPanel();
    }

     public void LoadUserTexture()
    {
        
        #if UNITY_STANDALONE || UNITY_EDITOR
                string path = UnityEditor.EditorUtility.OpenFilePanel(
                    "Select an Image", "", "png,jpg,jpeg");

                if (!string.IsNullOrEmpty(path))
                {
                    StartCoroutine(LoadTextureFromFile(path));
                }
        #elif UNITY_ANDROID
                string mobilePath = "";
                if(NativeFilePicker.CheckPermission(true))
                {
                    
                    NativeFilePicker.PickFile((mobilePath) =>
                    {
                        if (mobilePath == null)
                        {
                            Debug.Log("Canceled file operation");
                        }
                        else 
                            StartCoroutine(LoadTextureFromFile(mobilePath));
                    }, new string[] {"image/*"});
                }
                else
                {
                    NativeFilePicker.RequestPermissionAsync(true);
                }
                
        #endif
       

    }

    public void ChangeSpawnRate()
    {
         if (float.TryParse(spawnRateInputField.text, out float result))
        {
            ParticleSystem particleSystem = highlight.GetChild(0).GetComponent<ParticleSystem>();
            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;

            emissionModule.rateOverTime = result;
            highlight.GetComponent<PanScript>().spawnRate = result;
        }
    }

    public void ChangeParticleSize()
    {
         if (float.TryParse(particleSizeInputField.text, out float result))
        {
            ParticleSystem particleSystem = highlight.GetChild(0).GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = particleSystem.main;

            mainModule.startSize = result;
            highlight.GetComponent<PanScript>().startSize = result;
        }
        
         
    }

    private IEnumerator LoadTextureFromFile(string path)
    {
        ParticleSystem particleSystem = highlight.GetChild(0).GetComponent<ParticleSystem>();
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();

    
        // Create Texture2D from image
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData); // Auto-resizes the texture

        // Create a new material
        Material newMaterial = new Material(Shader.Find("Standard")); // Use the Standard Shade

        // Assign the loaded texture to the material's Albedo map
        newMaterial.mainTexture = tex;
        
        // Assign to Particle System Renderer
        renderer.material = newMaterial;

        // Update preview sprite
        Sprite previewSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 100);
        previewImage.GetComponent<Image>().sprite = previewSprite;
        highlight.GetComponent<PanScript>().previewSprite = previewSprite;
        
        noImagePresentText.SetActive(false);
        previewImage.SetActive(true);

        particleSystem.Clear();
        particleSystem.Play();

        yield return null;
    }
    
    

}