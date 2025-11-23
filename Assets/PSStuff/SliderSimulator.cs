using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderSimulator : MonoBehaviour
{
    public Slider targetSlider;
    public RectTransform virtualCursor;

    private PointerEventData GetPointerEventData()
    {
        return new PointerEventData(EventSystem.current)
        {
            position = virtualCursor.position,
            pointerEnter = targetSlider.gameObject,
            pointerPress = targetSlider.gameObject,
            clickCount = 1
        };
    }

    public void SimulateHover()
    {
        if (targetSlider == null) return;

        targetSlider.interactable = true;
        var pointerData = GetPointerEventData();
        ExecuteEvents.Execute(targetSlider.gameObject, pointerData, ExecuteEvents.pointerEnterHandler);
    }

    public void SimulateNormal()
    {
        if (targetSlider == null) return;

        targetSlider.interactable = false;
        var pointerData = GetPointerEventData();
        ExecuteEvents.Execute(targetSlider.gameObject, pointerData, ExecuteEvents.pointerExitHandler);
    }

    public void SimulateClickDown()
    {
        if (targetSlider == null) return;

        targetSlider.interactable = true;
        var pointerData = GetPointerEventData();
        ExecuteEvents.Execute(targetSlider.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
    }

    public void SimulateClickUp()
    {
        if (targetSlider == null) return;

        var pointerData = GetPointerEventData();

        targetSlider.interactable = false;
        ExecuteEvents.Execute(targetSlider.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(targetSlider.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
    }

    public void SimulateFullClick()
    {
        SimulateClickDown();
        SimulateClickUp();
    }
}
