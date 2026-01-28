using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float Timer = 0;
    public InputManager inputManager { get; private set; } = new InputManager();
    public CameraManager cameraManager { get; private set; } = new CameraManager();
    public TerrainManager terrainManager { get; private set; } = new TerrainManager();
    public PlayerController playerController { get; private set; }

    [SerializeField]
    private CinemachineCollider camCollider;
    [SerializeField]
    private CinemachineFreeLook camFreeLook;

    [SerializeField]
    private MeshRenderer[] meshRenderers;

    [SerializeField]
    private GameObject playerPrefab;


    private void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 60;
        cameraManager.camCollider = camCollider;
        cameraManager.camFreeLook = camFreeLook;
        terrainManager.meshRenderers = meshRenderers;
        terrainManager?.onAwake();
        inputManager?.OnAwake();

        InitPlayerController();
    }

    private void Start()
    {
        inputManager?.OnStart();
        cameraManager?.OnStart();
    }

    void Update()
    {
        Timer += Time.deltaTime;
        inputManager?.OnUpdate();
        cameraManager?.OnUpdate();
    }

    public void InitPlayerController()
    {
        this.playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        cameraManager.camFreeLook.Follow = this.playerController.transform;
        cameraManager.camFreeLook.LookAt = this.playerController.LookPos;
    }
}
