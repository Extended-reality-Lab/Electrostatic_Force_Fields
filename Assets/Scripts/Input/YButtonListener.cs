using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class YButtonListener : MonoBehaviour
{

    public ButtonHandler YButtonHandler = null;
    public GameObject[] legends;
    public LRSSender lrs;
    public Animations ani;

    public void OnEnable()
    {
        YButtonHandler.OnButtonDown += YButtonDown;
        YButtonHandler.OnButtonUp += YButtonUp;
    }

    public void OnDisable()
    {
        YButtonHandler.OnButtonDown -= YButtonDown;
        YButtonHandler.OnButtonUp -= YButtonUp;
    }

    private void YButtonDown(XRController controller)
    {
        print("Y button down");
        if(!ani.playing)
        {
            foreach(GameObject legend in legends)
            {
                legend.SetActive(!legend.activeSelf);
            }
        }
        lrs.SendLRS("User","Toggled","Controller Legend",20);
        
    }

    private void YButtonUp(XRController controller)
    {
        print("Y button up");
    }
}