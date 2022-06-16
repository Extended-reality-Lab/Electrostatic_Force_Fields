using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

using MarchingCubesProject;

public class subdivide : MonoBehaviour
{
    public GameObject[] rerender_objs;
    public GameObject arrow_prefab;
    public int scene;
    public StageManager SM;
    public Material boxmat1;
    public Material boxmat2;
    public bool test;
    private GameObject arrows;
    public ResetView resetter;

    public GameObject isoHolder;
    public GameObject isoPrefab;
    public Material mat;
    public UIController Controller;
    public Material mat2;
    public MODE mode = MODE.VECTOR;
    public TextMeshProUGUI text_mode;
    public Animations ani;

    public Material mat200N;
    public Material mat300N;
    public Material mat250N;
    public Material mat150N;

    public GameObject cutting_plane_prefab;
    public IsoToggle toggle;
    public GameObject LS;

    public GameObject cutter_holder_prefab;
    public Material m_material;
    public GameObject sphere;
    //private float constant = 8990000000f;    
    private float constant = 8.9875517923f;

    private List<(float,float,float,float)> forcelist = new List<(float, float, float, float)>();
    public int count = 0;
    public enum MODE {
        VECTOR,
        VECTOR_COLOR,
        ISOS,
        CUTTING,
        HYBRID,
        HYBRID2,
        HYBRID3,
        HYBRID4,
        ADV
    }


    public GameObject vector_desc;
    public GameObject iso_desc;
    public GameObject hybrid_desc;
    public GameObject[] controllers;
    public GameObject lr;

    public GameObject iso_info;
    public GameObject v_h_info;
    public GameObject info;
    public GameObject basic;
    // public LRSSender lrs;
    public GameObject process_1;
    public GameObject process_2;
    public GameObject loading_screen;
    bool asyc_task_running;

    List<GameObject> vector_field;
    List<GameObject> isosurfaces;
    List<GameObject> cutting_planes;

    List<GameObject> hybrid_300N;
    List<GameObject> hybrid_250N;
    List<GameObject> hybrid_200N;
    List<GameObject> hybrid_150N;

    List<Wireframe> hybrid_300N_W;
    List<Wireframe> hybrid_250N_W;
    List<Wireframe> hybrid_200N_W;
    List<Wireframe> hybrid_150N_W;
    public bool vis_running;
    bool do_sim = false;

    public void SetMODE(string m)
    {
        switch(m)
        {
            case "ISOS":
                mode = MODE.ISOS;
                // lrs.SendLRS("User","Toggled Mode to","IsoSurfaces",17);
                

                text_mode.text = "ISOS";
                break;
            case "HYBRID":
                mode = MODE.HYBRID;
                // lrs.SendLRS("User","Toggled Mode to","Hybrid",18);
                text_mode.text = "HYBRID1";
                break;
            case "VECTOR":
                mode = MODE.VECTOR;
                // lrs.SendLRS("User","Toggled Mode to","Vector Field",19);
                text_mode.text = "VECTOR";
                break;
            case "HYBRID2":
                mode = MODE.HYBRID2;
                // lrs.SendLRS("User","Toggled Mode to","HYBRID2 Field",20);
                text_mode.text = "HYBRID2";
                break;
            case "HYBRID3":
                mode = MODE.HYBRID3;
                // lrs.SendLRS("User","Toggled Mode to","HYBRID3 Field",21);
                text_mode.text = "HYBRID3";
                break;
            case "HYBRID4":
                mode = MODE.HYBRID4;
                // lrs.SendLRS("User","Toggled Mode to","HYBRID4 Field",22);
                text_mode.text = "HYBRID4";
                break;
            default:
                break;
        }
    }

    public void Update()
    {
        // if (loading_screen.activeSelf && !asyc_task_running)
        // {
        //     loading_screen.SetActive(!loading_screen.activeSelf);
        // }
        if(do_sim) {
            
            Thread thread = new Thread(sim);
            thread.Priority = System.Threading.ThreadPriority.Lowest;
            thread.IsBackground = true;
            thread.Start();
            LS.SetActive(false);
            // Debug.Log("Done Loading!");
            do_sim = false;
        }
    }

    public void run() {
        LS.SetActive(true);
        Debug.Log("Loading...");
        foreach (GameObject item in controllers)
        {
            foreach (Renderer r in item.GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }
        }
        lr.SetActive(false);
        resetter.ResetViz();
        do_sim = true;
    }

    // IEnumerator wait(int sec) {
    //     Debug.Log("Started Coroutine at timestamp : " + Time.time);
    //     yield return new WaitForSeconds(sec);
    //     Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    // }
    public void boxmaton()
    {
        GameObject o = GameObject.Find("Simulation Box");
        o.GetComponent<Renderer>().material = boxmat1;
    }
    public void boxmatoff()
    {
        GameObject o = GameObject.Find("Simulation Box");
        o.GetComponent<Renderer>().material = boxmat2;
    }
    public void sim()
    {        
        // if (ani.playing) {
        //     LS.SetActive(false);
        //     foreach (GameObject item in controllers)
        //     {
        //         foreach (Renderer r in item.GetComponentsInChildren<Renderer>())
        //         {
        //             r.enabled = true;
        //         }
        //     }
        //     lr.SetActive(true);
        //     return;
        // }
        // loading_screen.SetActive(!loading_screen.activeSelf);
        asyc_task_running = true;
        vis_running = true;
        Dispatcher.Instance.Invoke(() => {
        resetter.resetPos2();
        string sample_point_300;
        string distance_vectors_300;
        string force_vectors_300;

        //250N
        string sample_point_250;
        string distance_vectors_250;
        string force_vectors_250;

        //200N
        string sample_point_200;
        string distance_vectors_200;
        string force_vectors_200;

        //150N
        string sample_point_150;
        string distance_vectors_150;
        string force_vectors_150;

        //sim all things;
        vector_field = new List<GameObject>();
        isosurfaces = new List<GameObject>();
        cutting_planes = new List<GameObject>();
        
        hybrid_300N = new List<GameObject>();
        hybrid_250N = new List<GameObject>();
        hybrid_200N = new List<GameObject>();
        hybrid_150N = new List<GameObject>();
        GameObject o = GameObject.Find("Simulation Box");
        float scale = o.transform.localScale.x;
        Particle[] particles  = o.transform.parent.GetComponentsInChildren<Particle>();
        if (particles.Length == 0) {
            Debug.Log("NO PARTICLES DETECTED");
            LS.SetActive(false);
            foreach (GameObject item in controllers)
            {
                foreach (Renderer r in item.GetComponentsInChildren<Renderer>())
                {
                    r.enabled = true;
                }
            }
            lr.SetActive(true);
            return;
        }

        List<Vector3> p_pos = new List<Vector3>();
        List<float> charges = new List<float>();

        foreach (var item in particles)
        {
            float x = item.gameObject.transform.localPosition.x;
            float y = item.gameObject.transform.localPosition.y;
            float z = item.gameObject.transform.localPosition.z;
            Vector3 vv = new Vector3(x,y,z);
            //p_pos.Add(item.gameObject.transform.localPosition * 3/2);
            p_pos.Add(vv);
            charges.Add(item.charge);
            // Debug.Log(vv);
        }

        GameObject marching = GameObject.Find("marching_cubes");
        MarchingRunner solver = marching.GetComponent<MarchingRunner>();
        GameObject cutting_holder1 = GameObject.Instantiate(cutter_holder_prefab);
        cutting_holder1.transform.parent = o.transform;
        cutting_holder1.transform.localPosition = Vector3.zero;
        cutting_holder1.transform.localScale = new Vector3(2f,2f,.01f);
        cutting_holder1.tag = "viz_component";

        float target1 = 300f;
        float target2 = 250f;
        float target3 = 200f;
        float target4 = 150f;
        Vector3 sample1 = Vector3.zero;
        Vector3 sample2 = Vector3.zero;
        Vector3 sample3 = Vector3.zero;
        Vector3 sample4 = Vector3.zero;
        List<Mesh> meshi = solver.RunSim(p_pos, charges, 40, target1);
        Vector3[] pp = p_pos.ToArray();
        float[] cc = charges.ToArray();
        foreach (Mesh m in meshi)
        {
            // GameObject go = GameObject.Instantiate(isoHolder);
            // go.transform.parent = o.transform;
            // go.AddComponent<MeshFilter>();
            // go.AddComponent<MeshRenderer>();
            // go.GetComponent<Renderer>().material = m_material;
            // go.GetComponent<MeshFilter>().mesh = m;
            // go.transform.parent = o.transform;
            // go.transform.localPosition = new Vector3(0f,0f,0f);
            // go.name = go.GetInstanceID().ToString();
            // go.tag = "viz_component";

            GameObject go3 = GameObject.Instantiate(isoHolder);
            go3.transform.parent = o.transform;
            go3.AddComponent<MeshFilter>();
            go3.AddComponent<MeshRenderer>();
            go3.GetComponent<Renderer>().material = mat300N;
            go3.GetComponent<MeshFilter>().mesh = m;
            go3.transform.parent = o.transform;
            go3.transform.localPosition = new Vector3(0f,0f,0f);
            go3.name = go3.GetInstanceID().ToString();
            go3.tag = "viz_component";
            isosurfaces.Add(go3);

            GameObject go2 = new GameObject("Mesh");
            go2.transform.parent = transform;
            go2.AddComponent<MeshFilter>();
            go2.AddComponent<MeshRenderer>();
            go2.GetComponent<Renderer>().material = m_material;
            go2.GetComponent<MeshFilter>().mesh = m;

            Wireframe frame = go2.AddComponent<Wireframe>();
            frame.mat = mat;
            frame.mat2 = mat2;
            go2.transform.parent = o.transform;
            go2.tag = "viz_component";
            go2.transform.localPosition = new Vector3(0f,0f,0f);
            //go.transform.localScale = new Vector3(.05f,.05f,.05f);

            hybrid_300N.Add(go2);
           
            int c1 = 0;
            bool first_iter = true;
            foreach (Vector3 vertex in m.vertices)
            {
                if (first_iter)
                {
                    sample1 = vertex;
                    first_iter = false;
                }
                if (c1 == 8)
                {   
                    GameObject o2 = GameObject.Instantiate(arrow_prefab, Vector3.zero, Quaternion.identity);
                    o2.transform.parent = go2.transform;
                    o2.transform.localScale = new Vector3(.005f,.005f,.01f);
                    o2.transform.localPosition = vertex;
                    o2.name = "arrow: " + o2.GetInstanceID();
                    Arrow arrow = o2.gameObject.GetComponent<Arrow>();
                    // o2.transform.rotation = CalcFieldLine(arrow, o2, particles);
                    var computed_values = CalcAtPosition(o2.transform.localPosition, particles);
                    arrow.pos = o2.transform.localPosition;
                    arrow.force = computed_values.Item2;
                    o2.transform.rotation = computed_values.Item1;
                    o2.tag = "viz_component";
                    c1 = 0;
                    hybrid_300N.Add(o2);
                    o2.SetActive(false);
                }
                else
                {
                    c1++;
                }
            }

            //go.GetComponent<MeshCollider>().sharedMesh = m;

            // go.transform.parent = o.transform;
            // go.transform.localPosition = new Vector3(0f,0f,0f);
            // go.name = go.GetInstanceID().ToString();
            // go.tag = "viz_component";

            // GameObject cutting_plane = GameObject.Instantiate(cutting_plane_prefab);
            // cutting_plane.transform.parent = cutting_holder1.transform;
            // cutting_plane.transform.localScale = new Vector3(1,1,1);
            // cutting_plane.transform.localPosition = Vector3.zero;
            // CutPlane cutplane = cutting_plane.GetComponent<CutPlane>();
            // cutplane.clipObj = go;
            // cutting_plane.tag = "viz_component";

            // cutting_planes.Add(go);
            // cutting_planes.Add(cutting_plane);
            // cutting_plane.SetActive(false);
            // go.SetActive(false);
            go2.SetActive(false);
            go3.SetActive(false);


            // Controller.createIUI("LAYER 4 (INNER)", go.GetInstanceID(), 1, target1, m.vertices);
            //set threshold information
            //sample1 = GetRandomPointOnMesh(m);


        }
        int counter = 0;
        string step1 = sample1.ToString();
        string step2 = "";
        string step3 = "";
        foreach (Particle p in particles) {

            float dist = Vector3.Distance(p.transform.localPosition, sample1);
            float weight =  (constant*Mathf.Pow(p.charge,2))/(Mathf.Pow(dist,2));
            Vector3 vec = p.transform.localPosition - sample1;

            if (counter == 1)
            {
                step2 += "p" + (counter+1) + ": "+ dist;
                step3 += "p" + (counter+1) + ": "+ (-vec * weight);
            }
            if (counter < 3)
            {
                step2 += "\np" + (counter+1) + ": "+ dist + " ";
                step3 += "\np" + (counter+1) + ": "+ (-vec * weight) + " ";
            }
            if (counter == 5)
            {
                step2 += "...";
                step3 += "...";
            }
            counter++;
        }
        sample_point_300 = step1;
        distance_vectors_300 = step2;
        force_vectors_300 = step3;
        meshi = solver.RunSim(p_pos, charges, 40, target2);

        foreach (Mesh m in meshi)
        {
            // GameObject go = GameObject.Instantiate(isoHolder);
            
            // go.transform.parent = o.transform;
            // go.AddComponent<MeshFilter>();
            // go.AddComponent<MeshRenderer>();
            // go.GetComponent<Renderer>().material = m_material;
            // go.GetComponent<MeshFilter>().mesh = m;
            // go.transform.parent = o.transform;
            // go.transform.localPosition = new Vector3(0f,0f,0f);
            // go.name = go.GetInstanceID().ToString();
            // go.tag = "viz_component";

            GameObject go3 = GameObject.Instantiate(isoHolder);
            
            go3.transform.parent = o.transform;
            go3.AddComponent<MeshFilter>();
            go3.AddComponent<MeshRenderer>();
            go3.GetComponent<Renderer>().material = mat250N;
            go3.GetComponent<MeshFilter>().mesh = m;
            go3.transform.parent = o.transform;
            go3.transform.localPosition = new Vector3(0f,0f,0f);
            go3.name = go3.GetInstanceID().ToString();
            go3.tag = "viz_component";
            isosurfaces.Add(go3);
            


            GameObject go2 = new GameObject("Mesh");
            go2.transform.parent = transform;
            go2.AddComponent<MeshFilter>();
            go2.AddComponent<MeshRenderer>();
            go2.GetComponent<Renderer>().material = m_material;
            go2.GetComponent<MeshFilter>().mesh = m;
            Wireframe frame = go2.AddComponent<Wireframe>();
            frame.mat = mat;
            frame.mat2 = mat2;
            go2.transform.parent = o.transform;
            go2.tag = "viz_component";
            go2.transform.localPosition = new Vector3(0f,0f,0f);
            //go.transform.localScale = new Vector3(.05f,.05f,.05f);

            hybrid_250N.Add(go2);
           
            int c2 = 0;
            foreach (Vector3 vertex in m.vertices)
            {
                if (c2 == 8)
                {   
                    GameObject o2 = GameObject.Instantiate(arrow_prefab, Vector3.zero, Quaternion.identity);
                    
                    o2.transform.parent = go2.transform;
                    o2.transform.localScale = new Vector3(.005f,.005f,.01f);
                    o2.transform.localPosition = vertex;
                    o2.name = "arrow: " + o2.GetInstanceID();
                    Arrow arrow = o2.gameObject.GetComponent<Arrow>();
                    // o2.transform.rotation = CalcFieldLine(arrow, o2, particles);
                    var computed_values = CalcAtPosition(o2.transform.localPosition, particles);
                    arrow.pos = o2.transform.localPosition;
                    arrow.force = computed_values.Item2;
                    o2.transform.rotation = computed_values.Item1;
                    o2.tag = "viz_component";
                    c2 = 0;
                    hybrid_250N.Add(o2);
                    o2.SetActive(false);
                }
                else
                {
                    c2++;
                }
            }

            //go.GetComponent<MeshCollider>().sharedMesh = m;
            //add stuff for UI

            // go.transform.parent = o.transform;
            // go.transform.localPosition = new Vector3(0f,0f,0f);
            // go.name = go.GetInstanceID().ToString();
            // go.tag = "viz_component";
            // GameObject cutting_plane = GameObject.Instantiate(cutting_plane_prefab);
            // cutting_plane.transform.parent = cutting_holder1.transform;
            // cutting_plane.transform.localScale = new Vector3(1,1,1);
            // cutting_plane.transform.localPosition = Vector3.zero;
            // CutPlane cutplane = cutting_plane.GetComponent<CutPlane>();
            // cutplane.clipObj = go;
            // cutting_plane.tag = "viz_component";

            // Controller.createIUI("LAYER 3", go.GetInstanceID(), 2, target2, m.vertices);
            // sample2 = GetRandomPointOnMesh(m);
            // cutting_planes.Add(go);
            // cutting_planes.Add(cutting_plane);
            // cutting_plane.SetActive(false);
            // go.SetActive(false);
            go2.SetActive(false);
            go3.SetActive(false);
        }
        counter = 0;
        step1 = sample2.ToString();
        step2 = "";
        step3 = "";
        foreach (Particle p in particles) {

            float dist = Vector3.Distance(p.transform.localPosition, sample2);
            float weight =  (constant*Mathf.Pow(p.charge,2))/(Mathf.Pow(dist,2));
            Vector3 vec = p.transform.localPosition - sample2;

            if (counter == 1)
            {
                step2 += "p" + (counter+1) + ": "+ dist;
                step3 += "p" + (counter+1) + ": "+ (-vec * weight);
            }
            if (counter < 3)
            {
                step2 += "\np" + (counter+1) + ": "+ dist + " ";
                step3 += "\np" + (counter+1) + ": "+ (-vec * weight) + " ";
            }
            if (counter == 5)
            {
                step2 += "...";
                step3 += "...";
            }
            counter++;
        }
        sample_point_250 = step1;
        distance_vectors_250 = step2;
        force_vectors_250 = step3;
        meshi = solver.RunSim(p_pos, charges, 40, target3);

        foreach (Mesh m in meshi)
        {
            // GameObject go = GameObject.Instantiate(isoHolder);
            // go.transform.parent = o.transform;
            // go.AddComponent<MeshFilter>();
            // go.AddComponent<MeshRenderer>();
            // go.GetComponent<Renderer>().material = m_material;
            // go.GetComponent<MeshFilter>().mesh = m;
            // go.transform.parent = o.transform;
            // go.transform.localPosition = new Vector3(0f,0f,0f);
            // go.name = go.GetInstanceID().ToString();
            // go.tag = "viz_component";

            GameObject go3 = GameObject.Instantiate(isoHolder);
            
            go3.transform.parent = o.transform;
            go3.AddComponent<MeshFilter>();
            go3.AddComponent<MeshRenderer>();
            go3.GetComponent<Renderer>().material = mat200N;
            go3.GetComponent<MeshFilter>().mesh = m;
            go3.transform.parent = o.transform;
            go3.transform.localPosition = new Vector3(0f,0f,0f);
            go3.name = go3.GetInstanceID().ToString();
            go3.tag = "viz_component";
            isosurfaces.Add(go3);
            //go.GetComponent<MeshCollider>().sharedMesh = m;
            //add stuff for UI
            GameObject go2 = new GameObject("Mesh");
            go2.transform.parent = transform;
            go2.AddComponent<MeshFilter>();
            go2.AddComponent<MeshRenderer>();
            go2.GetComponent<Renderer>().material = m_material;
            go2.GetComponent<MeshFilter>().mesh = m;
            Wireframe frame = go2.AddComponent<Wireframe>();
            frame.mat = mat;
            frame.mat2 = mat2;
            go2.transform.parent = o.transform;
            go2.tag = "viz_component";
            go2.transform.localPosition = new Vector3(0f,0f,0f);
            //go.transform.localScale = new Vector3(.05f,.05f,.05f);

            hybrid_200N.Add(go2);
           
            int c3 = 0;
            foreach (Vector3 vertex in m.vertices)
            {
                if (c3 == 8)
                {   
                    GameObject o2 = GameObject.Instantiate(arrow_prefab, Vector3.zero, Quaternion.identity);
                   
                    o2.transform.parent = go2.transform;
                    o2.transform.localScale = new Vector3(.005f,.005f,.01f);
                    o2.transform.localPosition = vertex;
                    o2.name = "arrow: " + o2.GetInstanceID();
                    Arrow arrow = o2.gameObject.GetComponent<Arrow>();
                    // o2.transform.rotation = CalcFieldLine(arrow, o2, particles);
                    var computed_values = CalcAtPosition(o2.transform.localPosition, particles);
                    arrow.pos = o2.transform.localPosition;
                    arrow.force = computed_values.Item2;
                    o2.transform.rotation = computed_values.Item1;
                    o2.tag = "viz_component";
                    c3 = 0;
                    hybrid_200N.Add(o2);
                    o2.SetActive(false);
                }
                else
                {
                    c3++;
                }
            }
    
            // GameObject cutting_plane = GameObject.Instantiate(cutting_plane_prefab);
            // cutting_plane.transform.parent = cutting_holder1.transform;
            // cutting_plane.transform.localScale = new Vector3(1,1,1);
            // cutting_plane.transform.localPosition = Vector3.zero;
            // CutPlane cutplane = cutting_plane.GetComponent<CutPlane>();
            // cutplane.clipObj = go;
            // cutting_plane.tag = "viz_component";

            // Controller.createIUI("LAYER 2", go.GetInstanceID(), 3, target3, m.vertices);
            // sample3 = GetRandomPointOnMesh(m);
            // cutting_planes.Add(go);
            // cutting_planes.Add(cutting_plane);
            // cutting_plane.SetActive(false);
            // go.SetActive(false);
            go2.SetActive(false);
            go3.SetActive(false);
        }
        counter = 0;
        step1 = sample3.ToString();
        step2 = "";
        step3 = "";
        foreach (Particle p in particles) {

            float dist = Vector3.Distance(p.transform.localPosition, sample3);
            float weight =  (constant*Mathf.Pow(p.charge,2))/(Mathf.Pow(dist,2));
            Vector3 vec = p.transform.localPosition - sample3;

            if (counter == 1)
            {
                step2 += "p" + (counter+1) + ": "+ dist;
                step3 += "p" + (counter+1) + ": "+ (-vec * weight);
            }
            if (counter < 3)
            {
                step2 += "\np" + (counter+1) + ": "+ dist + " ";
                step3 += "\np" + (counter+1) + ": "+ (-vec * weight) + " ";
            }
            if (counter == 5)
            {
                step2 += "...";
                step3 += "...";
            }
            counter++;
        }
        sample_point_200 = step1;
        distance_vectors_200 = step2;
        force_vectors_200 = step3;
        meshi = solver.RunSim(p_pos, charges, 40, target4);

        foreach (Mesh m in meshi)
        {
            // GameObject go = GameObject.Instantiate(isoHolder);
            // go.transform.parent = o.transform;
            // go.AddComponent<MeshFilter>();
            // go.AddComponent<MeshRenderer>();
            // go.GetComponent<Renderer>().material = m_material;
            // go.GetComponent<MeshFilter>().mesh = m;
            // go.transform.parent = o.transform;
            // go.transform.localPosition = new Vector3(0f,0f,0f);
            // go.name = go.GetInstanceID().ToString();
            // go.tag = "viz_component";
            //add stuff for UI

            GameObject go3 = GameObject.Instantiate(isoHolder);
            go3.transform.parent = o.transform;
            go3.AddComponent<MeshFilter>();
            go3.AddComponent<MeshRenderer>();
            go3.GetComponent<Renderer>().material = mat150N;
            go3.GetComponent<MeshFilter>().mesh = m;
            go3.transform.parent = o.transform;
            go3.transform.localPosition = new Vector3(0f,0f,0f);
            go3.name = go3.GetInstanceID().ToString();
            go3.tag = "viz_component";
            isosurfaces.Add(go3);

            GameObject go2 = new GameObject("Mesh");
            go2.transform.parent = transform;
            go2.AddComponent<MeshFilter>();
            go2.AddComponent<MeshRenderer>();
            go2.GetComponent<Renderer>().material = m_material;
            go2.GetComponent<MeshFilter>().mesh = m;
            Wireframe frame = go2.AddComponent<Wireframe>();
            frame.mat = mat;
            frame.mat2 = mat2;
            go2.transform.parent = o.transform;
            go2.tag = "viz_component";
            go2.transform.localPosition = new Vector3(0f,0f,0f);
            //go.transform.localScale = new Vector3(.05f,.05f,.05f);

            hybrid_150N.Add(go2);
           
            int c4 = 0;
            foreach (Vector3 vertex in m.vertices)
            {
                if (c4 == 8)
                {   
                    GameObject o2 = GameObject.Instantiate(arrow_prefab, Vector3.zero, Quaternion.identity);
                    
                    o2.transform.parent = go2.transform;
                    o2.transform.localScale = new Vector3(.005f,.005f,.01f);
                    o2.transform.localPosition = vertex;
                    o2.name = "arrow: " + o2.GetInstanceID();
                    Arrow arrow = o2.gameObject.GetComponent<Arrow>();
                    // o2.transform.rotation = CalcFieldLine(arrow, o2, particles);
                    var computed_values = CalcAtPosition(o2.transform.localPosition, particles);
                    arrow.pos = o2.transform.localPosition;
                    arrow.force = computed_values.Item2;
                    o2.transform.rotation = computed_values.Item1;
                    o2.tag = "viz_component";
                    c4 = 0;
                    hybrid_150N.Add(o2);
                    o2.SetActive(false);
                }
                else
                {
                    c4++;
                }
            }

            
            // GameObject cutting_plane = GameObject.Instantiate(cutting_plane_prefab);
            // cutting_plane.transform.parent = cutting_holder1.transform;
            // cutting_plane.transform.localScale = new Vector3(1,1,1);
            // cutting_plane.transform.localPosition = Vector3.zero;
            // CutPlane cutplane = cutting_plane.GetComponent<CutPlane>();
            // cutplane.clipObj = go;
            // cutting_plane.tag = "viz_component";

            // Controller.createIUI("LAYER 1 (OUTER)", go.GetInstanceID(), 4, target4, m.vertices);
            // sample4 = GetRandomPointOnMesh(m);
            // cutting_planes.Add(go);
            // cutting_planes.Add(cutting_plane);
            // cutting_plane.SetActive(false);
            // go.SetActive(false);
            go2.SetActive(false);
            go3.SetActive(false);
        }
        counter = 0;
        step1 = sample4.ToString();
        step2 = "";
        step3 = "";
        foreach (Particle p in particles) {

            float dist = Vector3.Distance(p.transform.localPosition, sample4);
            float weight =  (constant*Mathf.Pow(p.charge,2))/(Mathf.Pow(dist,2));
            Vector3 vec = p.transform.localPosition - sample4;

            if (counter == 1)
            {
                step2 += "p" + (counter+1) + ": "+ dist;
                step3 += "p" + (counter+1) + ": "+ (-vec * weight);
            }
            if (counter < 3)
            {
                step2 += "\np" + (counter+1) + ": "+ dist + " ";
                step3 += "\np" + (counter+1) + ": "+ (-vec * weight) + " ";
            }
            if (counter == 5)
            {
                step2 += "...";
                step3 += "...";
            }
            counter++;
        }
        sample_point_150 = step1;
        distance_vectors_150 = step2;
        force_vectors_150 = step3;
        cutting_holder1.transform.rotation = Quaternion.Euler(-4.049f,-102.752f,2.863f);
        cutting_holder1.transform.localPosition = new Vector3(-1.01538f, 0.1452855f, 0.2419235f);
        cutting_planes.Add(cutting_holder1);
        cutting_holder1.SetActive(false);

        toggle.dump = new IsoInfoDump(sample_point_300, distance_vectors_300, force_vectors_300,
                        sample_point_250, distance_vectors_250, force_vectors_250,
                        sample_point_200, distance_vectors_200, force_vectors_200,
                        sample_point_150, distance_vectors_150, force_vectors_150);

        List<Arrow> gos = new List<Arrow>();
        for (float i = -scale/2 + 0.025f; i < scale/2 - 0.025f; i += scale*.1f) {
            for (float j = -scale/2 + 0.025f; j < scale/2 - 0.025f; j+= scale*.1f) {
                for (float k = -scale/2 + 0.025f; k < scale/2 - 0.025f; k += scale*.1f)
                {
                    GameObject o2 = GameObject.Instantiate(arrow_prefab, Vector3.zero, Quaternion.identity);
                    
                    o2.transform.parent = gameObject.transform;
                    o2.transform.localScale = new Vector3(.01f,.01f,.01f);
                    o2.transform.localPosition = new Vector3(i, j, k);
                    o2.name = "arrow: " + o2.GetInstanceID();
                    Arrow arrow = o2.gameObject.GetComponent<Arrow>();
                    // o2.transform.rotation = CalcFieldLine(arrow, o2, particles);
                    var computed_values = CalcAtPosition(o2.transform.localPosition, particles);
                    var force = computed_values.Item2;
                
                    // bound force so that the range isn't so large the heat map is useless
                    if (force > 400f)
                        force = 400f;
                    
                    arrow.pos = o2.transform.localPosition;
                    arrow.force = computed_values.Item2;
                    arrow.force_dir = computed_values.Item3;
                    arrow.nparticles = particles.Length;
                    o2.transform.rotation = computed_values.Item1;
                    o2.tag = "viz_component";
                    gos.Add(arrow);
                    vector_field.Add(o2);
                    o2.SetActive(false);
                }
            }
        }

        var min = 10000f;
        var max = 0f;

        foreach (Arrow arrow in gos)
        {
            var force = arrow.force > 400 ? 400 : arrow.force;
            force = force/25;
            if (force < min)
            {
                min = force;
            }
            else if (force > max)
            {
                max = force;
            }
        }
        foreach (Arrow arrow in gos)
        {
            var force = arrow.force > 400 ? 400 : arrow.force;
            force = force/25;
            var hue_mod = ((force - min) / (max-min));
            var color = new Color((0 + hue_mod), 150/255, (1 - hue_mod));
            // Debug.Log(color);
            arrow.simple.color = color;
            arrow.color = color;
        }

        //now show what is what
        switch(mode)
        {
            case MODE.VECTOR_COLOR:
                // if (SM.scene == 4)
                //     SM.incVector();
                info.SetActive(true);
                basic.SetActive(false);
                vector_desc.SetActive(true);
                iso_desc.SetActive(false);
                hybrid_desc.SetActive(false);
                iso_info.SetActive(false);
                v_h_info.SetActive(true);
                process_2.SetActive(false);
                process_1.SetActive(true);
                foreach (GameObject obj in vector_field)
                {
                    obj.SetActive(true);
                    Arrow arr = obj.GetComponent<Arrow>();
                    if (arr)
                        arr.goHeat(true);
                }
                break;
            case MODE.VECTOR:
                // if (SM.scene == 4)
                //     SM.incVector();
                info.SetActive(true);
                basic.SetActive(false);
                vector_desc.SetActive(true);
                iso_desc.SetActive(false);
                hybrid_desc.SetActive(false);
                iso_info.SetActive(false);
                v_h_info.SetActive(true);
                process_2.SetActive(false);
                process_1.SetActive(true);
                foreach (GameObject obj in vector_field)
                {
                    obj.SetActive(true);
                    Arrow arr = obj.GetComponent<Arrow>();
                    if (arr)
                        arr.goHeat(false);
                }
                break;
            case MODE.ISOS:
                // if (SM.scene == 4)
                //     SM.incIso();
                info.SetActive(true);
                basic.SetActive(false);
                vector_desc.SetActive(false);
                iso_desc.SetActive(true);
                hybrid_desc.SetActive(false);
                iso_info.SetActive(true);
                v_h_info.SetActive(false);
                process_2.SetActive(true);
                process_1.SetActive(false);
                foreach (GameObject obj in isosurfaces)
                {
                    obj.SetActive(true);
                }
                break;
            case MODE.CUTTING:
                // if (SM.scene == 4)
                //     SM.incIso();
                info.SetActive(true);
                basic.SetActive(false);
                vector_desc.SetActive(false);
                iso_desc.SetActive(true);
                hybrid_desc.SetActive(false);
                iso_info.SetActive(true);
                v_h_info.SetActive(false);
                process_2.SetActive(true);
                process_1.SetActive(false);
                foreach(GameObject obj in isosurfaces)
                {
                    obj.SetActive(true);
                }
                cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                cutting_plane_prefab.SetActive(true);
                break;
            case MODE.HYBRID:
                // if (SM.scene == 4)
                //     SM.incHybrid();
                info.SetActive(true);
                basic.SetActive(false);
                vector_desc.SetActive(false);
                iso_desc.SetActive(false);
                hybrid_desc.SetActive(true);
                iso_info.SetActive(false);
                v_h_info.SetActive(true);
                process_2.SetActive(false);
                process_1.SetActive(true);
                foreach (GameObject obj in hybrid_300N)
                {
                    obj.SetActive(true);
                }
                break;
            case MODE.HYBRID2:
                // if (SM.scene == 4)
                //     SM.incHybrid();
                info.SetActive(true);
                basic.SetActive(false);
                vector_desc.SetActive(false);
                iso_desc.SetActive(false);
                hybrid_desc.SetActive(true);
                iso_info.SetActive(false);
                v_h_info.SetActive(true);
                process_2.SetActive(false);
                process_1.SetActive(true);
                foreach (GameObject obj in hybrid_250N)
                {
                    obj.SetActive(true);
                }
                break;
            case MODE.HYBRID3:
                // if (SM.scene == 4)
                //     SM.incHybrid();
                info.SetActive(true);
                basic.SetActive(false);
                vector_desc.SetActive(false);
                iso_desc.SetActive(false);
                hybrid_desc.SetActive(true);
                iso_info.SetActive(false);
                v_h_info.SetActive(true);
                process_2.SetActive(false);
                process_1.SetActive(true);
                foreach (GameObject obj in hybrid_200N)
                {
                    obj.SetActive(true);
                }
                break;
            case MODE.HYBRID4:
                // if (SM.scene == 4)
                //     SM.incHybrid();
                info.SetActive(true);
                basic.SetActive(false);
                vector_desc.SetActive(false);
                iso_desc.SetActive(false);
                hybrid_desc.SetActive(true);
                iso_info.SetActive(false);
                v_h_info.SetActive(true);
                process_2.SetActive(false);
                process_1.SetActive(true);
                foreach (GameObject obj in hybrid_150N)
                {
                    obj.SetActive(true);
                }
                break;
            default:
                break;
        }
        asyc_task_running = false;
        LS.SetActive(false);
        foreach (GameObject item in controllers)
        {
            foreach (Renderer r in item.GetComponentsInChildren<Renderer>())
            {
                r.enabled = true;
            }
        }
        lr.SetActive(true);
        boxmatoff();
        foreach (GameObject item in rerender_objs)
        {
            item.SetActive(false);
            item.SetActive(true);
        }
        });
    }

    public void NewToggle(string mm)
    {
        Debug.Log("Changing mode to: " + mm);
        if (ani.playing)
            return;
        switch(mm)
        {
            case "VECTOR":
                mode = MODE.VECTOR;
                if (vis_running)
                {
                    // if (SM.scene == 4)
                    //     SM.incVector();
                    info.SetActive(true);
                    basic.SetActive(false);
                    vector_desc.SetActive(true);
                    iso_desc.SetActive(false);
                    hybrid_desc.SetActive(false);
                    iso_info.SetActive(false);
                    v_h_info.SetActive(true);
                    process_2.SetActive(false);
                    process_1.SetActive(true);
                    foreach (GameObject obj in vector_field)
                    {
                        obj.SetActive(true);
                        Arrow arr = obj.GetComponent<Arrow>();
                        if (arr)
                            arr.goHeat(false);
                    }
                    foreach (GameObject obj in isosurfaces)
                    {
                        obj.SetActive(false);
                    }
                    cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                    cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                    cutting_plane_prefab.SetActive(false);
                    foreach (GameObject obj in hybrid_300N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_250N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_200N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_150N)
                    {
                        obj.SetActive(false);
                    }
                }
                // lrs.SendLRS("User","Toggled Mode to","IsoSurfaces",17);
                break;
            case "CUTTING":
                mode = MODE.CUTTING;
                if (vis_running)
                {
                    // if (SM.scene == 4)
                    //     SM.incIso();
                    info.SetActive(true);
                    basic.SetActive(false);
                    vector_desc.SetActive(false);
                    iso_desc.SetActive(true);
                    hybrid_desc.SetActive(false);
                    iso_info.SetActive(true);
                    v_h_info.SetActive(false);
                    process_2.SetActive(true);
                    process_1.SetActive(false);
                    foreach (GameObject obj in vector_field)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in isosurfaces)
                    {
                        obj.SetActive(true);
                    }
                    cutting_plane_prefab.SetActive(true);
                    cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                    cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                    foreach (GameObject obj in hybrid_300N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_250N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_200N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_150N)
                    {
                        obj.SetActive(false);
                    }
                }
                // lrs.SendLRS("User","Toggled Mode to","IsoSurfaces",17);
                break;
            case "ISOS":
                mode = MODE.ISOS;
                if (vis_running)
                {
                    // if (SM.scene == 4)
                    //     SM.incIso();
                    info.SetActive(true);
                    basic.SetActive(false);
                    vector_desc.SetActive(false);
                    iso_desc.SetActive(true);
                    hybrid_desc.SetActive(false);
                    iso_info.SetActive(true);
                    v_h_info.SetActive(false);
                    process_2.SetActive(true);
                    process_1.SetActive(false);
                    foreach (GameObject obj in vector_field)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in isosurfaces)
                    {
                        obj.SetActive(true);
                    }
                    cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                    cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                    cutting_plane_prefab.SetActive(false);
                    foreach (GameObject obj in hybrid_300N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_250N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_200N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_150N)
                    {
                        obj.SetActive(false);
                    }
                }
                // lrs.SendLRS("User","Toggled Mode to","Hybrid",18);
                break;
            case "HYBRID":
                mode = MODE.HYBRID;
                if (vis_running)
                {
                    // if (SM.scene == 4)
                    //     SM.incHybrid();
                    info.SetActive(true);
                    basic.SetActive(false);
                    vector_desc.SetActive(false);
                    iso_desc.SetActive(false);
                    hybrid_desc.SetActive(true);
                    iso_info.SetActive(false);
                    v_h_info.SetActive(true);
                    process_2.SetActive(false);
                    process_1.SetActive(true);
                    foreach (GameObject obj in vector_field)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in isosurfaces)
                    {
                        obj.SetActive(false);
                    }
                    cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                    cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                    cutting_plane_prefab.SetActive(false);
                    foreach (GameObject obj in hybrid_300N)
                    {
                        obj.SetActive(true);
                    }
                    foreach (GameObject obj in hybrid_250N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_200N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_150N)
                    {
                        obj.SetActive(false);
                    }
                }
                // lrs.SendLRS("User","Toggled Mode to","Hybrid",18);
                break;
            case "HYBRID2":
                mode = MODE.HYBRID2;
                if (vis_running)
                {
                    // if (SM.scene == 4)
                    //     SM.incHybrid();
                    info.SetActive(true);
                    basic.SetActive(false);
                    vector_desc.SetActive(false);
                    iso_desc.SetActive(false);
                    hybrid_desc.SetActive(true);
                    iso_info.SetActive(false);
                    v_h_info.SetActive(true);
                    process_2.SetActive(false);
                    process_1.SetActive(true);
                    foreach (GameObject obj in vector_field)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in isosurfaces)
                    {
                        obj.SetActive(false);
                    }
                    cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                    cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                    cutting_plane_prefab.SetActive(false);
                    foreach (GameObject obj in hybrid_300N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_250N)
                    {
                        obj.SetActive(true);
                    }
                    foreach (GameObject obj in hybrid_200N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_150N)
                    {
                        obj.SetActive(false);
                    }
                }
                // lrs.SendLRS("User","Toggled Mode to","Vector Field",19);
                break;
            case "HYBRID3":
                mode = MODE.HYBRID3;
                if (vis_running)
                {
                    // if (SM.scene == 4)
                    //     SM.incHybrid();
                    info.SetActive(true);
                    basic.SetActive(false);
                    vector_desc.SetActive(false);
                    iso_desc.SetActive(false);
                    hybrid_desc.SetActive(true);
                    iso_info.SetActive(false);
                    v_h_info.SetActive(true);
                    process_2.SetActive(false);
                    process_1.SetActive(true);
                    foreach (GameObject obj in vector_field)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in isosurfaces)
                    {
                        obj.SetActive(false);
                    }
                    cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                    cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                    cutting_plane_prefab.SetActive(false);
                    foreach (GameObject obj in hybrid_300N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_250N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_200N)
                    {
                        obj.SetActive(true);
                    }
                    foreach (GameObject obj in hybrid_150N)
                    {
                        obj.SetActive(false);
                    }
                }
                // lrs.SendLRS("User","Toggled Mode to","HYBRID2 Field",20);
                break;
            case "HYBRID4":
                mode = MODE.HYBRID4;
                if (vis_running)
                {
                    // if (SM.scene == 4)
                    //     SM.incHybrid();
                    info.SetActive(true);
                    basic.SetActive(false);
                    vector_desc.SetActive(false);
                    iso_desc.SetActive(false);
                    hybrid_desc.SetActive(true);
                    iso_info.SetActive(false);
                    v_h_info.SetActive(true);
                    process_2.SetActive(false);
                    process_1.SetActive(true);
                    foreach (GameObject obj in vector_field)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in isosurfaces)
                    {
                        obj.SetActive(false);
                    }
                    cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                    cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                    cutting_plane_prefab.SetActive(false);
                    foreach (GameObject obj in hybrid_300N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_250N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_200N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_150N)
                    {
                        obj.SetActive(true);
                    }
                }
                // lrs.SendLRS("User","Toggled Mode to","HYBRID3 Field",21);
                break;
            case "VECTOR_COLORED":
                mode = MODE.VECTOR_COLOR;
                if (vis_running)
                {
                    // Desbug.Log("ERERERERERERERERERERERER")
                    // if (SM.scene == 4)
                    //     SM.incVector();
                    info.SetActive(true);
                    basic.SetActive(false);
                    vector_desc.SetActive(true);
                    iso_desc.SetActive(false);
                    hybrid_desc.SetActive(false);
                    iso_info.SetActive(false);
                    v_h_info.SetActive(true);
                    process_2.SetActive(false);
                    process_1.SetActive(true);
                    foreach (GameObject obj in vector_field)
                    {
                        obj.SetActive(true);
                        Arrow arr = obj.GetComponent<Arrow>();
                        if (arr)
                            arr.goHeat(true);
                    }
                    foreach (GameObject obj in isosurfaces)
                    {
                        obj.SetActive(false);
                    }
                    cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                    cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
                    cutting_plane_prefab.SetActive(false);
                    foreach (GameObject obj in hybrid_300N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_250N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_200N)
                    {
                        obj.SetActive(false);
                    }
                    foreach (GameObject obj in hybrid_150N)
                    {
                        obj.SetActive(false);
                    }
                }
                // lrs.SendLRS("User","Toggled Mode to","HYBRID4 Field",22);
                break;
            default:
                break;
        }

        Debug.Log("Mode is now: ");
        Debug.Log(mode);
        foreach (GameObject item in rerender_objs)
        {
            item.SetActive(false);
            item.SetActive(true);
        }
    }
    public void ClearViz()
    {
        if (vector_field != null) {
            if (vector_field.Count < 1) {
                foreach (GameObject obj in vector_field)
                {
                    Destroy(obj);
                }
            }
        }
        if (isosurfaces != null) {
            if (isosurfaces.Count < 1) {
                foreach (GameObject obj in isosurfaces)
                {
                    Destroy(obj);
                }
            }
        }
        if(cutting_plane_prefab != null) {
            if(cutting_plane_prefab) {
                cutting_plane_prefab.SetActive(false);
                cutting_plane_prefab.transform.localPosition = new Vector3(-0.5f,0f,-0.5f);
                cutting_plane_prefab.transform.eulerAngles = new Vector3(0f,0f,0f);
            }
        }
        if(hybrid_300N != null) {
            if (hybrid_300N.Count < 1) {
                foreach (GameObject obj in hybrid_300N)
                {
                    Destroy(obj);
                }
            }
        }
        if(hybrid_250N != null) {
            if (hybrid_250N.Count < 1) {
                foreach (GameObject obj in hybrid_250N)
                {
                    Destroy(obj);
                }
            }
        }
        if(hybrid_200N != null) {
            if (hybrid_200N.Count < 1) {
                foreach (GameObject obj in hybrid_200N)
                {
                    Destroy(obj);
                }
            }
        }
        if(hybrid_150N != null) {
            if (hybrid_150N.Count < 1) {
                foreach (GameObject obj in hybrid_150N)
                {
                    Destroy(obj);
                }
            }
        }
    }

    (Quaternion, float, Vector3) CalcAtPosition(Vector3 pos, Particle[] particles)
    {
        List<Vector3> init_vectors = new List<Vector3>();

        foreach (Particle p in particles) {

            float dist = Vector3.Distance(p.transform.localPosition, pos);
            float weight =  (constant*(p.charge*1.6f))/(Mathf.Pow(dist,2));
            Vector3 vec = p.transform.localPosition - pos;

            init_vectors.Add(-vec * weight);
        }
        //add all vectors together
        //normalize vectors
        Vector3 final_vec = new Vector3(0,0,0);
        foreach (Vector3 vec in init_vectors)
        {
            final_vec += vec;
        }
        return (Quaternion.LookRotation(final_vec, Vector3.up), final_vec.magnitude, final_vec);
    }

    Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        //if you're repeatedly doing this on a single mesh, you'll likely want to cache cumulativeSizes and total
        float[] sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        float[] cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }

        //so everything above this point wants to be factored out

        float randomsample = UnityEngine.Random.value* total;

        int triIndex = -1;
        
        for (int i = 0; i < sizes.Length; i++)
        {
            if (randomsample <= cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }

        if (triIndex == -1) Debug.LogError("triIndex should never be -1");

        Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
        Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
        Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];

        //generate random barycentric coordinates

        float r = UnityEngine.Random.value;
        float s = UnityEngine.Random.value;

        if(r + s >=1)
        {
            r = 1 - r;
            s = 1 - s;
        }
        //and then turn them back to a Vector3
        Vector3 pointOnMesh = a + r*(b - a) + s*(c - a);
        return pointOnMesh;

    }

    float[] GetTriSizes(int[] tris, Vector3[] verts)
    {
        int triCount = tris.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {
            sizes[i] = .5f*Vector3.Cross(verts[tris[i*3 + 1]] - verts[tris[i*3]], verts[tris[i*3 + 2]] - verts[tris[i*3]]).magnitude;
        }
        return sizes;
    }
}
