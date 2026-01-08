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
        public bool IsRunning;
    }

    PlayerInputData playerInputData;
    public PlayerInputData CurrentInput => playerInputData;

    private void Update()
    {
        playerInputData = new PlayerInputData();
        playerInputData.MoveVector.x = Input.GetAxis("Horizontal");
        playerInputData.MoveVector.y = Input.GetAxis("Vertical");
        if(playerInputData.MoveVector.magnitude > 1f)
        {
            playerInputData.MoveVector.Normalize();
        }

        //Debug.Log("MoveVector: " + playerInputData.MoveVector);
    }
}
