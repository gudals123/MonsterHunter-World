using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float sensitivity = 300f; // ���콺 ����

    float xRotation = 0.0f;

    void Start()
    {
        // Ŀ�� ���� �� �����
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ���콺 �Է� �ޱ�
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        /*        mouseY = transform.rotation.y + mouseY;
                xRotation = Mathf.Clamp(mouseY, -90f, 90f);*/ 


        // ���� ȸ��(���� �̵�)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // ī�޶� ȸ�� ����
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Debug.Log(xRotation);
    }
}

