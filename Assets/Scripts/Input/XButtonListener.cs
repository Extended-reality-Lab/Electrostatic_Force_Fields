using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XButtonListener : MonoBehaviour
{

    public ButtonHandler XButtonHandler = null;
    public Tutorial inputShtic;
    public Animations ani;

    public void OnEnable()
    {
        XButtonHandler.OnButtonDown += XButtonDown;
        XButtonHandler.OnButtonUp += XButtonUp;
    }

    public void OnDisable()
    {
        XButtonHandler.OnButtonDown -= XButtonDown;
        XButtonHandler.OnButtonUp -= XButtonUp;
    }

    private void XButtonDown(XRController controller)
    {
        print("X button down");
        if(!ani.playing)
            inputShtic.Progress();
    }

    private void XButtonUp(XRController controller)
    {
        print("X button up");
    }
}