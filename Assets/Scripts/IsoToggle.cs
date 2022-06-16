using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IsoToggle : MonoBehaviour
{

    public IsoInfoDump dump;
    public TextMeshProUGUI sample;
    public TextMeshProUGUI distance_vectors;
    public TextMeshProUGUI force_vectors;

    public void show_300N()
    {
        sample.text = dump.sample_point_300;
        distance_vectors.text = dump.distance_vectors_300;
        force_vectors.text = dump.force_vectors_300;
    }

    public void show_250N()
    {
        sample.text = dump.sample_point_250;
        distance_vectors.text = dump.distance_vectors_250;
        force_vectors.text = dump.force_vectors_250;
    }

    public void show_200N()
    {
        sample.text = dump.sample_point_200;
        distance_vectors.text = dump.distance_vectors_200;
        force_vectors.text = dump.force_vectors_200; 
    }

    public void show_150N()
    {
        sample.text = dump.sample_point_150;
        distance_vectors.text = dump.distance_vectors_150;
        force_vectors.text = dump.force_vectors_150; 
    }
}
