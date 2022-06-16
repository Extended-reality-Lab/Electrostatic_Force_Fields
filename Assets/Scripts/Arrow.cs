using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Arrow : MonoBehaviour
{

    public float force;
    public Vector3 pos;

    public int nparticles;

    public Vector3 force_dir;

    public Material base_tip_mat;

    public Material base_shaft_mat;
    public Material highlight_tip_mat;
    public Material simple;
    public Color color;

    public Material highlight_shaft_mat;

    public GameObject tip;
    public GameObject shaft;
    bool heat = false;

    public TextMeshProUGUI particle_locations;
    public TextMeshProUGUI distance_vectors;
    public TextMeshProUGUI force_vectors;
    public TextMeshProUGUI final_force;
    Pivot p;
    
    private float c_constant = 8.9875517923f;
    //private float c_constant = 90000.0f;
    public void OnSelect()
    {
        //highlight
        Debug.Log("highlighting arrow");
        tip.GetComponent<MeshRenderer>().material = highlight_tip_mat;
        shaft.GetComponent<MeshRenderer>().material = highlight_shaft_mat;

        //maths

        GameObject o = GameObject.Find("Simulation Box");
        Particle[] particles  = o.transform.parent.GetComponentsInChildren<Particle>();
        List<Vector3> init_vectors = new List<Vector3>();
        int counter = 0;
        string step1 = "";
        string step2 = "";
        string step3 = "";
        string step4 = "";
        foreach (Particle p in particles) {

            float dist = Vector3.Distance(p.transform.localPosition, pos);
            float weight =  (c_constant*Mathf.Pow(p.charge,2))/(Mathf.Pow(dist,2));
            Vector3 vec = p.transform.localPosition - pos;

            init_vectors.Add(-vec * weight);
            if (counter == 0)
            {
                step1 += "p" + (counter+1) + ": " + p.transform.localPosition.ToString();
                step2 += "p" + (counter+1) + ": "+ dist;
                step3 += "p" + (counter+1) + ": "+ (-vec * weight);
            }
            if (counter < 2)
            {
                step1 += "\np" + (counter+1) + ": " + p.transform.localPosition.ToString();
                step2 += "\np" + (counter+1) + ": "+ dist + " ";
                step3 += "\np" + (counter+1) + ": "+ (-vec * weight) + " ";
            }
            if (counter == 5)
            {
                step1 += "...";
                step2 += "...";
                step3 += "...";
            }
            counter++;
        }
        //add all vectors together
        //normalize vectors
        string text = "Number of influencing charges:\n" +nparticles+"\nMagnitude of force:\n"+(force)+"\nforce Directional vector:\n"+force_dir.normalized.ToString();
        counter = 0;
        Vector3 final_vec = new Vector3(0,0,0);
        foreach (Vector3 vec in init_vectors)
        {
            final_vec += vec;
        }
        //display
        step4 =  final_vec.magnitude + " N : " + final_vec.normalized.ToString();
    
        p = GameObject.Find("Controller_input_manager").GetComponent<Pivot>();
        p.GetParticleLocation().text = step1;
        p.GetDistanceVectors().text = step2;
        p.GetForceVectors().text = step3;
        p.GetFinalForce().text = step4;
    }

    public void goHeat(bool on_off)
    {
        if (on_off)
        {
            shaft.GetComponent<MeshRenderer>().material = simple;
            shaft.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
            heat = true;
        }
        else
        {
            shaft.GetComponent<MeshRenderer>().material = base_shaft_mat;
            heat = false;
        }
    }

    public void GoUnHeat()
    {
        shaft.GetComponent<MeshRenderer>().material = base_shaft_mat;
        heat = false;
    }
    public void OnRelease()
    {
        //reset materials
        Debug.Log("unhighlighting arrow");
        if(heat)
        {
            shaft.GetComponent<MeshRenderer>().material = simple;
        }
        else
        {
        shaft.GetComponent<MeshRenderer>().material = base_shaft_mat;
        }

        tip.GetComponent<MeshRenderer>().material = base_tip_mat;
        //clear display

        GameObject go = GameObject.FindGameObjectWithTag("tooltip_text");
        // TextMeshProUGUI t = go.GetComponent<TextMeshProUGUI>();

        // t.text = "";
    }
}
