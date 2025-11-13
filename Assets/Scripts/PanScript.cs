using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanScript : MonoBehaviour
{
    public static bool isMove = true;
    UnityEngine.Vector3 touchPosition;

    private UnityEngine.Vector3 GetTouchPos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        touchPosition = Input.mousePosition - GetTouchPos();
        OutlineSelection.instance.Highlight(this.gameObject.transform);
        
    }

    private void OnMouseDrag()
    {
        if (isMove)
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - touchPosition);
        else
        {
            Touch screenTouch = Input.GetTouch(0);
            if (screenTouch.phase == TouchPhase.Moved)
            {
                transform.Rotate(0f, screenTouch.deltaPosition.x, 0f);
            }
        }
    }

}
