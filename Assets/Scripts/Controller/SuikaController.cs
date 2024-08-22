using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaController : MonoBehaviour
{
    public SuikaManager manager; //nextDongle에서 게임매니저를 넘겨받음
    public bool isMerge;
    public int level;
    public bool isReleased;
    bool _isDrag;
    Rigidbody2D _rb;
    Animator _anim;
    CircleCollider2D _circle;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _circle = GetComponent<CircleCollider2D>();
    }

    void OnEnable()
    {
        
    }
    void OnDisable()
    {
        //동글 속성 초기화
        level = 0;
        _isDrag = false;
        //_isMerge = false;
        //_isAttach = false;

        //동글 트랜스폼 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        //동글 물리 초기화
        _rb.simulated = false;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
        _circle.enabled = true;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SuikaDongle")
        {
            //충돌한 동글을 가져온다.
            SuikaController other = collision.gameObject.GetComponent<SuikaController>();

            //1. 상대와 나의 레벨이 같을 때
            //2. 내가 합치는 중이 아닐때
            //3. 상대가 합치는 중이 아닐때
            //4. level이 7보다 낮을때
            if(level == other.level && !isMerge && !other.isMerge && level < 7)
            {
                //동글 합치기 로직

                //나와 상대편 위치 가져오기
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                //1. 내가 아래에 있을때
                //2. 동일한 높이에 있을때, 내가 오른쪽에 있을때
                if(meY < otherY || (meY == otherY && meX > otherX))
                {
                    //상대방은 숨기기
                    other.Hide(transform.position); //상대방은 나를 향해 움직이면서 숨긴다.
                    //나는 레벨업
                    LevelUp();
                }
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SuikaBorderline")
        {
            if (isReleased)
            {
                manager.GameOver();
            }
            else
            {
                isReleased = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SuikaBorderLine")
        {
            manager.GameOver();
        }
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
            float leftBorder = -3f + transform.localScale.x / 2f; //��1�� x��ǥ�� -5�̰� �β��� 0.5�̹Ƿ� ���� ������ ���� �˳��ϰ� -4.6���� ����, ������ �������� + ���ش�.
            float rightBorder = 3f - transform.localScale.x / 2f; //��2�� x��ǥ�� 5�̰� �β��� 0.5�̹Ƿ� ���� ���� ���� �˳��ϰ� 4.6���� ����, ������ �������� - ���ش�.

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
        manager.SumScore(level);
    }
    public void SetLevel(int l)
    {
        level = l;
        _anim.SetInteger("Level", level);
    }
    void LevelUp()
    {
        isMerge = true; //잠금장치 걸어두기

        _rb.velocity = Vector2.zero; // 움직이지 않도록 속도 0으로 설정
        _rb.angularVelocity = 0; // 회전 속도도 0으로 설정

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        _anim.SetInteger("Level", level + 1); //레벨 올려서 애니메이션 실행

        yield return new WaitForSeconds(0.3f); //애니메이션 실행 시간 대기

        manager.SumScore(level);

        level++; //실제 레벨도 증가

        isMerge = false; //잠금장치 해제
    }
    public void Hide(Vector3 targetPos)
    {
        isMerge = true; //잠금장치 걸어두기

        //흡수 이동을 위해 물리효과 모두 비활성화

        _rb.simulated = false; //리지드바디 2D 물리효과 중지
        _circle.enabled = false;  //서클 콜라이더 2D 비활성화

        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int frameCount = 0;

        while(frameCount < 20)
        {
            frameCount++;//20프레임 실행되도록 
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            yield return null; //프레임 단위로 대기
        }

        isMerge = false; //합치기 종료
        gameObject.SetActive(false); //숨김이 완료됐으므로 비활성화
    }
}
