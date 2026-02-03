using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController CC;
    private Animator animator;

    public StateMachine stateMachine;


    //[HideInInspector]
    public float moveSpeed = 5f;
    [Header("主角移动")]
    public float rotatSpeed = 10f;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    [Tooltip("跑步过渡时间")]
    public float fadeTime = 10f;
    Transform _camTransform;
    [Header("物理检测")]
    public Transform grdCheckPos;//地面检测点
    public LayerMask whatIsGround;//地面层
    public float checkRadius;//检测半径
    public bool isGrounded;
    [HideInInspector]
    public bool UseRootMotion = false;
    [Header("武器脚本")]
    public WeaponController weaponController;
    public Transform LookPos;//用于环境遮挡裁剪


    private void Start()
    {
        CC = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
        _camTransform = Camera.main.transform;

        stateMachine = new StateMachine(this);
        stateMachine.Initialize<PlayerIdleState>();
    }

    private void Update()
    {
        //状态帧更新
        stateMachine.OnUpdate();
        //角色混合树动画参数更新

        //UpdateLocomotion(GameManager.Instance.InputManager.CurrentInput.MoveVector.magnitude);
        //处理角色重力
        HandGravity();
        CheckIsGrounded();
    }
    #region 角色运动状态
    
    public void AddForce(Vector2 force)
    {
        
    }


    /// <summary>
    /// 处理角色重力
    /// </summary>
    private void HandGravity()
    {
        CC.Move(Physics.gravity * Time.deltaTime);
    }

    /// <summary>
    /// 让角色旋转移动
    /// </summary>
    /// <param name="input">输入方向</param>
    public void Move(Vector2 input)
    {
        Vector3 moveDir = GetCameraRelativeDir(input);

        FaceDirection(moveDir);
        if (GameManager.Instance.inputManager.CurrentInput.isMoveing)
        {
            CC.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 根据输入方向让角色面向该方向
    /// </summary>
    /// <param name="input">输入方向</param>
    public void FaceInput(Vector2 input)
    {
        Vector3 moveDir = GetCameraRelativeDir(input);
        FaceDirection(moveDir);
    }

    /// <summary>
    /// 让角色面向moveDir
    /// </summary>
    /// <param name="moveDir"></param>
    private void FaceDirection(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude <= 0.01f) return;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDir), rotatSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 计算相机相对方向的移动向量
    /// </summary>
    /// <param name="input">输入方向</param>
    /// <returns></returns>
    private Vector3 GetCameraRelativeDir(Vector2 input)
    {
        if (input.sqrMagnitude <= 0.01) return Vector3.zero;

        Vector3 camForward = _camTransform.forward;
        Vector3 camRight = _camTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 moveDir = camRight * input.x + camForward * input.y;
        return moveDir.normalized;
    }

    /// <summary>
    /// 处理角色根运动
    /// </summary>
    private void OnAnimatorMove()
    {
        if (!UseRootMotion) return;
        CC.Move(animator.deltaPosition);
        this.transform.rotation *= animator.deltaRotation;
    }

    /// <summary>
    /// 让角色混合树动画参数更新
    /// </summary>
    /// <param name="speed"></param>
    public void UpdateLocomotion(float speed)
    {
        animator.SetFloat(AnimationConfig_UnityChan.Parameters.XSpeed, speed);
    }
    #endregion
    #region 角色动画状态

    /// <summary>
    /// 让角色更新动画
    /// </summary>
    /// <param name="animHash"></param>
    /// <param name="fadeTime"></param>
    public void PlayAnimation(int animHash, float fadeTime = 0.1f)
    {
        animator.CrossFadeInFixedTime(animHash, fadeTime);// 参数2：过渡时间，0.1秒通常是 ARPG 的黄金标准
    }

    /// <summary>
    /// 角色攻击动画是否播放完毕
    /// </summary>
    /// <returns></returns>
    public bool IsAnimationFinished(int animHash, float exitTime = 0.95f)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash == animHash && stateInfo.normalizedTime >= exitTime)
        {
            return true;
        }
        return false;
    }
    #endregion
    #region 动画事件

    /// <summary>
    /// 动画事件：开始攻击
    /// </summary>
    public void AE_ATKStart()
    {
        weaponController.WeaponRedissolveValStart();//开始反溶解
        
    }

    /// <summary>
    /// 动画事件：结束攻击
    /// </summary>
    public void AE_ATKEnd()
    {
        weaponController.WeaponDissolveValStart();//开始溶解
        
    }

    /// <summary>
    /// 动画事件：武器碰撞开启
    /// </summary>
    public void AE_WeaponColliderOn()
    {
        weaponController.WeaponColliderOn();
        weaponController.WeaponTrailOn();  
    }

    /// <summary>
    /// 动画事件：武器碰撞关闭
    /// </summary>
    public void AE_WeaponColliderOff()
    {
        weaponController.WeaponColliderOff();
        weaponController.WeaponTrailOff();
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (grdCheckPos != null)
        {
            // 如果检测到地面，球体变绿；否则变红
            bool isGrounded = Physics.CheckSphere(grdCheckPos.position, checkRadius, whatIsGround);
            Gizmos.color = isGrounded ? Color.green : Color.red;

            // 画出实心球或线框球
            Gizmos.DrawWireSphere(grdCheckPos.position, checkRadius);
        }
    }
    public void CheckIsGrounded()
    {
        isGrounded = Physics.CheckSphere(grdCheckPos.position, checkRadius, whatIsGround);
    }
}
