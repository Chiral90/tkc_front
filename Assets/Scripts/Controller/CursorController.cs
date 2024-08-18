using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CursorController : MonoBehaviour
{
    [SerializeField] Transform tf_cursor;   // 커서의 현재 위치
    //[SerializeField] float dragSpeed = 10.0f;   // 화면 움직임 속도
    private float firstClickPointX;
    bool _selected;
    GameObject _selectedTarget;
    private Camera _uiCamera;
    private PolygonCollider2D _collider;
    bool _isMouseOver;
    bool _isMousePushed;

    // Start is called before the first frame update
    void Start()
    {
        _uiCamera = FindObjectOfType<Camera>(); // 해당 오브젝트를 비추고 있는 카메라
    }

    // Update is called once per frame
    void Update()
    {
        //CursorMoving();
    }
    void CursorMoving()
    {
        // 마우스 이동
        //float x = Input.mousePosition.x - (Screen.width / 2);
        //float y = Input.mousePosition.y - (Screen.height / 2);
        tf_cursor.localPosition = Input.mousePosition;  // 현재 마우스의 위치
        // 마우스 가두기 (범위 지정)
        float tmp_cursorPosX = tf_cursor.localPosition.x;
        float tmp_cursorPosY = tf_cursor.localPosition.y;

        float min_width = -Screen.width / 2;
        float max_width = Screen.width / 2;
        float min_height = -Screen.height / 2;
        float max_height = Screen.height / 2;
        int padding = 20;	// 값은 자유

        tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
        tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);
        
        tf_cursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);
    }
    void SwitchSelected() { _selected = !_selected; }
    void ResetSelected() { _selected = false; _selectedTarget = null; }
    void SelectTarget()
    {
        var ray = _uiCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider != null && hit.collider == _collider && !_isMouseOver)
        {
            Debug.Log(hit.collider.name);
            _isMouseOver = true;
            OnMouseEnter();
        }
        else if (hit.collider != _collider && _isMouseOver)
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
            if (_isMouseOver) OnMouseUpAsButton();
        }
    }
    private void OnMouseDown()
    {
        // Debug.Log("OnMouseDown");
        PolygonCollider2D pc = _collider;
        highlightAroundCollider(_selectedTarget, pc, Color.yellow, Color.red, 0.1f);
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
    }
    public void OnMouseExit()
    {
        if (!_selected)
        {
            // Debug.Log("Mouse Exit");
            deleteHighlight(_selectedTarget);
            ResetSelected();
        }
    }
    void highlightAroundCollider(GameObject target, Component cpType, Color beginColor, Color endColor, float highlightSize = 0.3f)
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
            //lineRenderer.SetColors(beginColor, endColor);
            //lineRenderer.SetWidth(highlightSize, highlightSize);
            lineRenderer.startWidth = lineRenderer.endWidth = highlightSize;
            lineRenderer.startColor = beginColor;
            lineRenderer.endColor = endColor;

            //4. Convert local to world points
            for (int i = 0; i < pColiderPos.Length; i++)
            {
                pColiderPos[i] = cpType.transform.TransformPoint(pColiderPos[i]);
            }

            //5. Set the SetVertexCount of the LineRenderer to the Length of the points
            //lineRenderer.SetVertexCount(pColiderPos.Length + 1);
            lineRenderer.positionCount = pColiderPos.Length + 1;
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
