using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform followingTarget;
    public float moveSpeed;
    public Vector2 canMoveAreaCenter;
    public Vector2 canMoveAreaSize;

    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(canMoveAreaCenter, canMoveAreaSize);
    }
    private void Move()
    {
        Vector2 h = new Vector2(Input.GetAxis("Horizontal"), 0);
        Vector2 v = new Vector2(0, Input.GetAxis("Vertical"));
        Vector2 _velocity = (h + v).normalized * moveSpeed;
        // transform.Translate(h + v);
        transform.Translate(_velocity);
    }
    void Awake()
    {
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = Screen.width * cameraHalfHeight / Screen.height;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void LateUpdate()                   /* 카메라 이동은 LateUpdate()에서 처리 */
    {
        FollowTarget();
    }
    
    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(followingTarget.position.x,
                                             this.transform.position.y,
                                             this.transform.position.z);

        this.transform.position = Vector3.Lerp(this.transform.position,
                                               targetPosition,
                                               moveSpeed * Time.deltaTime);

        float restrictionAreaX = canMoveAreaSize.x * 0.5f - cameraHalfWidth;
        float clampX = Mathf.Clamp(this.transform.position.x,
                                   -restrictionAreaX + canMoveAreaCenter.x,
                                   restrictionAreaX + canMoveAreaCenter.x);

        float restrictionAreaY = canMoveAreaSize.y * 0.5f - cameraHalfHeight;
        float clampY = Mathf.Clamp(this.transform.position.y,
                                   -restrictionAreaY + canMoveAreaCenter.y,
                                   restrictionAreaY + canMoveAreaCenter.y);


        this.transform.position = new Vector3(clampX, clampY, this.transform.position.z);
    }
}
