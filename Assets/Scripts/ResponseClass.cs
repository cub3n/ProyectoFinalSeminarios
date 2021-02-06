using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class AnalysisObject
{
    public List<Prediction> Predictions { get; set; }
}

[Serializable]
public class Prediction
{
    public string TagName { get; set; }
    public double Probability { get; set; }
}

[Serializable]
public class BoundingBox
{
    public double left { get; set; }
    public double top { get; set; }
    public double width { get; set; }
    public double height { get; set; }
}
