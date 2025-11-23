using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSimulator : MonoBehaviour
{
    public Button targetButton;
    public RectTransform virtualCursor;

    private PointerEventData GetPointerEventData()
    {
        return new PointerEventData(EventSystem.current)
        {
            position = virtualCursor.position,
            pointerEnter = targetButton.gameObject,
            pointerPress = targetButton.gameObject,
            clickCount = 1
        };
    }

    public void SimulateHover()
    {
        if (targetButton == null) return;

        var pointerData = GetPointerEventData();
        ExecuteEvents.Execute(targetButton.gameObject, pointerData, ExecuteEvents.pointerEnterHandler);
    }

    public void SimulateNormal()
    {
        if (targetButton == null) return;

        var pointerData = GetPointerEventData();
        ExecuteEvents.Execute(targetButton.gameObject, pointerData, ExecuteEvents.pointerExitHandler);
    }

    public void SimulateClickDown()
    {
        if (targetButton == null) return;

        var pointerData = GetPointerEventData();
        ExecuteEvents.Execute(targetButton.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
    }

    public void SimulateClickUp()
    {
        if (targetButton == null) return;

        var pointerData = GetPointerEventData();

        ExecuteEvents.Execute(targetButton.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(targetButton.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
    }

    public void SimulateFullClick()
    {
        SimulateClickDown();
        SimulateClickUp();
    }
}
