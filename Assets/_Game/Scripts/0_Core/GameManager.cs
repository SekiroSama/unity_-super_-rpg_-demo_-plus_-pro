using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    
    public GameInputManager gameInputManager { get; private set; } = new GameInputManager();
    public CameraManager cameraManager { get; private set; } = new CameraManager();
    public PlayerController playerController { get; private set; }

    private void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 60;

    }

    private void Start()
    {
        gameInputManager?.OnStart();
    }

    void Update()
    {
        gameInputManager?.OnUpdate();
        cameraManager?.OnUpdate();
    }

    public void InitPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }
}
