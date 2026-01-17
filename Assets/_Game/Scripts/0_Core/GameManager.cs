using Cinemachine;
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
    
    public InputManager InputManager { get; private set; } = new InputManager(); 
    public CameraManager cameraManager { get; private set; } = new CameraManager();
    public TerrainManager terrainManager { get; private set; } = new TerrainManager();
    public PlayerController playerController { get; private set; }

    [SerializeField]
    private CinemachineCollider camCollider;
    [SerializeField]
    private CinemachineFreeLook camFreeLook;

    [SerializeField]
    private MeshRenderer[] meshRenderers;

    private void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 60;
        cameraManager.camCollider = camCollider;
        cameraManager.camFreeLook = camFreeLook;
        terrainManager.meshRenderers = meshRenderers;
        terrainManager?.onAwake();
        InputManager?.OnAwake();
    }

    private void Start()
    {
        InputManager?.OnStart();
        cameraManager?.OnStart();
    }

    void Update()
    {
        InputManager?.OnUpdate();
        cameraManager?.OnUpdate();
    }

    public void InitPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }
}
