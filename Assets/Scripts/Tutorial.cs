using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public subdivide sd;
    public ResetView rv;
    public SpawnParticle sp;

    //audios
    public AudioSource welcome;
    public AudioSource controls;
    public AudioSource UI;
    public AudioSource concepts;
    public AudioSource setup;
    public AudioSource running;
    public AudioSource exploration;
    public AudioSource fin;
    public AudioSource visualizations;

    public GameObject leftController_example;
    public GameObject rightController_example;


    //animations
    public Animation anim;
    public GameObject tut_menu;

    public int state;

    void Start()
    {
        state = -1;
        // Debug.Log("about to play");
        // anim.Play("Welcome");
        // Debug.Log("played");
    }

    public void Progress()
    {
        // rv.ResetParticles();
        // rv.ResetViz();
        state += 1;

        switch(state)
        {
            case 0:
                anim.Play("Welcome");
                break;
            case 1:
                anim.Play("Controls");
                break;
            case 2:
                anim.Play("UI");
                break;
            case 3:
                anim.Play("Concepts");
                break;
            case 4:
                anim.Play("Visualizations");
                break;
            case 5:
                anim.Play("Setup");
                break;
            case 6:
                anim.Play("RunSim");
                break;
            case 7:
                anim.Play("Exploration");
                break;
            case 8:
                anim.Play("Fin");
                break;
            default:
                toggle_tutorial_menu();
                break;
        }
    }

    void toggle_tutorial_menu()
    {
        tut_menu.SetActive(!tut_menu.activeSelf);
    }

    public void Play_controls()
    {
        anim.Play("Controls");
    }
    public void Play_UI()
    {
        anim.Play("UI");
    }
    public void Play_Concepts()
    {
        anim.Play("Concepts");
    }
    public void Play_visualizations()
    {
        anim.Play("Visualizations");
    }
    public void Play_setup()
    {
        anim.Play("Setup");
    }
    public void Play_running()
    {
        anim.Play("RunSim");
    }
    public void Play_welcome()
    {
        anim.Play("Welcome");
    }
    public void Play_Fin()
    {
        anim.Play("Fin");
    }
    public void Play_exploration()
    {
        anim.Play("Exploration");
    }
}
