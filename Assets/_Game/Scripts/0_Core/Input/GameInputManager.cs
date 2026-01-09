using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    private static GameInputManager _instance;
    public static GameInputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public struct PlayerInputData
    {
        public Vector2 MoveVector;
        public bool IsAttack;
    }

    PlayerInputData _playerInputData;
    public PlayerInputData CurrentInput => _playerInputData;

    //接收ui按钮输入
    private bool _virtualAttackPressed = false;
    private void Update()
    {
        _playerInputData = new PlayerInputData();
        _playerInputData.MoveVector.x = Input.GetAxis("Horizontal");
        _playerInputData.MoveVector.y = Input.GetAxis("Vertical");
        if(_playerInputData.MoveVector.magnitude > 1f)
        {
            _playerInputData.MoveVector.Normalize();
        }

        _playerInputData.IsAttack = Input.GetMouseButtonDown(0) || _virtualAttackPressed;
        _virtualAttackPressed = false;
        //Debug.Log("MoveVector: " + playerInputData.MoveVector);
    }
}
