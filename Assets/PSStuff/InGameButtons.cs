using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameButtons : MonoBehaviour
{
    public RectTransform cursor;

    //Button template
    public Image optionsBack;
    public ButtonSimulator optionsBackSim;
    private bool clickedOptionsBack;
    private bool hoveredOptionsBack;

    public Image optionsApply;
    public ButtonSimulator optionsApplySim;
    private bool clickedOptionsApply;
    private bool hoveredOptionsApply;

    public Image optionsSensitivity;
    public SliderSimulator optionsSensitivitySim;
    private bool clickedOptionsSensitivity;
    private bool hoveredOptionsSensitivity;

    //Slider template
    public Image optionsVolume;
    public SliderSimulator optionsVolumeSim;
    private bool clickedOptionsVolume;
    private bool hoveredOptionsVolume;

    public Image menuContinue;
    public ButtonSimulator menuContinueSim;
    private bool clickedMenuContinue;
    private bool hoveredMenuContinue;
    
    public Image menuOptions;
    public ButtonSimulator menuOptionsSim;
    private bool clickedMenuOptions;
    private bool hoveredMenuOptions;

    public Image menuExit;
    public ButtonSimulator menuExitSim;
    private bool clickedMenuExit;
    private bool hoveredMenuExit;

    public Image exitYes;
    public ButtonSimulator exitYesSim;
    private bool clickedExitYes;
    private bool hoveredExitYes;

    public Image exitNo;
    public ButtonSimulator exitNoSim;
    private bool clickedExitNo;
    private bool hoveredExitNo;

    private string CurrentScreen;

    public Canvas menuCanvas;

    void Awake()
    {
        CurrentScreen = "PauseMenu";
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform1 = cursor;

        //Options recttransforms
        RectTransform rectTransform2 = optionsBack.rectTransform;
        RectTransform rectTransform3 = optionsApply.rectTransform;
        RectTransform rectTransform4 = optionsVolume.rectTransform;
        RectTransform rectTransform5 = optionsSensitivity.rectTransform;

        //Pause menu recttransforms
        RectTransform rectTransform6 = menuContinue.rectTransform;
        RectTransform rectTransform7 = menuOptions.rectTransform;
        RectTransform rectTransform8 = menuExit.rectTransform;

        RectTransform rectTransform9 = exitYes.rectTransform;
        RectTransform rectTransform10 = exitNo.rectTransform;

        if (CurrentScreen == "ExitMenu")
        {
            //exitYesSim
            if (RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && !hoveredExitYes)
            {
                exitYesSim.SimulateHover();
                hoveredExitYes = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && hoveredExitYes)
            {
                exitYesSim.SimulateNormal();
                hoveredExitYes = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                exitYesSim.SimulateClickDown();
                clickedExitYes = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                exitYesSim.SimulateNormal();
                clickedExitYes = false;
            }
            if (Input.GetButtonUp("Cross") && clickedExitYes)
            {
                exitYesSim.SimulateClickUp();
                clickedExitYes = false;
                hoveredExitYes = false;
            }

            //exitNoSim
            if (RectOverlaps(rectTransform1, rectTransform10, menuCanvas) && !hoveredExitNo)
            {
                exitNoSim.SimulateHover();
                hoveredExitNo = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform10, menuCanvas) && hoveredExitNo)
            {
                exitNoSim.SimulateNormal();
                hoveredExitNo = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform10, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                exitNoSim.SimulateClickDown();
                clickedExitNo = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform10, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                exitNoSim.SimulateNormal();
                clickedExitNo = false;
            }
            if (Input.GetButtonUp("Cross") && clickedExitNo)
            {
                exitNoSim.SimulateClickUp();
                clickedExitNo = false;
                hoveredExitNo = false;
            }
        }
        
        if (CurrentScreen == "PauseMenu")
        {
            //menuContinueSim
            if (RectOverlaps(rectTransform1, rectTransform6, menuCanvas) && !hoveredMenuContinue)
            {
                menuContinueSim.SimulateHover();
                hoveredMenuContinue = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform6, menuCanvas) && hoveredMenuContinue)
            {
                menuContinueSim.SimulateNormal();
                hoveredMenuContinue = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform6, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                menuContinueSim.SimulateClickDown();
                clickedMenuContinue = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform6, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                menuContinueSim.SimulateNormal();
                clickedMenuContinue = false;
            }
            if (Input.GetButtonUp("Cross") && clickedMenuContinue)
            {
                menuContinueSim.SimulateClickUp();
                clickedMenuContinue = false;
                hoveredMenuContinue = false;
            }

            //menuOptionsSim
            if (RectOverlaps(rectTransform1, rectTransform7, menuCanvas) && !hoveredMenuOptions)
            {
                menuOptionsSim.SimulateHover();
                hoveredMenuOptions = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform7, menuCanvas) && hoveredMenuOptions)
            {
                menuOptionsSim.SimulateNormal();
                hoveredMenuOptions = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform7, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                menuOptionsSim.SimulateClickDown();
                clickedMenuOptions = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform7, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                menuOptionsSim.SimulateNormal();
                clickedMenuOptions = false;
            }
            if (Input.GetButtonUp("Cross") && clickedMenuOptions)
            {
                menuOptionsSim.SimulateClickUp();
                CurrentScreen = "Options";
                clickedMenuOptions = false;
                hoveredMenuOptions = false;
            }

            //menuExitSim
            if (RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && !hoveredMenuExit)
            {
                menuExitSim.SimulateHover();
                hoveredMenuExit = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && hoveredMenuExit)
            {
                menuExitSim.SimulateNormal();
                hoveredMenuExit = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                menuExitSim.SimulateClickDown();
                clickedMenuExit = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                menuExitSim.SimulateNormal();
                clickedMenuExit = false;
            }
            if (Input.GetButtonUp("Cross") && clickedMenuExit)
            {
                menuExitSim.SimulateClickUp();
                CurrentScreen = "ExitMenu";
                clickedMenuExit = false;
                hoveredMenuExit = false;
            }
        }

        if (CurrentScreen == "Options")
        {
            //optionsBackSim
            if (RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && !hoveredOptionsBack)
            {
                optionsBackSim.SimulateHover();
                hoveredOptionsBack = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && hoveredOptionsBack)
            {
                optionsBackSim.SimulateNormal();
                hoveredOptionsBack = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsBackSim.SimulateClickDown();
                clickedOptionsBack = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                optionsBackSim.SimulateNormal();
                clickedOptionsBack = false;
            }
            if (Input.GetButtonUp("Cross") && clickedOptionsBack)
            {
                optionsBackSim.SimulateClickUp();
                clickedOptionsBack = false;
                CurrentScreen = "PauseMenu";
                hoveredOptionsBack = false;
            }

            //optionsApplySim
            if (RectOverlaps(rectTransform1, rectTransform3, menuCanvas) && !hoveredOptionsApply)
            {
                optionsApplySim.SimulateHover();
                hoveredOptionsApply = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform3, menuCanvas) && hoveredOptionsApply)
            {
                optionsApplySim.SimulateNormal();
                hoveredOptionsApply = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform3, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsApplySim.SimulateClickDown();
                clickedOptionsApply = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform3, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                optionsApplySim.SimulateNormal();
                clickedOptionsApply = false;
            }
            if (Input.GetButtonUp("Cross") && clickedOptionsApply)
            {
                optionsApplySim.SimulateClickUp();
                clickedOptionsApply = false;
                hoveredOptionsApply = false;
            }

            //optionsVolumeSim
            if (RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && !hoveredOptionsVolume)
            {
                optionsVolumeSim.SimulateHover();
                hoveredOptionsVolume = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && hoveredOptionsVolume)
            {
                optionsVolumeSim.SimulateNormal();
                hoveredOptionsVolume = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsVolumeSim.SimulateClickDown();
                clickedOptionsVolume = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                optionsVolumeSim.SimulateNormal();
                clickedOptionsVolume = false;
            }
            if (Input.GetButtonUp("Cross") && clickedOptionsVolume)
            {
                optionsVolumeSim.SimulateClickUp();
                clickedOptionsVolume = false;
                hoveredOptionsVolume = false;
            }
            if (Input.GetButtonDown("Cross") && !RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && !clickedOptionsVolume)
            {
                optionsVolumeSim.SimulateNormal();
                hoveredOptionsVolume = false;
                clickedOptionsVolume = false;
            }
            if (clickedOptionsVolume)
            {
                optionsVolumeSim.SimulateClickDown();
            }

            //optionsSensitivitySim
            if (RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && !hoveredOptionsSensitivity)
            {
                optionsSensitivitySim.SimulateHover();
                hoveredOptionsSensitivity = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && hoveredOptionsSensitivity)
            {
                optionsSensitivitySim.SimulateNormal();
                hoveredOptionsSensitivity = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsSensitivitySim.SimulateClickDown();
                clickedOptionsSensitivity = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                optionsSensitivitySim.SimulateNormal();
                clickedOptionsSensitivity = false;
            }
            if (Input.GetButtonUp("Cross") && clickedOptionsSensitivity)
            {
                optionsSensitivitySim.SimulateClickUp();
                clickedOptionsSensitivity = false;
                hoveredOptionsSensitivity = false;
            }
            if (Input.GetButtonDown("Cross") && !RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && !clickedOptionsSensitivity)
            {
                optionsSensitivitySim.SimulateNormal();
                hoveredOptionsSensitivity = false;
                clickedOptionsSensitivity = false;
            }
            if (clickedOptionsSensitivity)
            {
                optionsSensitivitySim.SimulateClickDown();
            }
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
