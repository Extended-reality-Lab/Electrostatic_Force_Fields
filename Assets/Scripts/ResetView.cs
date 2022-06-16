using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResetView : MonoBehaviour
{
    public GameObject vector_desc;
    public GameObject iso_desc;
    public GameObject hybrid_desc;

    public GameObject iso_info;
    public GameObject v_h_info;
    public GameObject info;
    public GameObject basic;
    // public GameObject heat_toggle;
    public UIController uicontroller;
    // public LRSSender lrs;
    public bool viz_up;

    public GameObject sim_area;
    public GameObject tablet1;
    public GameObject tablet2;
    public GameObject tut_menu;
    public subdivide sd;
    public TextMeshProUGUI logger;
    
    
    public void ResetViz()
    {
        
        sd.vis_running = false;
        // lrs.SendLRS("User","reset ","Visualization",8);
        GameObject.Find("Simulation Box").GetComponent<MeshRenderer>().enabled = true;
        var to_delete = GameObject.FindGameObjectsWithTag("viz_component");
        foreach (var item in to_delete)
        {
            GameObject.Destroy(item);
        }
        info.SetActive(false);
        basic.SetActive(true);
        vector_desc.SetActive(false);
        iso_desc.SetActive(true);
        hybrid_desc.SetActive(false);
        iso_info.SetActive(true);
        v_h_info.SetActive(false);
        // heat_toggle.GetComponent<Toggle>().isOn = true;
        uicontroller.on = false;
        sd.ClearViz();
    }

    

    public void ResetParticles() 
    {
        // lrs.SendLRS("User","reset ","particles",16);

        // var canvas = GameObject.Find("Canvas");
        // UIController controller = canvas.GetComponent<UIController>();
        var canvas = GameObject.Find("particle_canvas");
        UIController controller = canvas.GetComponent<UIController>();
       
        var to_delete = GameObject.FindGameObjectsWithTag("particle_component");
        foreach (var item in to_delete)
        {
            GameObject.Destroy(item);
        }
        if (controller) {
            controller.num_particles = 0;
        }
    }

    public void ResetBoth() {
        // logger.text = to_delete.Length.ToString();
        ResetViz();
        ResetParticles();

        
        sd.boxmaton();
    }

    public void resetPos()
    {
        tut_menu.SetActive(false);
        sim_area.transform.localPosition = new Vector3(-1.826311f, 0.1132083f, -5.329831f);
        sim_area.transform.localScale = new Vector3(1f, 1f, 1f);
        sim_area.transform.eulerAngles = new Vector3(0f, 0f, 0f);

        tablet1.transform.localPosition = new Vector3(-1.27039f, 0.0862f, -5.5842f);
        tablet1.transform.eulerAngles = new Vector3(0f,60f,0f);

        tablet2.transform.localPosition = new Vector3(-1.036f, 0.0993f, -6.075f);
        tablet2.transform.eulerAngles = new Vector3(0f,60f, 0f);
    }

    public void resetPos2()
    {
        sim_area.transform.localScale = new Vector3(1f, 1f, 1f);
        sim_area.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
