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

            //x�� ��輳��
            float leftBorder = -2.3f + transform.localScale.x / 2f; //��1�� x��ǥ�� -5�̰� �β��� 0.5�̹Ƿ� ���� ������ ���� �˳��ϰ� -4.6���� ����, ������ �������� + ���ش�.
            float rightBorder = 2.3f - transform.localScale.x / 2f; //��2�� x��ǥ�� 5�̰� �β��� 0.5�̹Ƿ� ���� ���� ���� �˳��ϰ� 4.6���� ����, ������ �������� - ���ش�.

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
