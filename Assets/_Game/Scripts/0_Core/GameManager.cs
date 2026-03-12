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
    public InputManager inputManager { get; private set; } = new InputManager();
    public CameraManager cameraManager { get; private set; } = new CameraManager();
    public TerrainManager terrainManager { get; private set; } = new TerrainManager();
    public PlayerController playerController { get; private set; }
    public EnemyController enemyController { get; private set; }
    public UIManager uiManager { get; private set; } = new UIManager();

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
    [SerializeField]
    private Transform FatFatDragonHome;
    [SerializeField]
    private Transform[] FatFatDragon_Patrols;
    [SerializeField]
    private Transform Player_BornPos;
    [SerializeField]
    private Material FX_RadialBlur_FullScreen_Material;
    [SerializeField]
    private MainPanel MainPanel;

    private void Awake()
    {
        SetApplicationTargetFrameRate(60);

        this.Init();
        cameraManager?.Init(camCollider, camFreeLook, FX_RadialBlur_FullScreen_Material);
        terrainManager?.Init(meshRenderers);
        uiManager?.Init(MainPanel);

        terrainManager?.onAwake();
        inputManager?.OnAwake();

        InitPlayerController();
        InitEnemyController();
    }

    private void Init()
    {
        _instance = this;
    }

    private void Start()
    {
        inputManager?.OnStart();
        cameraManager?.OnStart();
    }

    void Update()
    {
        inputManager?.OnUpdate();
        cameraManager?.OnUpdate();
    }

    /// <summary>
    /// 设置应用目标帧率
    /// </summary>
    /// <param name="frameRate"></param>
    public void SetApplicationTargetFrameRate(int frameRate)
    {
        Application.targetFrameRate = frameRate;
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
        this.playerController.gameObject.transform.position = Player_BornPos.position;
    }

    /// <summary>
    /// 创建敌人
    /// </summary>
    public void InitEnemyController()
    {
        this.enemyController = Instantiate(enemyPrefab).GetComponent<EnemyController>();
        this.enemyController.transform.position = FatFatDragonHome.position;
        this.enemyController.gameObject.transform.SetParent(CharactersTransform);
        this.enemyController.Init(FatFatDragonHome, FatFatDragon_Patrols);
    }
}
