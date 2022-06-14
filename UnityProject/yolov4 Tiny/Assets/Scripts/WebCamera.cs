using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class WebCamera : MonoBehaviour
{

    bool isCamAvailable;
    WebCamTexture cameraTexture;
    Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    public WebCamTexture CameraTexture=> cameraTexture;
    public bool IsCamAvailable=> isCamAvailable;
    private void Start()
    {
        defaultBackground = background.texture;

        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            isCamAvailable = false;
            return;
        }

        //foreach (WebCamDevice device in devices)
        //{
        //    if (!device.isFrontFacing)
        //    {
        //        cameraTexture = new WebCamTexture(device.name, Screen.width, Screen.height); ;
        //    }
        //}


        WebCamDevice device = devices[0];
        cameraTexture = new WebCamTexture(device.name, Screen.width, Screen.height); 

        if (cameraTexture == null)
        {
            Debug.Log("No camera detected");
            return;
        }

        cameraTexture.Play();
        background.texture = cameraTexture;

        isCamAvailable = true;
    }

    private void Update()
    {
        if (!isCamAvailable)
        {
            return;
        }

        float ratio = (float)cameraTexture.width / (float)cameraTexture.height;

        fit.aspectRatio = ratio;

        float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f;

        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);


        int orient = -cameraTexture.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}

