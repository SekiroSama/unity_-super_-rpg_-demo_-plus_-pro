using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController CC;
    Animator Animator;
    int MoveSpeedHash = Animator.StringToHash("MoveSpeed");

    float moveSpeed = 5f;
    float rotatSpeed = 10f;

    Transform _camTransform;
    private void Start()
    {
        CC = this.GetComponent<CharacterController>();
        Animator = this.GetComponent<Animator>();
        _camTransform = Camera.main.transform;
    }

    private void Update()
    {
        Move(GameInputManager.Instance.CurrentInput.MoveVector);
    }

    void Move(Vector2 input)
    {
        //Debug.Log(input);
        if (input.sqrMagnitude <= 0.01) return;

        Vector3 camForward = _camTransform.forward;
        Vector3 camRight = _camTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 moveDir = camRight * input.x + camForward * input.y;

        CC.Move(moveDir * moveSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDir), rotatSpeed * Time.deltaTime);
    }
}

//1.变量声明
//在类中声明一个 Animator 类型的私有变量（建议加 [SerializeField] 方便在 Inspector 赋值）。
//声明一个 int 类型的私有变量，用于存储动画参数的 Hash ID（比如叫 _animIDMoveSpeed）。
//2. 初始化 (Awake 或 Start)
//调用 Animator.StringToHash() 方法。
//传入的字符串必须是你刚才在 Animator 面板里设置的参数名："MoveSpeed"。
//将返回的整数结果保存到刚才声明的 int 变量中。
//3. 对外接口 (UpdateAnimation)
//创建一个 public void 方法，参数接收一个 float 类型（比如叫 targetSpeed）。
//核心逻辑：在方法内部调用 animator.SetFloat()。
//关键指引：请务必使用 4个参数 的重载版本，这是实现平滑的关键：
//ID：传入你缓存的那个 int 变量。
//Value：传入方法的参数 targetSpeed。
//DampTime：传入 0.1f。
//逻辑含义：告诉 Unity “我希望在 0.1 秒内平滑过渡到目标值”。
//DeltaTime：传入 Time.deltaTime。
//逻辑含义：让平滑计算与帧率解耦。