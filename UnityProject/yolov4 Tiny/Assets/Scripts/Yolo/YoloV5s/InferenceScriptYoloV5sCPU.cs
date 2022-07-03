using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;


[CreateAssetMenu(fileName = "YoloV5s Inference CPU",
                 menuName = "ScriptableObjects/Inference/YoloV5s Inference CPU")]
public class InferenceScriptYoloV5sCPU : InferenceScript
{
    int[] shape;
    Texture2D texture2D;
    int width;
    int height;

    Tensor output;
    public override void InitInference(ResourceSet resourceSet)
    {
        base.InitInference(resourceSet);
        worker = model.CreateWorker(WorkerFactory.Device.CPU);

        shape = model.inputs[0].shape;
        width = shape[5];
        height = shape[6];
    }
    public override List<BoundingBoxDimensions> RunInference(RenderTexture source, float threshold)
    {
        Debug.Log("ScriptableObjects/YoloV5s Inference CPU");

        worker.Execute(PreProcess(source));

        return PostProcess(worker.PeekOutput(), threshold);
    }


    private Tensor PreProcess(RenderTexture source)
    {
        return TextureConverter.RenderTextureToTensor(source, width, height);
    }

    private List<BoundingBoxDimensions> PostProcess(Tensor input, float threshold)
    {
        var inputShape = input.shape;


        var Res = new List<BoundingBoxDimensions>();
        for (int i = 0; i < input.shape[7]; i++)
        {
            var confidence = input[0, 0, 4, i];
            if (confidence < threshold)
            {
                continue;
            }
            //int[] starts = {  0, 0, 0, i };
            //int[] ends = {  0, 0, 85, i };
            //var classConf = input.StridedSlice(starts, ends).AsFloats();

            var classConf = new List<float>();
            for (int j = 0; j <85; j++)
                classConf.Add(input[0, 0, j, i]);

        }

        return null;
    }



    private void xywh2xyxy(int[] x)
    {
        var y = new int[4];
        y[0] = x[0] - x[2] / 2;  // top left x
        y[1] = x[1] - x[3] / 2;  // top left y
        y[2] = x[0] + x[2] / 2;  // bottom right x
        y[3] = x[1] + x[3] / 2;  // bottom right y
        return y;
    }

    //public void OnDisable()
    //{
    //    worker.Dispose();
    //}
}


//Res.Add(new BoundingBoxDimensions
//{
//    X = output[0, 0, 0, 0, 0, 0, i, 0],
//    Y = output[0, 0, 0, 0, 0, 0, i, 1],
//    Width = output[0, 0, 0, 0, 0, 0, i, 2],
//    Height = output[0, 0, 0, 0, 0, 0, i, 3]

//});





