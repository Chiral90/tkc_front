using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] Transform tf_cursor;   // 커서의 현재 위치
    [SerializeField] float dragSpeed = 10.0f;   // 화면 움직임 속도
    private float firstClickPointX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CursorMoving();
        ViewMoving();
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
    void ViewMoving()
    {
        // 마우스 최초 클릭 시의 위치 기억
        if (Input.GetMouseButtonDown(0))
        {
            firstClickPointX = tf_cursor.localPosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            // (현재 마우스 위치 - 최초 위치)의 음의 방향으로 카메라 이동
            Vector2 position = Camera.main.ScreenToViewportPoint(- new Vector3(tf_cursor.localPosition.x - firstClickPointX, 0, 0));
            Vector2 move = position * (Time.deltaTime * dragSpeed);

            Camera.main.transform.Translate(move);
        }
    }
}
