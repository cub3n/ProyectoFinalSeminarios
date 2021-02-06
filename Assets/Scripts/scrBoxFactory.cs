using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrBoxFactory : MonoBehaviour
{
    private List<GameObject> boxes;
    public GameObject container;
    public GameObject boxModel;
    private float sizex;
    private float sizey;


    public void Start()
    {
        boxes = new List<GameObject>();
        
    }
    public void CreateBox(BoundingBox box, string name, float realWidth, float realHeight)
    {
        sizex = container.GetComponentInParent<RectTransform>().rect.width;
        sizey = container.GetComponentInParent<RectTransform>().rect.height;


        var newBox = GameObject.Instantiate(boxModel);
        newBox.name = name;
        newBox.transform.SetParent(container.transform);
        boxes.Add(newBox);
        newBox.transform.Find("BoxName").GetComponent<Text>().text = name;
        
        var properties = newBox.GetComponent<RectTransform>();
        Debug.Log(box.width * 265 + " Width " + box.width);
        Debug.Log(box.height * 211 + " Height " + box.height);
        Debug.Log(box.left / 1.5f + " left " + box.left);
        Debug.Log(box.top / 1.5f + " top " + box.top);

        properties.sizeDelta = new Vector2((float)box.width * sizex/4.5f, (float)box.height * sizey/2.5f);
        properties.anchoredPosition = new Vector3((float) box.left * sizex + (float) box.width * sizex / 2 , - (float)box.top * sizex - (float)box.height * sizex / 2 , 0);

    }

    public void DestroyBoxes()
    {
        foreach(GameObject box in boxes)
        {
            Destroy(box);
        }
    }

    private static float Logistic(float x)
    {
        if (x > 0)
        {
            return (float)(1 / (1 + Math.Exp(-x)));
        }
        else
        {
            var e = Math.Exp(x);
            return (float)(e / (1 + e));
        }
    }
}
