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

    public Texture2D image;

    WebCamTexture cameraTexture;
    bool isCamAvailable;
    int outputWidth;
    int outputHeight;



    void Start()
    {
        outputWidth = output.width;
        outputHeight = output.height;

        switch (imageSourceType)
        {
            case ImageSourceType.WebCam:
                {
                    WebCamInit();
                    break;
                }
            case ImageSourceType.Image:
                {
                    ImageInit();
                    break;
                }
        }
    }

    private void Update()
    {

        switch (imageSourceType)
        {
            case ImageSourceType.WebCam:
                {
                    WebcamUpdate();
                    break;
                }
            case ImageSourceType.Image:
                {
                    break;
                }
        }
    }

    private void ImageInit()
    {
        float factor = outputWidth / outputHeight;

        if (image.height * factor < image.width)
        {
            Graphics.Blit(TextureConverter.Texture2DCenterCrop(image, (int)(image.height * factor), image.height), output);
        }
        else
        {
            Graphics.Blit(TextureConverter.Texture2DCenterCrop(image, image.width, (int)(image.width / factor)), output);
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
        cameraTexture = new WebCamTexture(device.name, 320  , 320);

        if (cameraTexture == null)
        {
            Debug.Log("No camera detected");
            return;
        }

        cameraTexture.Play();
        float h = cameraTexture.height;
        float w = cameraTexture.width;
        Debug.Log("The camera pixel is: " + w);
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

