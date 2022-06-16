using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{

    public float maxLength;
    public GameObject cursorvis;

    public Material linemat1;

    public Material linemat2;
    public float charge;

    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    // private void Update() {
    //     Vector3 pos = gameObject.transform.localPosition;
    //     if (pos.x % 0.025f == 0)
    //     {
    //         if (pos.y % 0.025f == 0)
    //         {
    //             if (pos.z % 0.025f == 0)
    //             {
    //                 return;
    //             }
    //         }
    //     }
    //     float flox = 0.025f * Mathf.Floor(pos.x / 0.025f);
    //     float ceilx = 0.025f * Mathf.Ceil(pos.x / 0.025f);
    //     float floy = 0.025f * Mathf.Floor(pos.y / 0.025f);
    //     float ceily = 0.025f * Mathf.Ceil(pos.y / 0.025f);
    //     float floz = 0.025f * Mathf.Floor(pos.z / 0.025f);
    //     float ceilz = 0.025f * Mathf.Ceil(pos.z / 0.025f);
    //     float nx = Mathf.Abs(flox-pos.x) < Mathf.Abs(ceilx-pos.x) ? flox : ceilx;
    //     float ny = Mathf.Abs(floy-pos.y) < Mathf.Abs(ceilx-pos.y) ? floy : ceily;
    //     float nz = Mathf.Abs(floz-pos.z) < Mathf.Abs(ceilz-pos.z) ? floz : ceilz;
       

    //     gameObject.transform.localPosition = new Vector3(nx,ny,nz);

    

    // }


    public int Initialize(int c = 0)
    {
        int ccc = 0;
        if (c == 0) {


            ccc = Random.Range(-1, 2);
            while (ccc == 0)
            {
                ccc = Random.Range(-1, 2);
            }

        }
        else {

            ccc = c;

        }

        Renderer rndr = gameObject.GetComponent<Renderer>();
        if (ccc < 0)
        {
            charge = -1.6f;
            rndr.material = linemat1;
        }
        else {
            charge = 1.6f;
            rndr.material = linemat2;
        }

        return (int)charge;
    }

    public void mod_material()
    {
        Renderer rndr = gameObject.GetComponent<Renderer>();
        if (charge < 0) {
            rndr.material = linemat1;
        }
        else {
            rndr.material = linemat2;
        }
    }
}
