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
        handleStartPos = imgJoystickHandle.transform.position;
        HandJoystickHandlePos(eventData.position);
        HandJoystickDragDir(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        HandJoystickHandlePos(eventData.position);
        HandJoystickDragDir(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetJoystickHandlePos();
    }

    private void HandJoystickHandlePos(Vector2 touchPos)
    {
        if((touchPos-handleStartPos).sqrMagnitude> backgroundRadiusSqr)
        {
            Vector2 dir = (touchPos - handleStartPos).normalized;
            imgJoystickHandle.transform.position = handleStartPos + dir * backgroundRadius;
        }
        else
        {
            imgJoystickHandle.transform.position = touchPos;
        }
    }

    private void HandJoystickDragDir(Vector2 touchPos)
    {
        joystickDragDir = (touchPos - handleStartPos)/ backgroundRadius;
        GameManager.Instance.InputManager.UIJoystickInput(joystickDragDir);
    }

    private void ResetJoystickHandlePos()
    {
        imgJoystickHandle.transform.position = handleStartPos;
    }

    private void ResethandleStartPos()
    {

    }
}
