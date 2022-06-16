using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{

    public float speed = 1;
    public XRNode input;
    private Vector2 input_axis;
    // Start is called before the first frame update
    private CharacterController character;

    //private XRRig  rig;
    void Start()
    {
        character = GetComponent<CharacterController>();
       // rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(input);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out input_axis);

    }
    private void FixedUpdate()
    {
       // Quaternion headyaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direc = new Vector3(input_axis.x, 0, input_axis.y);
        character.Move(direc * Time.fixedDeltaTime * speed);
    }
}
