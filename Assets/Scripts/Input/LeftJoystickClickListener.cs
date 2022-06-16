using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftJoystickClickListener : MonoBehaviour
{

    public ButtonHandler LeftJoystickHandler = null;
    public GameObject inputShtic;
    public Animations ani;

    public void OnEnable()
    {
        LeftJoystickHandler.OnButtonDown += LeftJoystickDown;
        LeftJoystickHandler.OnButtonUp += LeftJoystickUp;
    }

    public void OnDisable()
    {
        LeftJoystickHandler.OnButtonDown -= LeftJoystickDown;
        LeftJoystickHandler.OnButtonUp -= LeftJoystickUp;
    }

    private void LeftJoystickDown(XRController controller)
    {
        print("Left Joystick down");
        if(!ani.playing)
            inputShtic.GetComponent<ResetView>().ResetBoth();
        
    }

    private void LeftJoystickUp(XRController controller)
    {
        print("Left Joystick up");
    }
}
