using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pivot : MonoBehaviour
{

    public TextMeshProUGUI particle_locations;
    public TextMeshProUGUI distance_vectors;
    public TextMeshProUGUI force_vectors;
    public TextMeshProUGUI final_force;

    public TextMeshProUGUI GetParticleLocation()
    {
        return particle_locations;
    }

    public TextMeshProUGUI GetDistanceVectors()
    {
        return distance_vectors;
    }

    public TextMeshProUGUI GetForceVectors()
    {
        return force_vectors;
    }

    public TextMeshProUGUI GetFinalForce()
    {
        return final_force;
    }
}
