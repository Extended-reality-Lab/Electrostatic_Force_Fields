using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

[CreateAssetMenu(fileName = "NewButtonHandler")]
public class ButtonHandler : InputHandler
{

    public InputHelper.Button button = InputHelper.Button.None;

    public delegate void StateChange(XRController controller);
    public event StateChange OnButtonDown;
    public event StateChange OnButtonUp;

    private bool previousPress = false;

    public override void HandleState(XRController controller)
    {
        if(controller.inputDevice.isPressed(button, out bool pressed, controller.axisToPressThreshold))
        {
           // Debug.Log(pressed);
            if(pressed != previousPress)
            {
                // if(previousPress != pressed)
                // {
                    previousPress = pressed;

                    if(pressed)
                    {
                        OnButtonDown?.Invoke(controller);
                    }
                    else
                    {
                        OnButtonUp?.Invoke(controller);
                    }
                // }
            }
        }
    }

    IEnumerator accept_in(float delay){
      yield return new WaitForSeconds(delay);
 }
}