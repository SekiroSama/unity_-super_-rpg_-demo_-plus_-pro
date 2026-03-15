using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager
{
    public struct PlayerInputData
    {
        public Vector2 MoveVector;
        public bool isJump;
        public bool IsRun;
        public bool IsAttack;
        public bool isMoveing;
        public bool isDodge;
        public bool isDefense;
    }

    PlayerInputData _playerInputData;
    public PlayerInputData CurrentInput => _playerInputData;


    public bool isReadIngPlayerInput = true;

    //接收ui按钮输入atk
    public bool uibtnAttackPressed = false;

    private float sensitivity = 1f;

    public void OnAwake()
    {
        _playerInputData = new PlayerInputData();
        sensitivity = 1080f / Screen.width;
    }

    public void OnStart()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

#else
        //Cursor.lockState = CursorLockMode.Locked;//CursorLockMode影响鼠标运动逻辑，Locked大概率会隐藏但在某些环境不行
        //Cursor.visible = false;//确保隐藏
#endif
    }

    public void OnUpdate()
    {
        //CheckAndSetCursorEnable();

        if (isReadIngPlayerInput)
        {
            UpdateMovementInput();

            CheckIsAccelerate();

            CheckIsAttack();

            CheckIsJump();

            CheckIsDefense();

            UpdateFreelookInput();

            CheckIsDodge();
        }
    }

    /// <summary>
    /// Android更新相机输入
    /// </summary>
    private void UpdateFreelookInput()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Touch[] touches = Input.touches;
        for (int i = 0; i < touches.Length; i++)
        {
            if (touches[i].position.x > Screen.width / 2)
            {
                GameManager.Instance.cameraManager.camFreeLook.m_XAxis.Value += touches[i].deltaPosition.x * sensitivity;
                GameManager.Instance.cameraManager.camFreeLook.m_YAxis.Value -= touches[i].deltaPosition.y * sensitivity * 0.01f;
                break;
            }
        }
#endif
        //Touch[] touches = Input.touches;
        //for (int i = 0; i < touches.Length; i++)
        //{
        //    if (touches[i].position.x > Screen.width / 2)
        //    {
        //        GameManager.Instance.cameraManager.camFreeLook.m_XAxis.Value += touches[i].deltaPosition.x * sensitivity;
        //        GameManager.Instance.cameraManager.camFreeLook.m_YAxis.Value -= touches[i].deltaPosition.y * sensitivity * 0.01f;
        //        break;
        //    }
        //}
    }

    /// <summary>
    /// 接受UI摇杆输入
    /// </summary>
    /// <param name="joystickDragDir"></param>
    public void UIJoystickInput(Vector2 joystickDragDir)
    {
        _playerInputData.MoveVector = joystickDragDir;
        _playerInputData.isMoveing = _playerInputData.MoveVector.sqrMagnitude > 0.01;
    }

    /// <summary>
    /// 检查并设置鼠标
    /// </summary>
    private void CheckAndSetCursorEnable()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //移动平台不处理鼠标
#else
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
#endif
    }
    /// <summary>
    /// 更新移动输入
    /// </summary>
    private void UpdateMovementInput()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //android在ui层处理
#else
        _playerInputData.MoveVector.x = Input.GetAxis("Horizontal");
        _playerInputData.MoveVector.y = Input.GetAxis("Vertical");
        _playerInputData.isMoveing = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        if (_playerInputData.MoveVector.sqrMagnitude > 1f)
        {
            _playerInputData.MoveVector.Normalize();
        }
#endif
    }
    /// <summary>
    /// 检查是否加速跑
    /// </summary>
    private void CheckIsAccelerate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //待实现
#else
        _playerInputData.IsRun = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _playerInputData.IsRun = false;
            return;
        }
#endif
    }
    /// <summary>
    /// 是否躲避
    /// </summary>
    private void CheckIsDodge()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
#else
        _playerInputData.isDodge = Input.GetKeyDown(KeyCode.LeftControl);
#endif
    }
    /// <summary>
    /// 检查是否攻击
    /// </summary>
    private void CheckIsAttack()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _playerInputData.IsAttack = uibtnAttackPressed;
        Debug.Log("uibtnAttackPressed");
        uibtnAttackPressed = false;
#else
        _playerInputData.IsAttack = Input.GetMouseButtonDown(0);
#endif
    }
    /// <summary>
    /// 检查是否跳跃
    /// </summary>
    private void CheckIsJump()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //_playerInputData.IsJump= uibtnJumpPressed;
#else
        _playerInputData.isJump = Input.GetKey(KeyCode.Space);
#endif
    }
    /// <summary>
    /// 检查是否在防御
    /// </summary>
    private void CheckIsDefense()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //_playerInputData.IsDefense = uibtnDefensePressed;
#else
        _playerInputData.isDefense = Input.GetMouseButton(1);
        if (Input.GetMouseButtonUp(1))
        {
            _playerInputData.isDefense = false;
            return;
        }
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
