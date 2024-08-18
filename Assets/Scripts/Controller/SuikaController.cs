using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaController : MonoBehaviour
{
    public int level;
    bool _isDrag;
    Rigidbody2D _rb;
    Animator _anim;
    CircleCollider2D _circle;
    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _circle = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Debug.Log("On enable level " + level);
        _anim.SetInteger("Level", level);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDrag)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //x축 경계설정
            float leftBorder = -2.3f + transform.localScale.x / 2f; //벽1의 x좌표는 -5이고 두께가 0.5이므로 벽의 오른쪽 끝을 넉넉하게 -4.6으로 설정, 동글의 반지름도 + 해준다.
            float rightBorder = 2.3f - transform.localScale.x / 2f; //벽2의 x좌표는 5이고 두께가 0.5이므로 벽의 왼쪽 끝을 넉넉하게 4.6으로 설정, 동글의 반지름도 - 해준다.

            if (mousePos.x < leftBorder)
            {
                mousePos.x = leftBorder;
            }
            else if (mousePos.x > rightBorder)
            {
                mousePos.x = rightBorder;
            }
            mousePos.y = 4;
            mousePos.z = 0;
            transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f);
        }
    }
    public void Drag()
    {
        _isDrag = true;
    }
    public void Drop()
    {
        _isDrag = false;
        _rb.simulated = true;
    }
}
