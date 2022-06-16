using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.XR.Interaction.Toolkit;

public class Animations : MonoBehaviour
{

    public Tutorial tut;
    public bool playing;

    public void playWelcome()
    {
        tut.welcome.Play();
    }

    public void togglePlaying()
    {
        playing = !playing;
        List<GameObject> rootObjectsInScene = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjectsInScene);
        Debug.Log(playing);
        if (playing)
        {
            for (int i = 0; i < rootObjectsInScene.Count; i++)
            {
                XRGrabInteractable[] allComponents = rootObjectsInScene[i].GetComponentsInChildren<XRGrabInteractable>(true);
                Debug.Log("disabling masks");
                Debug.Log(allComponents.Length);
                for (int j = 0; j < allComponents.Length; j++)
                {
                    allComponents[j].interactionLayerMask = LayerMask.GetMask("Nothing");
                }
            }
        }
        else
        {
            for (int i = 0; i < rootObjectsInScene.Count; i++)
            {
                XRGrabInteractable[] allComponents = rootObjectsInScene[i].GetComponentsInChildren<XRGrabInteractable>(true);
                Debug.Log("enabling masks");
                Debug.Log(allComponents.Length);
                for (int j = 0; j < allComponents.Length; j++)
                {
                    allComponents[j].interactionLayerMask = ~0;
                }
            }
        }
    }
    public void playFin()
    {
        tut.fin.Play();
    }

    public void playConcepts()
    {
        tut.concepts.Play();
    }

    public void playControls()
    {
        tut.controls.Play();
    }

    public void playVisualizations()
    {
        tut.visualizations.Play();
    }
    public void playUI()
    {
        tut.UI.Play();
    }

    public void playSetup()
    {
        tut.setup.Play();
    }

    public void playExploration()
    {
        tut.exploration.Play();
    }

    public void playRunning()
    {
        tut.running.Play();
    }


    public void SpawnFixedPositiveParticle()
    {
        tut.sp.spawnDemoParticle(0);

    }

    public void resetViz()
    {
        tut.rv.ResetViz();
    }

    public void resetParticles()
    {
        tut.rv.ResetViz();
    }

    public void SetMode(string i)
    {
        tut.sd.SetMODE(i);
    }

    public void resetPos()
    {
        tut.rv.resetPos();
    }
}
