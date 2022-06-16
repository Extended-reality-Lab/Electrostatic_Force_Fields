using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnParticle : MonoBehaviour
{
    public Particle particlePrefab;

    public UIController Controller;
    public Animations ani;
    public LRSSender lrs;
    public int num_p;

    public string spawnParticleWithOptions(Vector3 pos, int value)
    {
        Debug.Log("Spawning Particle");
        // add randomized charges here.
        GameObject box = GameObject.Find("Simulation Box");
        var x = box.transform.localScale.x;
        var y = box.transform.localScale.y;
        var z = box.transform.localScale.z;

        Particle new_particle = Object.Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        new_particle.name = new_particle.GetInstanceID().ToString();
        GameObject bucket = GameObject.Find("Simulation Area");
        new_particle.transform.parent = bucket.transform;
        new_particle.tag = "particle_component";
        //ector3 position = new Vector3(0,0,0);
        new_particle.transform.localPosition = new Vector3(pos.x*x, pos.y*y, pos.z*z);
        int charge = new_particle.Initialize(value);
        GameObject canvas = GameObject.Find("test_canvas");

        UIController controller = canvas.GetComponent<UIController>();
        controller.createPUI((charge > 0 ? false: true), new_particle.GetInstanceID());
        //new_particle.Initialize();
        Debug.Log("Particle Spawned");
        return new_particle.GetInstanceID().ToString();
    }
    public void spawnParticle()
    {
        // if (ani.playing)
        //     return;
        // add randomized charges here.
        GameObject canvas = GameObject.Find("particle_canvas");
        UIController controller = canvas.GetComponent<UIController>();
        if (controller.num_particles == 5)
            return;
        GameObject box = GameObject.Find("Simulation Box");
        var x = box.transform.localScale.x;
        var y = box.transform.localScale.y;
        var z = box.transform.localScale.z;


        Vector3 position = new Vector3(Random.Range(-0.5f*x, 0.5f*x), Random.Range(-0.5f*y, 0.5f*y), Random.Range(-0.5f*y, 0.5f*y));
        Particle new_particle = Object.Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        new_particle.name = new_particle.GetInstanceID().ToString();
        GameObject bucket = GameObject.Find("Simulation Area");
        new_particle.transform.parent = bucket.transform;
        new_particle.tag = "particle_component";
        //ector3 position = new Vector3(0,0,0);
        new_particle.transform.localPosition = position;
        int charge = new_particle.Initialize();


        controller.createPUI((charge > 0 ? false: true), new_particle.GetInstanceID());
        // lrs.SendLRS("User","Spawned ","Particle"+new_particle.GetInstanceID().ToString(),3);

        //new_particle.Initialize();
        Debug.Log("Particle Spawned");
    }

    public void spawnDemoParticle(int intie)
    {
        // add randomized charges here.
        GameObject canvas = GameObject.Find("particle_canvas");

        UIController controller = canvas.GetComponent<UIController>();
        if (controller.num_particles == 5)
            return;
        GameObject box = GameObject.Find("Simulation Box");
        var x = box.transform.localScale.x;
        var y = box.transform.localScale.y;
        var z = box.transform.localScale.z;
        Vector3 position = Vector3.zero;
        if (intie == 0)
            position = new Vector3(0f,0f,0f);
        else
            position = new Vector3(Random.Range(-0.5f*x, 0.5f*x), Random.Range(-0.5f*y, 0.5f*y), Random.Range(-0.5f*y, 0.5f*y));
        Particle new_particle = Object.Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        new_particle.name = new_particle.GetInstanceID().ToString();
        GameObject bucket = GameObject.Find("Simulation Area");
        new_particle.transform.parent = bucket.transform;
        new_particle.tag = "particle_component";
        //ector3 position = new Vector3(0,0,0);
        new_particle.transform.localPosition = position;
        int charge = new_particle.Initialize(1);



        // make UI component

        controller.createPUI((charge > 0 ? false: true), new_particle.GetInstanceID());
        lrs.SendLRS("User","Spawned ","Particle"+new_particle.GetInstanceID().ToString(),3);
        //new_particle.Initialize();
        Debug.Log("Particle Spawned");
    }

     public void spawnParticle2()
    {
        Debug.Log("Spawning Particles");
        // add randomized charges here.
        GameObject box = GameObject.Find("Simulation Box");
        var x = box.transform.localScale.x;
        var y = box.transform.localScale.y;
        var z = box.transform.localScale.z;

        GameObject bucket = GameObject.Find("Simulation Area");

        Vector3 position = new Vector3(0.1190365f,-0.1527148f,0.06519084f);
        Particle new_particle = Object.Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        new_particle.name = new_particle.GetInstanceID().ToString();
        new_particle.transform.parent = bucket.transform;
        //ector3 position = new Vector3(0,0,0);
        new_particle.transform.localPosition = position;
        int charge = new_particle.Initialize(1);

        // Vector3 position2 = new Vector3(Random.Range(-0.5f*x, 0.5f*x), Random.Range(-0.5f*y, 0.5f*y), Random.Range(-0.5f*y, 0.5f*y));
        Vector3 position2 = new Vector3(-0.1022345f,0.1029936f,0.2089079f);
        Particle new_particle2 = Object.Instantiate(particlePrefab, Vector3.zero, Quaternion.identity);
        new_particle2.name = new_particle2.GetInstanceID().ToString();
        new_particle2.transform.parent = bucket.transform;
        //ector3 position = new Vector3(0,0,0);
        new_particle2.transform.localPosition = position2;
        int charge2 = new_particle2.Initialize(-1);

        // System.IO.File.WriteAllText(@"C:\Unity Projects\educational_tool\particles.csv", "Particle,x,y,z\nParticle 1," + position.x + "," +position.y + "," +position.z + "\nParticle 2," + position2.x + "," + position2.y + "," + position2.z + "\n");



        // make UI component
        GameObject canvas = GameObject.Find("test_canvas");

        UIController controller = canvas.GetComponent<UIController>();
        controller.createPUI((charge > 0 ? false: true), new_particle.GetInstanceID());

        //new_particle.Initialize();
        Debug.Log("Particle Spawned");
    }

    public void moveParticle(string name, Vector3 pos) {
        GameObject o = GameObject.Find(name);
        GameObject box = GameObject.Find("Simulation Box");
        var x = box.transform.localScale.x;
        var y = box.transform.localScale.y;
        var z = box.transform.localScale.z;
        o.transform.localPosition = new Vector3(pos.x*x,pos.y*y,pos.z*z);
    }
}
