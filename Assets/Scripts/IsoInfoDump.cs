using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoInfoDump
{
    //300N
    public string sample_point_300;
    public string distance_vectors_300;
    public string force_vectors_300;

    //250N
    public string sample_point_250;
    public string distance_vectors_250;
    public string force_vectors_250;

    //200N
    public string sample_point_200;
    public string distance_vectors_200;
    public string force_vectors_200;

    //150N
    public string sample_point_150;
    public string distance_vectors_150;
    public string force_vectors_150;

    public IsoInfoDump(string sample_point_300, string distance_vectors_300, string force_vectors_300,
                        string sample_point_250, string distance_vectors_250, string force_vectors_250,
                        string sample_point_200, string distance_vectors_200, string force_vectors_200,
                        string sample_point_150, string distance_vectors_150, string force_vectors_150)
    {
        this.sample_point_300 = sample_point_300;
        this.distance_vectors_300 = distance_vectors_300;
        this.force_vectors_300 = force_vectors_300;

        this.sample_point_250 = sample_point_250;
        this.distance_vectors_250 = distance_vectors_250;
        this.force_vectors_250 = force_vectors_250;

        this.sample_point_200 = sample_point_200;
        this.distance_vectors_200 = distance_vectors_200;
        this.force_vectors_200 = force_vectors_200;

        this.sample_point_150 = sample_point_150;
        this.distance_vectors_150 = distance_vectors_150;
        this.force_vectors_150 = force_vectors_150;


    }
}
