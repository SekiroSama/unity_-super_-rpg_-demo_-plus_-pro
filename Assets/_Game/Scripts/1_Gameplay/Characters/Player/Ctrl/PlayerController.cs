using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    float moveSpeed = 5f;
    float rotatSpeed = 10f;

    Transform _camTransform;
    private void Start()
    {
        cc = this.GetComponent<CharacterController>();
        _camTransform = Camera.main.transform;
    }

    private void Update()
    {
        Move(GameInputManager.Instance.CurrentInput.MoveVector);
    }

    void Move(Vector2 input)
    {
        Debug.Log(input);
        if (input.sqrMagnitude <= 0.01) return;

        Vector3 camForward = _camTransform.forward;
        Vector3 camRight = _camTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 moveDir = camRight * input.x + camForward * input.y;

        cc.Move(moveDir * moveSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDir), rotatSpeed * Time.deltaTime);
    }
}
