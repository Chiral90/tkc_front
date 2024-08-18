using System;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
using UnityEngine.UIElements;

public class BattleUnitEventManager : MonoBehaviour
{
    Camera _uiCamera;
    PolygonCollider2D _collider;
    Rigidbody2D _rb;
    BattleCursor _battleCursor;
    BattleUnit _battleUnit;

    bool _isMouseOver;
    bool _isMousePushed;
    GameObject _target;

    bool _isMovingStart = false;
    public bool IsMovingStart
    {
        get { return _isMovingStart; }
        set { _isMovingStart = value; }
    }
    bool _forceStopMoving = false;
    bool _isMovingEnd = true;
    bool _isMoving = false;
    bool _isAttacking = false;
    float _attackStay = 0;
    float _attackDelay = 2.0f;

    //
    Queue<Vector2> _path;
    public Queue<Vector2> Path { get { return _path; } }

    float _pathMagnitude;
    Vector2 _startPosition;
    Vector2 _nextOriginalPosition;
    public bool IsMoving
    {
        get { return _isMoving; }
        set { _isMoving = value; }
    }
    string HighlightSortingLayer = "Highlight";
    int HighlightSortingOrderInLayer = 1;

    private void Awake()
    {
        _uiCamera = FindObjectOfType<Camera>(); // 해당 오브젝트를 비추고 있는 카메라
        _collider = this.gameObject.GetComponent<PolygonCollider2D>(); // 오브젝트의 Collider
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _battleCursor = GameObject.Find("BattleCursor").GetComponent<BattleCursor>();
        _battleUnit = this.gameObject.GetComponent<BattleUnit>();
        //_path = new Queue<Vector3>();
        _path = new Queue<Vector2>();

        _target = _collider.gameObject;
    }

    private void Update()
    {
        // mouse action
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
        // show path
        if (_battleUnit.Selected && Input.GetMouseButtonDown(1))
        {
            LineRenderer lineRenderer = _target.transform.Find("Path").GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                HighlightPath(_target, RetruneScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f)), Color.red, Color.green);
            }
            else
            {
                AddPath(
                    new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x
                    , Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
            }
        }
        // move
        if (_isMoving)
        {
            if (!_isMovingStart && _isMovingEnd)
            {
                CalculatePath();
                SetVelocity();
            }
            else if (_isMovingStart && !_isMovingEnd)
            {
                CompareDestination();
            }
        }

        if (_isAttacking)
        {
            _attackStay += Time.deltaTime;
        }
    }
    private void OnMouseDown()
    {
        // Debug.Log("OnMouseDown");
        if (_battleUnit.Selected)
        {
            RemoveSelectedUnit();
        }
        else
        {
            AddSelectedUnit();
        }
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
        // Debug.Log("Mouse Exit");
        //deleteHighlight(_target);
    }
    void HighlightAroundCollider(GameObject target, Component cpType, Color beginColor, Color endColor, float highlightSize = 0.3f)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = target.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = cpType.gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.useWorldSpace = false;
        lineRenderer.sortingLayerName = HighlightSortingLayer;
        lineRenderer.sortingOrder = HighlightSortingOrderInLayer;

        //2. Assign Material to the new Line Renderer
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

        float zPos = 0f;//Since this is 2D. Make sure it is in the front

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
                pColiderPos[i] = (pColiderPos[i]);
                //pColiderPos[i] = cpType.transform.TransformPoint(pColiderPos[i]);
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
    void DeleteHighlight(GameObject target)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = target.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            
        }
        Destroy(target.GetComponent<LineRenderer>());
    }
    void HighlightPath(GameObject target, Vector3 destination, Color beginColor, Color endColor, float highlightSize = 0.1f)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = target.transform.Find("Path").GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = target.transform.Find("Path").AddComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerName = HighlightSortingLayer;
        lineRenderer.sortingOrder = HighlightSortingOrderInLayer;

        //2. Assign Material to the new Line Renderer
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

        float zPos = -1f;
        //Since this is 2D. Make sure it is in the front
        //3. Get the points from the PolygonCollider2D

        //Set color and width
        //lineRenderer.SetColors(beginColor, endColor);
        //lineRenderer.SetWidth(highlightSize, highlightSize);
        lineRenderer.startWidth = lineRenderer.endWidth = highlightSize;
        lineRenderer.startColor = beginColor;
        lineRenderer.endColor = endColor;

        //5. Set the SetVertexCount of the LineRenderer to the Length of the points
        //6. Draw the  line
        lineRenderer.SetPosition(0, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, zPos));
        Vector3 endPoint = destination;
        endPoint.z = zPos;
        lineRenderer.SetPosition(1, endPoint);

        //7. Check if this is the last loop. Now Close the Line drawn
    }

    void AddPath(Vector2 destination)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = _target.transform.Find("Path").GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            return;
        }

        float zPos = 0f;
        //5. Set the SetVertexCount of the LineRenderer to the Length of the points
        var lastIndex = lineRenderer.positionCount;
        //lineRenderer.SetVertexCount(lastIndex + 1);
        lineRenderer.positionCount = lastIndex + 1;
        //6. Draw the  line
        lineRenderer.SetPosition(lastIndex, new Vector3(destination.x, destination.y, zPos));
        _path.Enqueue(destination);

        //7. Check if this is the last loop. Now Close the Line drawn
    }

    void SetVelocity()
    {
        _rb.velocity = _battleUnit.MoveDirection * _battleUnit.moveSpeed;
    }
    void ToggleUnitPath(bool status, GameObject unit)
    {
        if (unit.transform.Find("Path").GetComponent<LineRenderer>())
        {
            if (status)
            {
                unit.transform.Find("Path").GetComponent<LineRenderer>().startWidth = 0.1f;
                unit.transform.Find("Path").GetComponent<LineRenderer>().endWidth = 0.1f;
            }
            else
            {
                unit.transform.Find("Path").GetComponent<LineRenderer>().startWidth = 0;
                unit.transform.Find("Path").GetComponent<LineRenderer>().endWidth = 0;
            }
        }
    }
    void DeleteUnitPath()
    {
        if (_target.transform.Find("Path").GetComponent<LineRenderer>())
        {
            Destroy(_target.transform.Find("Path").GetComponent<LineRenderer>());
        }
    }
    public void AbleToHighlightSelectedUnit()
    {
        if (_battleUnit.Selected)
        {
            HighlightAroundCollider(_target, _collider, Color.yellow, Color.green, 0.1f);
            ToggleUnitPath(true, _target);
        }
    }
    public void UnableToHighlightSelectedUnit()
    {
        if (!_battleUnit.Selected)
        {
            DeleteHighlight(_target);
            ToggleUnitPath(false, _target);
        }
    }
    void AddSelectedUnit()
    {
        _battleCursor.AddSelectedUnit(this.gameObject);
    }
    void RemoveSelectedUnit()
    {
        _battleCursor.RemoveSelectedUnit(this.gameObject);
    }

    void CalculatePath()
    {
        LineRenderer lr = this.transform.Find("Path").GetComponent<LineRenderer>();
        if (lr != null)
        {
            Vector3[] _v = new Vector3[lr.positionCount];
            var _i = lr.GetPositions(_v);
            foreach (var v in _v)
            {
                _path.Enqueue(new Vector2(v.x, v.y));
            }
        }
        if (_path.Count > 0)
        {
            CalculateDirection();
        }
    }
    void CalculateDirection()
    {
        LineRenderer lr = this.transform.Find("Path").GetComponent<LineRenderer>();
        if (lr != null)
        {
            Vector2 _start;
            Vector2 _end;

            // first of path need to be poped
            if (this._path.Peek() == new Vector2(lr.GetPosition(0).x, lr.GetPosition(0).y))
            {
                _start = this._path.Dequeue();
            }
            else
            {
                _start = _nextOriginalPosition;
            }
            _end = this._path.Dequeue();

            Vector2 _path = _end - _start;
            Vector2 _direction = _path.normalized;
            _battleUnit.MoveDirection = _direction;
            _pathMagnitude = _path.magnitude;
            _startPosition = _start;
            _isMovingStart = true;
            _isMovingEnd = false;
        }
    }
    void CompareDestination()
    {
        if (Math.Abs(_pathMagnitude) 
            > Math.Abs((new Vector2(_target.transform.position.x, _target.transform.position.y) - _startPosition).magnitude))
        {
            return;
        }
        else
        {
            _nextOriginalPosition = this.transform.position;
            // calculate next path end move
            if (_path.Count > 0)
            {
                CalculateDirection();
                SetVelocity();
            }
            else
            {
                EndMovingUnit();
            }
        }
    }

    void EndMovingUnit()
    {
        if (_forceStopMoving || _isMoving)
        {
            ResetPathStack();
            DeleteUnitPath();
        }
        _isMoving = false;
        _isMovingEnd = true;
        _isMovingStart = false;
    }
    void ResetPathStack()
    {
        Destroy(this.transform.Find("Path").GetComponent<LineRenderer>());
        _path.Clear();
        _nextOriginalPosition = _target.transform.position;
        _battleUnit.MoveDirection = new Vector2(0, 0);
        _rb.velocity = new Vector2(0, 0);
    }
    Vector3 RetruneScreenToWorldPoint(Vector3 p)
    {
        return Camera.main.ScreenToWorldPoint(p);
    }
    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌을 시작했을 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 이 컴포넌트가 부착된 게임 오브젝트의 콜라이더와 충돌한 게임 오브젝트 가져오기
        var obj = collision.gameObject;
        // 특정 컴포넌트 가져오기
        var component = collision.gameObject;
        // 콜라이더 가져오기
        var collider = collision.collider;
        Debug.Log(this.name + " starts collision! [" + component.name + "]");
        StartBattle(collision);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이 컴포넌트가 부착된 게임 오브젝트의 콜라이더와 충돌한 게임 오브젝트 가져오기
        var obj = collision.gameObject;
        // 특정 컴포넌트 가져오기
        var component = collision.gameObject;
        // 콜라이더 가져오기
        var collider = collision.GetComponent<Collider2D>();
        Debug.Log("start trigger! [" + component.name + "]");
    }

    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌중일 때
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("collision ...! / " + collision.gameObject.GetComponent<Collider2D>().name);
        Attack(collision);
    }

    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌이 끝났을 때
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("end collision!");
        EndBattle(collision);
    }
    void StartBattle(Collision2D collision)
    {
        _isAttacking = true;
        CreateBattleSign(collision);
        ResetPathStack();
        EndMovingUnit();
    }
    void CreateBattleSign(Collision2D collision)
    {
        var component = collision.gameObject;
        var collider = collision.collider;
        if (collider && collider.tag == "BattleUnit")
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector3 position = contact.point;
            GameObject _sign = Resources.Load<GameObject>("Prefabs/BattleSign");
            GameObject _p = Instantiate(_sign, this.transform.Find("State").transform, false);//prefab 적용 시
            _p.transform.position = new Vector3(position.x, position.y, -9);
            Variables.Object(_p).Set("enemy", component.GetComponent<BattleUnit>());
        }
    }
    void Attack(Collision2D collision)
    {
        if (_attackStay >= _attackDelay)
        {
            //a second has passed so do something
            _battleUnit.Attack(collision.gameObject.GetComponent<Collider2D>());
            _attackStay = 0;
        }
    }
    void EndBattle(Collision2D collision)
    {
        _isAttacking = false;
        DeleteBattleSign(collision);
    }
    void DeleteBattleSign(Collision2D collision)
    {
        int cnt = this.transform.Find("State").childCount;
        for (int i = 0; i < cnt; i++)
        {
            if (!transform.Find("State").GetChild(i).name.Contains("BattleSign")) continue;
            BattleUnit _b = Variables.Object(transform.Find("State").GetChild(i)).Get<BattleUnit>("enemy");
            if (_b.name == collision.collider.name)
            {
                Destroy(transform.Find("State").GetChild(i).gameObject);
            }
        }
    }
}