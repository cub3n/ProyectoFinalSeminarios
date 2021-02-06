using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using UnityEditor;
using UnityEngine.Networking;
using Unity.Barracuda;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

public class FileManager : MonoBehaviour
{
    string path;
    public RawImage image;
    private WebCamTexture wCam;
    public GameObject detector;

   
    public void OpenExplorerPC()
    {
        if (wCam != null)
        {
            this.wCam.Stop();
        }
        this.detector.GetComponent<scrDetector>().mode = 0;
        path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png,jpg");
        GetImage();
    }
    
    

    public void GetImage()
    {
        if (path != null)
        {
            UpdateImage();
        }
    }
    void UpdateImage()
    {
        this.detector.GetComponent<scrDetector>().mode = 1;
        this.detector.GetComponent<scrDetector>().boxFactory.GetComponent<scrBoxFactory>().DestroyBoxes();
        WWW www = new WWW("file:///" + path);
        this.image.texture = www.texture;
    }

    public void StartWebCamera()
    {
        this.detector.GetComponent<scrDetector>().mode = 2;
        this.detector.GetComponent<scrDetector>().boxFactory.GetComponent<scrBoxFactory>().DestroyBoxes();
        this.wCam = new WebCamTexture();
        this.image.texture = this.wCam;
        this.wCam.Play();
    }

    public Texture2D StopWebCamera()
    {
        
        Texture2D texture = new Texture2D(this.wCam.width, this.wCam.height);
        texture.SetPixels(this.wCam.GetPixels());
        texture.Apply();
        this.image.texture = texture;
        return rotateTexture(texture, true);
    }

    public void Update()
    {
        if (this.wCam != null)
        {
            if (this.wCam.isPlaying)
            {
                Vector3 rotationVector = new Vector3(0f, 0f, 0f);
                rotationVector.z = -this.wCam.videoRotationAngle;
                image.rectTransform.localEulerAngles = rotationVector;
            } else
            {
                Vector3 rotationVector = new Vector3(0f, 0f, 0f);
                rotationVector.z = this.wCam.videoRotationAngle;
                image.rectTransform.localEulerAngles = rotationVector;
            }
           
        }
        
    }

    public void OpenExplorer(int maxSize)
	{
        this.detector.GetComponent<scrDetector>().mode = 1;

		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{
				// Create Texture from selected image
				Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                //Texture2D texture = new Texture2D(aux.width, aux.height);
                //texture.SetPixels(aux.GetPixels());
                //texture.Apply();
                if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}
                // Assign texture to a temporary quad and destroy it after 5 seconds
                this.image.texture = texture;
                this.detector.GetComponent<scrDetector>().boxFactory.GetComponent<scrBoxFactory>().DestroyBoxes();

            }
        }, "Select a PNG image", "image/png");

		Debug.Log("Permission result: " + permission);
	}

    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }
}
