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

    public float runTimer = 0;
    public InputManager inputManager { get; private set; } = new InputManager();
    public CameraManager cameraManager { get; private set; } = new CameraManager();
    public TerrainManager terrainManager { get; private set; } = new TerrainManager();
    public PlayerController playerController { get; private set; }
    public EnemyController enemyController { get; private set; }

    [SerializeField]
    private CinemachineCollider camCollider;
    [SerializeField]
    private CinemachineFreeLook camFreeLook;

    [SerializeField]
    private MeshRenderer[] meshRenderers;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform CharactersTransform;


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
        InitEnemyController();
    }

    private void Start()
    {
        inputManager?.OnStart();
        cameraManager?.OnStart();
    }

    void Update()
    {

        runTimer += Time.deltaTime;
        inputManager?.OnUpdate();
        cameraManager?.OnUpdate();
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    public void InitPlayerController()
    {
        this.playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        cameraManager.camFreeLook.Follow = this.playerController.transform;
        cameraManager.camFreeLook.LookAt = this.playerController.LookPos;
        this.playerController.gameObject.transform.SetParent(CharactersTransform);
    }

    /// <summary>
    /// 创建敌人
    /// </summary>
    public void InitEnemyController()
    {
        this.enemyController = Instantiate(enemyPrefab).GetComponent<EnemyController>();
        this.enemyController.gameObject.transform.SetParent(CharactersTransform);
    }
}
