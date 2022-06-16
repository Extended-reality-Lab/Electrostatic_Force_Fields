using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public static class InputHelper
{
    public enum Button
    {
        None = 0,
        MenuButton,
        Trigger,
        Grip, 
        TriggerPressed,
        GripPressed,
        PrimaryButton,
        PrimaryTouch,
        SecondaryButton,
        SecondaryTouch,
        Primary2DAxisTouch,
        Primary2DAxisClick,
        Secondary2DAxisTouch,
        Secondary2DAxisClick,
        PrimaryAxis2DUp,
        PrimaryAxis2DDown,
        PrimaryAxis2DLeft,
        PrimaryAxis2DRight,
        SecondaryAxis2DUp,
        SecondaryAxis2DDown,
        SecondaryAxis2DLeft,
        SecondaryAxis2DRight
    }

    enum ButtonReadType
    {
        None = 0,
        Binary,
        Axis1D,
        Axis2DUp,
        Axis2DDown,
        Axis2DLeft,
        Axis2DRight
    }

    struct ButtonInfo
    {
        public string name;
        public ButtonReadType type;
        public ButtonInfo(string name, ButtonReadType type)
        {
            this.name = name;
            this.type = type;
        }
    }

    static ButtonInfo[] s_ButtonData = new ButtonInfo[]
    {
        new ButtonInfo("", ButtonReadType.None),
        new ButtonInfo("MenuButton", ButtonReadType.Binary),
        new ButtonInfo("Trigger", ButtonReadType.Axis1D),
        new ButtonInfo("Grip", ButtonReadType.Axis1D),
        new ButtonInfo("TriggerPressed", ButtonReadType.Binary),
        new ButtonInfo("GripPressed", ButtonReadType.Binary),
        new ButtonInfo("PrimaryButton", ButtonReadType.Binary),
        new ButtonInfo("PrimaryTouch", ButtonReadType.Binary),
        new ButtonInfo("SecondaryButton", ButtonReadType.Binary),
        new ButtonInfo("SecondaryTouch", ButtonReadType.Binary),
        new ButtonInfo("Primary2DAxisTouch", ButtonReadType.Binary),
        new ButtonInfo("Primary2DAxisClick", ButtonReadType.Binary),
        new ButtonInfo("Secondary2DAxisTouch", ButtonReadType.Binary),
        new ButtonInfo("Secondary2DAxisClick", ButtonReadType.Binary),
        new ButtonInfo("PrimaryAxis2DUp", ButtonReadType.Axis2DUp),
        new ButtonInfo("PrimaryAxis2DDown", ButtonReadType.Axis2DDown),
        new ButtonInfo("PrimaryAxis2DLeft", ButtonReadType.Axis2DLeft),
        new ButtonInfo("PrimaryAxis2DRight", ButtonReadType.Axis2DRight),
        new ButtonInfo("SecondaryAxis2DUp", ButtonReadType.Axis2DUp),
        new ButtonInfo("SecondaryAxis2DDown", ButtonReadType.Axis2DDown),
        new ButtonInfo("SecondaryAxis2DLeft", ButtonReadType.Axis2DLeft),
        new ButtonInfo("SecondaryAxis2DRight", ButtonReadType.Axis2DRight),
    };

    static float s_DefaultPressThreshold = 0.1f;

    public static bool isPressed(this InputDevice device, Button button, out bool isPressed, float pressthreshold = -1.0f)
    {
        // if((int)button >= s_ButtonData.Length)
        // {
        //     throw new ArgumentExpection("[InputHelper.IsPressed] The value of <button> is out of the supported range");
        // }

        if (!device.isValid)
        {
            isPressed = false;
            return false;
        }

        ButtonInfo info = s_ButtonData[(int)button];

        switch(info.type)
        {
            case ButtonReadType.Binary:
                {
                    if(device.TryGetFeatureValue(new InputFeatureUsage<bool>(info.name), out bool value))
                    {
                        //Debug.Log(value);
                        isPressed = value;
                        return true;
                    }
                }
                break;
            case ButtonReadType.Axis1D:
                {
                    if(device.TryGetFeatureValue(new InputFeatureUsage<float>(info.name), out float value))
                    {
                        float threshold = (pressthreshold >= 0.0f) ? pressthreshold : s_DefaultPressThreshold;
                        isPressed = value >= threshold; 
                        return true;
                    }
                }
                break;
            case ButtonReadType.Axis2DUp:
                {
                    if(device.TryGetFeatureValue(new InputFeatureUsage<Vector2>(info.name), out Vector2 value))
                    {
                        float threshold = (pressthreshold >= 0.0f) ? pressthreshold : s_DefaultPressThreshold;
                        isPressed = value.y >= threshold;
                        return true;
                    }
                }
                break;
            case ButtonReadType.Axis2DDown:
                {
                    if(device.TryGetFeatureValue(new InputFeatureUsage<Vector2>(info.name), out Vector2 value))
                    {
                        float threshold = (pressthreshold >= 0.0f) ? pressthreshold : s_DefaultPressThreshold;
                        isPressed = value.y <= -threshold;
                        return true;
                    }
                }
                break;
            case ButtonReadType.Axis2DRight:
                {
                    if(device.TryGetFeatureValue(new InputFeatureUsage<Vector2>(info.name), out Vector2 value))
                    {
                        float threshold = (pressthreshold >= 0.0f) ? pressthreshold : s_DefaultPressThreshold;
                        isPressed = value.x >= threshold;
                        return true;
                    }
                }
                break;
            case ButtonReadType.Axis2DLeft:
                {
                    if(device.TryGetFeatureValue(new InputFeatureUsage<Vector2>(info.name), out Vector2 value))
                    {
                        float threshold = (pressthreshold >= 0.0f) ? pressthreshold : s_DefaultPressThreshold;
                        isPressed = value.x <= -threshold;
                        return true;
                    }
                }
                break;
            default:
                {
                    isPressed = false;
                    return false;
                }
        }
        isPressed = false;
        return false;
    }
}
