using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int scene;
    public Timer timer;
    private int iso_count = 0;
    private int vector_count = 0;
    private int hybrid_count = 0;
    private int step = 1;
    public GameObject done_text;
    public GameObject norm_text;
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public GameObject step4;
    public GameObject step5;
    public GameObject step6;
    public GameObject step7;
    public GameObject step8;

    public AudioClip a1;
    public AudioClip a2;
    public AudioClip a3;
    public AudioClip a4;
    public AudioClip a5;
    public AudioClip a6;
    public AudioClip a7;
    public AudioClip a8;

    AudioSource audioo;

    public subdivide sd;
    public SpawnParticle sp;

    private string filePath = "";

    public RenderTexture sim_cam;
    void Start()
    {
        audioo = GetComponent<AudioSource>();
        Debug.Log(Application.persistentDataPath);
        filePath = Application.persistentDataPath;
        //any start stuff based on scene
        switch (scene)
        {
            case 0:
                timer.startTimer(600);
                audioo.clip = a1;
                audioo.Play();
                break;
            case 1: //task 1
                timer.startTimer(300);
                break;
            case 2: //task 2
                timer.startTimer(300);
                break;
            case 3: //task 3
                timer.startTimer(300);
                break;
            case 4: //free roam
                timer.startTimer(600);
                break;
            default:
                break;
        }
    }
    void load2() {
        step1.SetActive(false);
        step2.SetActive(true);
        timer.SetTextSource(step2.GetComponentInChildren<TextMeshProUGUI>());
        audioo.clip = a2;
        audioo.Play();
        //sound
    }
    void load3() {
        step2.SetActive(false);
        step3.SetActive(true);
        timer.SetTextSource(step3.GetComponentInChildren<TextMeshProUGUI>());
        sp.spawnParticle();
        audioo.clip = a3;
        audioo.Play();
    }
    void load4() {
        step3.SetActive(false);
        step4.SetActive(true);
        timer.SetTextSource(step4.GetComponentInChildren<TextMeshProUGUI>());
        audioo.clip = a4;
        audioo.Play();
    }
    void load5() {
        step4.SetActive(false);
        step5.SetActive(true);
        timer.SetTextSource(step5.GetComponentInChildren<TextMeshProUGUI>());
        sd.sim();
        audioo.clip = a5;
        audioo.Play();
    }
    void load6() {
        step5.SetActive(false);
        step6.SetActive(true);
        timer.SetTextSource(step6.GetComponentInChildren<TextMeshProUGUI>());
        sd.NewToggle("CUTTING");
        // GameObject.Find("").transform.
        audioo.clip = a6;
        audioo.Play();
    }
    void load7() {
        step6.SetActive(false);
        step7.SetActive(true);
        timer.SetTextSource(step7.GetComponentInChildren<TextMeshProUGUI>());
        sd.NewToggle("HYBRID3");
        audioo.clip = a7;
        audioo.Play();
    }
    void load8() {
        step7.SetActive(false);
        step8.SetActive(true);
        timer.SetTextSource(step8.GetComponentInChildren<TextMeshProUGUI>());
        audioo.clip = a8;
        audioo.Play();
    }

    public void redoTut()
    {
        sd.resetter.ResetBoth();
        step8.SetActive(false);
        step1.SetActive(true);
        step = 1;
    }
    public void Continue()
    {
        switch (step)
        {
            case 1:
                load2();
                break;
            case 2:
                load3();
                break;
            case 3:
                load4();
                break;
            case 4:
                load5();
                break;
            case 5:
                load6();
                break;
            case 6:
                load7();
                break;
            case 7:
                load8();
                break;
            default:
                break;
        }
        step++;
    }

    public void Done(){
        timer.DisableCountdown();
        float time_left = timer.getTime();
        float time_spent = 0;
        if(sim_cam)
        {
            Texture2D image = new Texture2D(512, 512, TextureFormat.RGB24, false);
            RenderTexture.active = sim_cam;
            image.ReadPixels(new Rect(0, 0, sim_cam.width, sim_cam.height), 0, 0);
            image.Apply();
            byte[] bytes = image.EncodeToPNG();
            File.WriteAllBytes(filePath+"/task"+scene.ToString()+".png", bytes);
        }
        switch (scene)
        {
            case 0:
                if (time_left <= 0)
                    time_spent = 300;
                else
                    time_spent = 300 - time_left;
                break;
            case 1: //task 1
                if (time_left <= 0)
                    time_spent = 300;
                else
                    time_spent = 300 - time_left;
                break;
            case 2: //task 2
                if (time_left <= 0)
                    time_spent = 300;
                else
                    time_spent = 300 - time_left;
                break;
            case 3: //task 3
                if (time_left <= 0)
                    time_spent = 300;
                else
                    time_spent = 300 - time_left;
                break;
            case 4: //free roam
                if (time_left <= 0)
                    time_spent = 600;
                else
                    time_spent = 600 - time_left;
                
                break;
            default:
                break;
        }
        string entry = "Scene #" + scene.ToString() + ": " + time_spent.ToString();
        if (scene == 4)
        {
            //write vis counts as well
            entry += ", " + vector_count.ToString() + ", " + iso_count.ToString() + ", " + hybrid_count.ToString();
        }
        entry += "\n";
        File.AppendAllText(filePath + "/data.txt", entry);
        done_text.SetActive(true);
        if(scene == 0)
        {
            step1.SetActive(false);
            step2.SetActive(false);
            step3.SetActive(false);
            step4.SetActive(false);
            step5.SetActive(false);
            step6.SetActive(false);
            step7.SetActive(false);
            step8.SetActive(false);
        }
        else
            norm_text.SetActive(false);
    }

    public void Back()
    {
        SceneManager.LoadScene (sceneName:"Scenes/start");
    }

    public void incVector()
    {
        vector_count++;
    }

    public void incIso()
    {
        iso_count++;
    }

    public void incHybrid()
    {
        hybrid_count++;
    }

}
