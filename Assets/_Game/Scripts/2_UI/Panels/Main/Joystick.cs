using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image imgJoystickHandle;
    private Vector2 joystickDragDir;
    private Vector2 handleStartPos;//摇杆把手初始位置
    private float backgroundRadius = 100f;
    private float backgroundRadiusSqr = 100f * 100f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        handleStartPos = this.transform.position;
        HandJoystickHandlePos(eventData.position);
        HandJoystickDragDir(imgJoystickHandle.transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        HandJoystickHandlePos(eventData.position);
        HandJoystickDragDir(imgJoystickHandle.transform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetJoystickHandlePos();
    }

    /// <summary>
    /// 处理摇杆把手位置
    /// </summary>
    /// <param name="touchPos"></param>
    private void HandJoystickHandlePos(Vector2 touchPos)
    {
        if((touchPos - handleStartPos).sqrMagnitude> backgroundRadiusSqr)
        {
            Vector2 dir = (touchPos - handleStartPos).normalized;
            imgJoystickHandle.transform.position = handleStartPos + dir * backgroundRadius;
        }
        else
        {
            imgJoystickHandle.transform.position = touchPos;
        }
    }

    /// <summary>
    /// 计算摇杆拖拽方向并传递给输入管理器
    /// </summary>
    /// <param name="touchPos"></param>
    private void HandJoystickDragDir(Vector2 imgJoystickHandlePos)
    {
        joystickDragDir = (imgJoystickHandlePos - handleStartPos)/ backgroundRadius;
        GameManager.Instance.InputManager.UIJoystickInput(joystickDragDir);
        Debug.Log("joystickDragDir:"+ joystickDragDir);
    }

    private void ResetJoystickHandlePos()
    {
        imgJoystickHandle.transform.position = handleStartPos;
    }

    private void ResethandleStartPos()
    {

    }
}
