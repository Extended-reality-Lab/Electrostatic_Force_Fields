using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
public class ss : MonoBehaviour
{
    public Material mat;
    public Material mat2;

    // Start is called before the first frame update
    void Start()
    {
        Wireframe frame = gameObject.AddComponent<Wireframe>();
        frame.mat = mat;
        frame.mat2 = mat2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
