/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSystem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public GUIStyle MouseDragSkin;
    private bool isDragging;
    private Vector3 mouseDownPoint;
    private Vector2 mouseDragStart;
    Rect properRect;
    Rect dRect;
    private void Start() { }

    private void OnGUI()
    {
        if (isDragging)
        {
            Vector3 currentMousePoint = new BattleCursor().GetWorldPointUnderCursor();
            float boxWidth = Camera.main.WorldToScreenPoint(mouseDownPoint).x - Camera.main.WorldToScreenPoint(currentMousePoint).x;
            float boxHeight = Camera.main.WorldToScreenPoint(mouseDownPoint).y - Camera.main.WorldToScreenPoint(currentMousePoint).y;
            float boxLeft = Input.mousePosition.x;
            float boxTop = (Screen.height - Input.mousePosition.y) - boxHeight;

            dRect = new Rect(boxLeft, boxTop, boxWidth, boxHeight);
            properRect = Rect.MinMaxRect(
                 dRect.center.x - Mathf.Abs(dRect.width / 2),
                 dRect.center.y - Mathf.Abs(dRect.height / 2),
                 dRect.center.x + Mathf.Abs(dRect.width / 2),
                 dRect.center.y + Mathf.Abs(dRect.height / 2));

            GUI.Box(properRect, "");
        }
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        mouseDownPoint = new BattleCursor().GetWorldPointUnderCursor();
        mouseDragStart = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        GameObject[] group = GameObject.FindGameObjectsWithTag("unit_tag");
        if (group.Length > 0)
        {
            Vector3 topLeftPosition = new Vector3(properRect.position.x, Screen.height - properRect.y);
            Vector3 bottomRightPosition = new Vector3(properRect.position.x + properRect.width, Screen.height - (properRect.position.y + properRect.height));
            for (int i = 0; i < group.Length; i++)
            {
                Vector3 objectPosition = Camera.main.WorldToScreenPoint(group[i].transform.position);
                Debug.DrawLine(objectPosition, objectPosition, Color.blue, 10);
                Debug.DrawLine(Camera.main.WorldToScreenPoint(group[i].transform.position), topLeftPosition, Color.green, 10);
                Debug.DrawLine(Camera.main.WorldToScreenPoint(group[i].transform.position), bottomRightPosition, Color.black, 10);
                if (objectPosition.x > topLeftPosition.x && objectPosition.x < bottomRightPosition.x
                    && objectPosition.y < topLeftPosition.y && objectPosition.y > bottomRightPosition.y)
                {
                    Debug.Log("object is within my bounds");
                    //var entity = group[i].GetComponent<GameObjectEntity>();

                }
            }
        }

    }
}
*/