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
        public bool isMoveing;
    }

    PlayerInputData _playerInputData;
    public PlayerInputData CurrentInput => _playerInputData;


    public bool isReadIngPlayerInput = true;

    //接收ui按钮输入atk
    public bool uibtnAttackPressed = false;

    public void OnAwake()
    {
        _playerInputData = new PlayerInputData();
    }

    public void OnStart()
    {
        Cursor.lockState = CursorLockMode.Locked;//CursorLockMode影响鼠标运动逻辑，Locked大概率会隐藏但在某些环境不行
        Cursor.visible = false;//确保隐藏
    }

    public void OnUpdate()
    {
        CheckAndSetCursorEnable();

        if (isReadIngPlayerInput)
        {
            UpdateMovementInput();

            CheckIsAttack();
        }

    }

    /// <summary>
    /// 接受UI摇杆输入
    /// </summary>
    /// <param name="joystickDragDir"></param>
    public void UIJoystickInput(Vector2 joystickDragDir)
    {
        _playerInputData.MoveVector = joystickDragDir;
    }

    /// <summary>
    /// 检查并设置鼠标
    /// </summary>
    private void CheckAndSetCursorEnable()
    {
        //激活鼠标
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.Instance.cameraManager.camFreeLook.m_XAxis.m_MaxSpeed = 0f;
            GameManager.Instance.cameraManager.camFreeLook.m_YAxis.m_MaxSpeed = 0f;
            isReadIngPlayerInput = false;
            ResetInputData();
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            GameManager.Instance.cameraManager.camFreeLook.m_XAxis.m_MaxSpeed = 400f;
            GameManager.Instance.cameraManager.camFreeLook.m_YAxis.m_MaxSpeed = 3f;
            Cursor.lockState = CursorLockMode.Locked;//CursorLockMode影响鼠标运动逻辑，Locked大概率会隐藏但在某些环境不行
            Cursor.visible = false;//确保隐藏
            isReadIngPlayerInput = true;
        }
    }

    /// <summary>
    /// 更新移动输入
    /// </summary>
    private void UpdateMovementInput()
    {
        _playerInputData.MoveVector.x = Input.GetAxis("Horizontal");
        _playerInputData.MoveVector.y = Input.GetAxis("Vertical");
        _playerInputData.isMoveing = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        if (_playerInputData.MoveVector.sqrMagnitude > 1f)
        {
            _playerInputData.MoveVector.Normalize();
        }
    }

    /// <summary>
    /// 检查是否攻击
    /// </summary>
    private void CheckIsAttack()
    {
#if UNITY_ANDROID
        _playerInputData.IsAttack = uibtnAttackPressed;
        uibtnAttackPressed = false;
#else
        _playerInputData.IsAttack = Input.GetMouseButtonDown(0);
#endif
    }

    /// <summary>
    /// 重置输入数据
    /// </summary>
    private void ResetInputData()
    {
        _playerInputData.MoveVector = Vector2.zero;
        _playerInputData.IsAttack = false;
    }
}
