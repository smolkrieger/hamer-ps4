using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SingleMenuButtons : MonoBehaviour
{
    public RectTransform cursor;

    //Button template
    public Image buttonTemplate;
    public ButtonSimulator buttonTemplateSim;
    private bool clickedButtonTemplate;
    private bool hoveredButtonTemplate;

    public Canvas menuCanvas;

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform1 = cursor;

        RectTransform rectTransform2 = buttonTemplate.rectTransform;

        if (RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && !hoveredButtonTemplate)
        {
            buttonTemplateSim.SimulateHover();
            hoveredButtonTemplate = true;
        }
        if (!RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && hoveredButtonTemplate)
        {
            buttonTemplateSim.SimulateNormal();
            hoveredButtonTemplate = false;
        }
        if (RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && Input.GetButtonDown("Cross"))
        {
            buttonTemplateSim.SimulateClickDown();
            clickedButtonTemplate = true;
        }
        if (!RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && !Input.GetButtonDown("Cross"))
        {
            buttonTemplateSim.SimulateNormal();
            clickedButtonTemplate = false;
        }
        if (Input.GetButtonUp("Cross") && clickedButtonTemplate)
        {
            buttonTemplateSim.SimulateClickUp();
            clickedButtonTemplate = false;
            hoveredButtonTemplate = false;
        }
    }

    public static bool RectOverlaps(RectTransform rect1, RectTransform rect2, Canvas canvas)
    {
        if (rect1 == null || rect2 == null || canvas == null)
            return false;

        Vector3[] corners1 = new Vector3[4];
        rect1.GetWorldCorners(corners1);

        Vector3[] corners2 = new Vector3[4];
        rect2.GetWorldCorners(corners2);

        for (int i = 0; i < 4; i++)
        {
            corners1[i] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners1[i]);
            corners2[i] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners2[i]);
        }

        Rect screenRect1 = GetScreenRect(corners1);
        Rect screenRect2 = GetScreenRect(corners2);

        return screenRect1.Overlaps(screenRect2);
    }

    private static Rect GetScreenRect(Vector3[] corners)
    {
        float xMin = corners[0].x;
        float xMax = corners[0].x;
        float yMin = corners[0].y;
        float yMax = corners[0].y;

        foreach (Vector3 corner in corners)
        {
            if (corner.x < xMin) xMin = corner.x;
            if (corner.x > xMax) xMax = corner.x;
            if (corner.y < yMin) yMin = corner.y;
            if (corner.y > yMax) yMax = corner.y;
        }

        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }
}
