using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOptions : MonoBehaviour
{

    UIController controller;
    public GameObject p_info;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("particle_canvas").GetComponent<UIController>();
    }


    public void Remove() {
        controller.delete_particle(p_info.GetInstanceID());
    }

    public void Flip() {
        controller.flip_particle(p_info.GetInstanceID());
    }

}
