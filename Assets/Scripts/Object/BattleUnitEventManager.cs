using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BattleUnitEventManager : MonoBehaviour
{
    private Camera _uiCamera;
    private PolygonCollider2D _collider;

    private bool _isMouseOver;
    private bool _isMousePushed;
    GameObject _target;
    
    private void Awake()
    {
        _uiCamera = FindObjectOfType<Camera>(); // 해당 오브젝트를 비추고 있는 카메라
        _collider = GetComponent<PolygonCollider2D>(); // 오브젝트의 Collider

        _target = _collider.gameObject;
    }

    private void Update()
    {
        var ray = _uiCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.collider != null && hit.collider == _collider && !_isMouseOver)
        {
            Debug.Log(hit.collider.name);
            _isMouseOver = true;
            OnMouseEnter();
        }else if (hit.collider != _collider && _isMouseOver)
        {
            _isMouseOver = false;
            OnMouseExit();
        }

        if (_isMouseOver) OnMouseOver();
        if (_isMouseOver && Input.GetMouseButtonDown(0))
        {
            _isMousePushed = true;
            OnMouseDown();
        }
        if (_isMousePushed && Input.GetMouseButton(0)) OnMouseDrag();
        if (_isMousePushed && Input.GetMouseButtonUp(0))
        {
            _isMousePushed = false;
            OnMouseUp();
            if(_isMouseOver) OnMouseUpAsButton();
        }
    }
    private void OnMouseDown()
    {
        // Debug.Log("OnMouseDown");
    }
    private void OnMouseUp()
    {
        // Debug.Log("OnMouseUp");
    }
    // private void OnMouseExit()
    // {
    //     Debug.Log("OnMouseExit");
    // }
    private void OnMouseOver()
    {
        // Debug.Log("OnMouseOver");
    }
    private void OnMouseDrag()
    {
        // Debug.Log("OnMouseDrag");
    }
    private void OnMouseUpAsButton()
    {
        // Debug.Log("OnMouseUpAsButton");
    }
    // private void OnMouseEnter()
    // {
    //     Debug.Log("OnMouseEnter");
    // }
    public void OnMouseEnter()
    {
        // Debug.Log("Mouse Enter");
        PolygonCollider2D pc = _collider;
        highlightAroundCollider(_target, pc, Color.yellow, Color.red, 0.1f);
    }
    public void OnMouseExit()
    {
        // Debug.Log("Mouse Exit");
        deleteHighlight(_target);
    }
    void highlightAroundCollider(GameObject target, Component cpType, Color beginColor, Color endColor, float hightlightSize = 0.3f)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = target.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = cpType.gameObject.AddComponent<LineRenderer>();
        }

        //2. Assign Material to the new Line Renderer
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

        float zPos = 10f;//Since this is 2D. Make sure it is in the front

        if (cpType is PolygonCollider2D)
        {
            //3. Get the points from the PolygonCollider2D
            Vector2[] pColiderPos = (cpType as PolygonCollider2D).points;

            //Set color and width
            lineRenderer.SetColors(beginColor, endColor);
            lineRenderer.SetWidth(hightlightSize, hightlightSize);

            //4. Convert local to world points
            for (int i = 0; i < pColiderPos.Length; i++)
            {
                pColiderPos[i] = cpType.transform.TransformPoint(pColiderPos[i]);
            }

            //5. Set the SetVertexCount of the LineRenderer to the Length of the points
            lineRenderer.SetVertexCount(pColiderPos.Length + 1);
            for (int i = 0; i < pColiderPos.Length; i++)
            {
                //6. Draw the  line
                Vector3 finalLine = pColiderPos[i];
                finalLine.z = zPos;
                lineRenderer.SetPosition(i, finalLine);

                //7. Check if this is the last loop. Now Close the Line drawn
                if (i == (pColiderPos.Length - 1))
                {
                    finalLine = pColiderPos[0];
                    finalLine.z = zPos;
                    lineRenderer.SetPosition(pColiderPos.Length, finalLine);
                }
            }
        }

         //Not Implemented. You can do this yourself
        else if (cpType is BoxCollider2D)
        {

        }
    }
    void deleteHighlight(GameObject target)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = target.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            
        }
        Destroy(target.GetComponent<LineRenderer>());
    }
}