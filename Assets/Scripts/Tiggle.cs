using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiggle : MonoBehaviour
{

    UIController controller;
    public GameObject iso_info;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("particle_canvas").GetComponent<UIController>();
    }

    // Update is called once per frame
    void toggle()
    {
        controller.toggle_iso(iso_info.GetInstanceID());
    }
}
