using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private Vector2 verticalLookRange;
    [SerializeField]
    float lookSpeed = 10;
    private Camera playerCamera;

    private void Awake() {
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Vector2 centerPoint = new(Screen.width/2f,Screen.height/2f);
        Mouse.current.WarpCursorPosition(centerPoint);
    }

    private void Update(){
        Look();
    }

    private void Look(){
        Vector2 mouseMovement = new(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
