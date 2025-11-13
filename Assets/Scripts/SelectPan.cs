using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPan : MonoBehaviour
{

    public static SelectPan instance { get; private set; }
    private Transform highlight;
    public List<GameObject> sanctuaryPansList = new List<GameObject>();

    public GameObject panPrefab;
    public Transform originalPos;

    private bool wasButtonClicked = false;

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
         if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // if (!Physics.Raycast(ray, out hit, 100))
                //     RemoveHighlight();

        
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
    
    public void EditPan()
    {
        Debug.Log("edit pan: " + highlight.gameObject.name);
    }

    public void SwapMoveAndRotate()
    {
        PanScript.isMove = !PanScript.isMove;
        StaticUIHandler.instance.SwapMoveRotateButtons();
    }
    
    

}