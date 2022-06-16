using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RightJoystickClickListener : MonoBehaviour
{

    public ButtonHandler RightJoystickHandler = null;
    public GameObject inputShtic;
    public Animations ani;

    public void OnEnable()
    {
        RightJoystickHandler.OnButtonDown += RightJoystickDown;
        RightJoystickHandler.OnButtonUp += RightJoystickUp;
    }

    public void OnDisable()
    {
        RightJoystickHandler.OnButtonDown -= RightJoystickDown;
        RightJoystickHandler.OnButtonUp -= RightJoystickUp;
    }

    private void RightJoystickDown(XRController controller)
    {
        print("Right Joystick down");
    }

    private void RightJoystickUp(XRController controller)
    {
        print("Right Joystick up");
    }
}
