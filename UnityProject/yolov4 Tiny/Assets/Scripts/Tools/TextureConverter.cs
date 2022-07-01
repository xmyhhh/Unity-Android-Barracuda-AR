using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;

public static class TextureConverter
{
   public static Texture2D RenderTextureToTexture2D(RenderTexture rTex)
    {
        Texture2D dest = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        dest.Apply(false);
        Graphics.CopyTexture(rTex, dest);
        return dest;
    }


    public static Tensor Texture2DToTensor(Texture2D texture2D)
    {
        return new Tensor(texture2D, 3);
    }


    public static Tensor RenderTextureToTensor(RenderTexture pic, int width, int height)
    {

        Texture2D texture2D = RenderTextureToTexture2D(pic);

        texture2D.Reinitialize(width, height);

        return Texture2DToTensor(texture2D);

    }

}

