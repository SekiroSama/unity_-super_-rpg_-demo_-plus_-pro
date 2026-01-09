using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    CharacterController CC;
    private Animator animator;

    private StateMachine stateMachine;

    float moveSpeed = 5f;
    float rotatSpeed = 10f;

    Transform _camTransform;
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
        stateMachine.OnUpdate();
    }

    /// <summary>
    /// 让角色位移旋转
    /// </summary>
    /// <param name="input"></param>
    public void Move(Vector2 input)
    {
        if (input.sqrMagnitude <= 0.01) return;

        Vector3 camForward = _camTransform.forward;
        Vector3 camRight = _camTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 moveDir = camRight * input.x + camForward * input.y;

        CC.Move(moveDir * moveSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDir), rotatSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 让角色混合树动画更新
    /// </summary>
    /// <param name="speed"></param>
    public void UpdateAnimation(float speed)
    {
        animator.SetFloat(id: AnimHash.MoveSpeed, value: speed, dampTime: 0.1f, deltaTime: Time.deltaTime);
    }

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
    public bool IsAttckFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Attack01") && stateInfo.normalizedTime >= 1f)
        {
            return true;
        }
        return false;
    }

}
