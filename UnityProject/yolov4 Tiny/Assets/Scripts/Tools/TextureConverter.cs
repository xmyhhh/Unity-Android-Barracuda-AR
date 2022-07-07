using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;

public static class TextureConverter
{
    public static Texture2D RenderTextureToTexture2D(RenderTexture rTex)
    {
        //这个方法已经测试过应该没问题
        Texture2D dest = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        dest.Apply(false);

        RenderTexture.active = rTex;
        dest.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0, false);

        return dest;
    }

    public static RenderTexture Texture2DToRenderTexture(Texture2D texRef)
    {
        //这个方法已经测试过应该没问题
        // texRef is your Texture2D
        // You can also reduice your texture 2D that way
        RenderTexture rt = new RenderTexture(texRef.width, texRef.height, 0);
        RenderTexture.active = rt;
        // Copy your texture ref to the render texture
        Graphics.Blit(texRef, rt);


        return rt;
    }



    public static Tensor ToTensor(RenderTexture pic)
    {
        return new Tensor(pic, 3);
    }

    public static Tensor ToTensor(Texture pic)
    {
        return new Tensor(pic, 3);
    }

    public static Tensor ToTensor(Texture2D pic)
    {
        return new Tensor(pic, 3);
    }

    public static Texture2D Texture2DResize(Texture2D texture2D, int targetX, int targetY)
    {

        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(targetX, targetY);
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        return result;

    }


    public static void Texture2DToPNG(Texture2D texture2D)
    {
        //这个方法已经测试过应该没问题
        byte[] bytes = texture2D.EncodeToPNG();
        var dirPath = Application.dataPath + "/../SaveImages/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + "Imagexmy" + ".png", bytes);
    }



    public static Texture2D RenderTextureCenterCrop(RenderTexture input, int width, int height)
    {
        return Texture2DCenterCrop(RenderTextureToTexture2D(input), width, height);
    }

    public static Texture2D Texture2DCenterCrop(Texture2D input, int width, int height)
    {

        var inputWidth = input.width;
        var inputHeight = input.height;

        var centerX = inputWidth / 2;
        var centerY = inputHeight / 2;

        if (inputWidth < width || inputHeight < height)
        {
            Debug.Log("Texture2DCenterCrop Error");
            return null;
        }

        var cropWidth = (int)(centerX + width / 2) - (int)(centerX - width / 2);
        var cropHeight = (int)(centerY + height / 2) - (int)(centerY - height / 2);
        Color[] c = input.GetPixels((int)(centerX - width / 2), (int)(centerY - height / 2), cropWidth, cropHeight);
        Texture2D output = new Texture2D(cropWidth, cropHeight, input.format, false);
        output.SetPixels(c);
        output.Apply();
        return output;
    }






    /// Pass a GPU RenderTexture to CPU Texture2D
    /// This opperation has heavy cost (ReadPixels)
    /// We reduice the RenderTexture size first to be more efficient

    static void RenderTextureToTexture2D()
    {

        // Please, be power of 2 for performance issue (2, 4, 8, ...)
        int downSize = 2;

        RenderTexture textureToDownsizeAndCopyToCPU = new RenderTexture(1920, 1080, 0);
        Texture2D newTexture2DToBeLoadedTo = new Texture2D(textureToDownsizeAndCopyToCPU.width / downSize, textureToDownsizeAndCopyToCPU.height / downSize, TextureFormat.ARGB32, false);

        // Load textureToDownsizeAndCopyToCPU from something you like...

        // Create a temporary downsized texture
        RenderTexture textureDownsized = RenderTexture.GetTemporary(textureToDownsizeAndCopyToCPU.width / downSize, textureToDownsizeAndCopyToCPU.height / downSize);

        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        RenderTexture.active = textureDownsized;

        // Copy textureToDownsizeAndCopyToCPU to textureDownsized
        Graphics.Blit(textureToDownsizeAndCopyToCPU, textureDownsized);

        // Read the RenderTexture image into it
        // Pass GPU bytes to CPU
        // Very performance consuming
        newTexture2DToBeLoadedTo.ReadPixels(new Rect(0, 0, textureDownsized.width, textureDownsized.height), 0, 0, false);
        // Debug.Log(tex2D.width + " " + tex2D.height);
        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;

        RenderTexture.ReleaseTemporary(textureDownsized);
    }




}

