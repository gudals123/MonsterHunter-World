using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float sensitivity = 300f; // 마우스 감도

    float xRotation = 0.0f;

    void Start()
    {
        // 커서 고정 및 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 마우스 입력 받기
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        /*        mouseY = transform.rotation.y + mouseY;
                xRotation = Mathf.Clamp(mouseY, -90f, 90f);*/ 


        // 수직 회전(상하 이동)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 카메라 회전 적용
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Debug.Log(xRotation);
    }
}

