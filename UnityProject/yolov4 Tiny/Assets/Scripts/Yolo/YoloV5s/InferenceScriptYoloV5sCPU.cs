using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;


[CreateAssetMenu(fileName = "YoloV5s Inference CPU",
                 menuName = "ScriptableObjects/Inference/YoloV5s Inference CPU")]
public class InferenceScriptYoloV5sCPU : InferenceScript
{
    public override void RunInference(NNModel model, Texture source, float threshold)
    {
        Debug.Log("ScriptableObjects/YoloV5s Inference CPU");
    }





    private const int IMAGE_MEAN = 0;
    private const float IMAGE_STD = 255.0F;
    private static Tensor TransformInput(Color32[] pic, int width, int height, int requestedWidth)
    {
        float[] floatValues = new float[width * height * 3];

        int beginning = (((pic.Length / requestedWidth) - height) * requestedWidth) / 2;
        int leftOffset = (requestedWidth - width) / 2;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var color = pic[beginning + leftOffset + j];

                floatValues[(i * width + j) * 3 + 0] = (color.r - IMAGE_MEAN) / IMAGE_STD;
                floatValues[(i * width + j) * 3 + 1] = (color.g - IMAGE_MEAN) / IMAGE_STD;
                floatValues[(i * width + j) * 3 + 2] = (color.b - IMAGE_MEAN) / IMAGE_STD;
            }
            beginning += requestedWidth;
        }

        return new Tensor(1, height, width, 3, floatValues);
    }
}
