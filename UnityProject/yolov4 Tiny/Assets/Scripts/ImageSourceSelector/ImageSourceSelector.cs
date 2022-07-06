using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;


public enum ImageSourceType { WebCam, Image }

public sealed class ImageSourceSelector : MonoBehaviour
{

    public RawImage rawImage;
    public RenderTexture output;
    public ImageSourceType imageSourceType;

    WebCamTexture cameraTexture;
    bool isCamAvailable;
    void Start()
    {
         if(imageSourceType == ImageSourceType.WebCam)
        {
            WebCamInit();
        }
    }

    private void Update()
    {
        if (imageSourceType == ImageSourceType.WebCam)
        {
            WebcamUpdate();
        }
    }

    private void WebCamInit()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            var isCamAvailable = false;
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
        cameraTexture = new WebCamTexture(device.name, 640, 480);

        if (cameraTexture == null)
        {
            Debug.Log("No camera detected");
            return;
        }

        cameraTexture.Play();
        float h = cameraTexture.height;
        float w = cameraTexture.width;
        Debug.Log("The camera pixel is: " + w );
        Debug.Log("The camera pixel is: " + h);
        rawImage.texture = cameraTexture;

        isCamAvailable = true;
    }


    private void WebcamUpdate()
    {
        if (isCamAvailable)
        {
            Graphics.Blit(cameraTexture, output);
        }

        //float ratio = (float)cameraTexture.width / (float)cameraTexture.height;

        //fit.aspectRatio = ratio;

        //float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f;

        //rawImage.rectTransform.localScale = new Vector3(1f, scaleY, 1f);


        //int orient = -cameraTexture.videoRotationAngle;
        //rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

    }
}

