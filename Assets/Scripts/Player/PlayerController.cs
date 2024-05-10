using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float cameraSpeed;


    [SerializeField]
    private Camera theCamera;  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
    }
    
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");  
        float _moveDirZ = Input.GetAxisRaw("Vertical");  

        Vector2 _moveHorizontal = transform.right * _moveDirX; 
        Vector2 _moveVertical = transform.forward * _moveDirZ; 

        Vector2 _velocity = (_moveHorizontal + _moveVertical).normalized * cameraSpeed;  

        //theCamera.transform.localPosition = new Vector2(_moveHorizontal, _moveVertical);
    }
}
