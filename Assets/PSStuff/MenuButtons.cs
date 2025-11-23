using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour
{
    public RectTransform cursor;
    public Image options;
    public Image exit;
    public Image newGame;
    public Image continueButton;

    public ButtonSimulator optionsSim;
    public ButtonSimulator exitSim;
    public ButtonSimulator newGameSim;
    public ButtonSimulator continueSim;

    private bool hoveredOptions;
    private bool hoveredExit;
    private bool hoveredNewGame;
    private bool hoveredContinue;

    private bool clickedOptions;
    private bool clickedExit;
    private bool clickedNewGame;
    private bool clickedContinue;

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

    public Image optionsVolume;
    public SliderSimulator optionsVolumeSim;
    private bool clickedOptionsVolume;
    private bool hoveredOptionsVolume;

    private string CurrentScreen;

    public Canvas menuCanvas;

    void Awake()
    {
        CurrentScreen = "MainMenu";
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform1 = cursor;
        RectTransform rectTransform2 = options.rectTransform;
        RectTransform rectTransform3 = exit.rectTransform;
        RectTransform rectTransform4 = newGame.rectTransform;
        RectTransform rectTransform5 = continueButton.rectTransform;
        RectTransform rectTransform6 = optionsBack.rectTransform;
        RectTransform rectTransform7 = optionsApply.rectTransform;
        RectTransform rectTransform8 = optionsVolume.rectTransform;
        RectTransform rectTransform9 = optionsSensitivity.rectTransform;

        if (CurrentScreen == "MainMenu")
        {
            //optionsSim
            if (RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && !hoveredOptions)
            {
                optionsSim.SimulateHover();
                hoveredOptions = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && hoveredOptions)
            {
                optionsSim.SimulateNormal();
                hoveredOptions = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsSim.SimulateClickDown();
                clickedOptions = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform2, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                optionsSim.SimulateNormal();
                clickedOptions = false;
            }
            if (Input.GetButtonUp("Cross") && clickedOptions)
            {
                optionsSim.SimulateClickUp();
                clickedOptions = false;
                CurrentScreen = "Options";
                hoveredOptions = false;
            }

            //exitSim
            if (RectOverlaps(rectTransform1, rectTransform3, menuCanvas) && !hoveredExit)
            {
                exitSim.SimulateHover();
                hoveredExit = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform3, menuCanvas) && hoveredExit)
            {
                exitSim.SimulateNormal();
                hoveredExit = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform3, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                exitSim.SimulateClickDown();
                clickedExit = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform3, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                exitSim.SimulateNormal();
                clickedExit = false;
            }
            if (Input.GetButtonUp("Cross") && clickedExit)
            {
                exitSim.SimulateClickUp();
                clickedExit = false;
                hoveredExit = false;
            }

            //newGameSim
            if (RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && !hoveredNewGame)
            {
                newGameSim.SimulateHover();
                hoveredNewGame = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && hoveredNewGame)
            {
                newGameSim.SimulateNormal();
                hoveredNewGame = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                newGameSim.SimulateClickDown();
                clickedNewGame = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform4, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                newGameSim.SimulateNormal();
                clickedNewGame = false;
            }
            if (Input.GetButtonUp("Cross") && clickedNewGame)
            {
                newGameSim.SimulateClickUp();
                clickedNewGame = false;
                hoveredNewGame = false;
            }

            //continueSim
            if (RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && !hoveredContinue)
            {
                continueSim.SimulateHover();
                hoveredContinue = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && hoveredContinue)
            {
                continueSim.SimulateNormal();
                hoveredContinue = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                continueSim.SimulateClickDown();
                clickedContinue = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform5, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                continueSim.SimulateNormal();
                clickedContinue = false;
            }
            if (Input.GetButtonUp("Cross") && clickedContinue)
            {
                continueSim.SimulateClickUp();
                clickedContinue = false;
                hoveredContinue = false;
            }
        }


        if (CurrentScreen == "Options")
        {
            //optionsBackSim
            if (RectOverlaps(rectTransform1, rectTransform6, menuCanvas) && !hoveredOptionsBack)
            {
                optionsBackSim.SimulateHover();
                hoveredOptionsBack = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform6, menuCanvas) && hoveredOptionsBack)
            {
                optionsBackSim.SimulateNormal();
                hoveredOptionsBack = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform6, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsBackSim.SimulateClickDown();
                clickedOptionsBack = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform6, menuCanvas) && !Input.GetButtonDown("Cross"))
            {
                optionsBackSim.SimulateNormal();
                clickedOptionsBack = false;
            }
            if (Input.GetButtonUp("Cross") && clickedOptionsBack)
            {
                optionsBackSim.SimulateClickUp();
                clickedOptionsBack = false;
                CurrentScreen = "MainMenu";
                hoveredOptionsBack = false;
            }

            //optionsApplySim
            if (RectOverlaps(rectTransform1, rectTransform7, menuCanvas) && !hoveredOptionsApply)
            {
                optionsApplySim.SimulateHover();
                hoveredOptionsApply = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform7, menuCanvas) && hoveredOptionsApply)
            {
                optionsApplySim.SimulateNormal();
                hoveredOptionsApply = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform7, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsApplySim.SimulateClickDown();
                clickedOptionsApply = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform7, menuCanvas) && !Input.GetButtonDown("Cross"))
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
            if (RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && !hoveredOptionsVolume)
            {
                optionsVolumeSim.SimulateHover();
                hoveredOptionsVolume = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && hoveredOptionsVolume)
            {
                optionsVolumeSim.SimulateNormal();
                hoveredOptionsVolume = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsVolumeSim.SimulateClickDown();
                clickedOptionsVolume = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && !Input.GetButtonDown("Cross"))
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
            if (Input.GetButtonDown("Cross") && !RectOverlaps(rectTransform1, rectTransform8, menuCanvas) && !clickedOptionsVolume)
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
            if (RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && !hoveredOptionsSensitivity)
            {
                optionsSensitivitySim.SimulateHover();
                hoveredOptionsSensitivity = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && hoveredOptionsSensitivity)
            {
                optionsSensitivitySim.SimulateNormal();
                hoveredOptionsSensitivity = false;
            }
            if (RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && Input.GetButtonDown("Cross"))
            {
                optionsSensitivitySim.SimulateClickDown();
                clickedOptionsSensitivity = true;
            }
            if (!RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && !Input.GetButtonDown("Cross"))
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
            if (Input.GetButtonDown("Cross") && !RectOverlaps(rectTransform1, rectTransform9, menuCanvas) && !clickedOptionsSensitivity)
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
