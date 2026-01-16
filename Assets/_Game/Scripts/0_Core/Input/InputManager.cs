using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public struct PlayerInputData
    {
        public Vector2 MoveVector;
        public bool IsAttack;
    }

    PlayerInputData _playerInputData;
    public PlayerInputData CurrentInput => _playerInputData;

    //接收ui按钮输入
    private bool _virtualAttackPressed = false;

    public void OnStart()
    {
        //Cursor.lockState = CursorLockMode.Locked;//CursorLockMode影响鼠标运动逻辑，Locked大概率会隐藏但在某些环境不行
        //Cursor.visible = false;//确保隐藏
    }

    public void OnUpdate()
    {
        //激活鼠标
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.Instance.cameraManager.camFreeLook.enabled = false;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;//CursorLockMode影响鼠标运动逻辑，Locked大概率会隐藏但在某些环境不行
            Cursor.visible = false;//确保隐藏
            GameManager.Instance.cameraManager.camFreeLook.enabled = true;
        }

        _playerInputData = new PlayerInputData();
        _playerInputData.MoveVector.x = Input.GetAxis("Horizontal");
        _playerInputData.MoveVector.y = Input.GetAxis("Vertical");
        if(_playerInputData.MoveVector.magnitude > 1f)
        {
            _playerInputData.MoveVector.Normalize();
        }

        _playerInputData.IsAttack = Input.GetMouseButtonDown(0) || _virtualAttackPressed;
        _virtualAttackPressed = false;
    }
}
