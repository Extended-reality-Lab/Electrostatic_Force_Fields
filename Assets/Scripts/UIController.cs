using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{

    public int num_particles;

    public GameObject particle_ui;
    public GameObject iso_ui;
    public GameObject equations;
    public Animations ani;

    private Dictionary<int, int> p_to_ui = new Dictionary<int, int>();
    private Dictionary<int, int> i_to_ui = new Dictionary<int, int>();
    public LRSSender lrs;
    public bool on;

    void Start()
    {
        num_particles = 0;
        on = false;
    }

    public void createPUI(bool charge, int id)
    {
        GameObject o = GameObject.Instantiate(particle_ui);
        o.name = o.GetInstanceID().ToString();
        o.transform.parent = gameObject.transform;
        o.transform.localPosition = new Vector3(0, 120f - (80f * num_particles), 0);
        o.transform.localScale = new Vector3(2, 1, 1);
        o.transform.rotation = new Quaternion(0, 0, 0, 0);
        o.tag = "particle_component";
        p_to_ui.Add(o.GetInstanceID(), id);
        TextMeshProUGUI text = o.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Charge " + (num_particles + 1);
        if (charge)
            text.color = Color.blue;
        else
            text.color = Color.red;
        num_particles++;
    }

    public void createIUI(string text_to_do, int id, int numnum, float target, Vector3[] verts)
    {

        GameObject o = GameObject.Instantiate(iso_ui);
        o.name = o.GetInstanceID().ToString();
        o.transform.parent = equations.transform;
        o.transform.localPosition = new Vector3(0.6f, 1.9f - (.2f * numnum), 0);
        o.transform.localScale = new Vector3(0.0015f, 0.0015f, 1f);
        o.transform.rotation = new Quaternion(0, 0, 0, 0);
        o.tag = "viz_component";
        i_to_ui.Add(o.GetInstanceID(), id);
        TextMeshProUGUI text = o.GetComponentInChildren<TextMeshProUGUI>();
        text.text = text_to_do;
    }


    public void flip_particle(int id)
    {
        if (ani.playing)
            return;
        int p_id;

        if (p_to_ui.TryGetValue(id, out p_id))
        {
            GameObject particle = GameObject.Find(p_id.ToString());
            Particle p = particle.GetComponent<Particle>();
            GameObject ui_com = GameObject.Find(id.ToString());
            TextMeshProUGUI text = ui_com.GetComponentInChildren<TextMeshProUGUI>();
            if (p.charge > 0)
            {
                p.charge = -1.6f;
                text.color = Color.blue;
            }
            else
            {
                p.charge = 1.6f;
                text.color = Color.red;
            }
            p.mod_material();
            lrs.SendLRS("User", "Flipped Signage", "Particle" + p_id.ToString(), 1);
        }

    }

    public void delete_particle(int id)
    {
        if (ani.playing)
            return;
        int p_id;

        if (p_to_ui.TryGetValue(id, out p_id))
        {
            GameObject particle = GameObject.Find(p_id.ToString());
            GameObject ui_com = GameObject.Find(id.ToString());
            GameObject.Destroy(particle);
            GameObject.Destroy(ui_com);
            num_particles--;
            lrs.SendLRS("User", "Deleted", "Particle" + p_id.ToString(), 2);

            //reformat uis
        }
    }

    public void toggle_iso(int id)
    {
        if (ani.playing)
            return;
        int i_id;
        if (i_to_ui.TryGetValue(id, out i_id))
        {
            Debug.Log(id);
            GameObject iso = GameObject.Find(i_id.ToString());
            if (iso.GetComponent<MeshRenderer>().enabled == false)
                iso.GetComponent<MeshRenderer>().enabled = true;
            else
                iso.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
    