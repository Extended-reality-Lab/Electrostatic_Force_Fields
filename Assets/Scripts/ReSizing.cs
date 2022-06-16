using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSizing : MonoBehaviour
{

    public GameObject simArea;
    public Animations ani;
    // Start is called before the first frame update

    public void scaleUp()
    {
        if (ani.playing)
            return;
        float x = simArea.transform.localScale.x + .1f;
        float y = simArea.transform.localScale.y + .1f;
        float z = simArea.transform.localScale.z + .1f;
        Debug.Log(x);
        simArea.transform.localScale = new Vector3(x > 2f ? 2f : x, y > 2f ? 2f : y, z > 2f ? 2f : z);
    }

    public void scaleDown()
    {
        if (ani.playing)
            return;
        float x = simArea.transform.localScale.x - .1f;
        float y = simArea.transform.localScale.y - .1f;
        float z = simArea.transform.localScale.z - .1f;
        simArea.transform.localScale = new Vector3(x < .5f ? .5f : x, y < .5f ? .5f : y, z < .5f ? .5f : z);
    }
}
