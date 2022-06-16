using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class BButtonListener : MonoBehaviour
{

    public ButtonHandler BButtonHandler = null;
    public Tutorial inputShtic;

    public void OnEnable()
    {
        BButtonHandler.OnButtonDown += BButtonDown;
        BButtonHandler.OnButtonUp += BButtonUp;
    }

    public void OnDisable()
    {
        BButtonHandler.OnButtonDown -= BButtonDown;
        BButtonHandler.OnButtonUp -= BButtonUp;
    }

    private void BButtonDown(XRController controller)
    {
        print("B button down");
        inputShtic.state = 8;
        //inputShtic.GetComponent<SpawnParticle>().spawnParticle();

    }

    private void BButtonUp(XRController controller)
    {
        print("B button up");
    }
}
