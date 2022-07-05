using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;


[CreateAssetMenu(fileName = "YoloV5s Inference CPU",
                 menuName = "ScriptableObjects/Inference/YoloV5s Inference CPU")]
public class InferenceScriptYoloV5sCPU : InferenceScript
{
    int[] shape;

    int width;
    int height;

    bool hasDetect = false;

    Tensor output;
    public override void InitInference(Model model)
    {
        shape = model.inputs[0].shape;
        width = shape[5];
        height = shape[6];
    }
    public override List<BoundingBoxDimensions> RunInference(IWorker worker, RenderTexture source, float threshold)
    {
        Debug.Log("ScriptableObjects/YoloV5s Inference CPU");
        var input = PreProcess(source);

        worker.Execute(input);

        var output = worker.PeekOutput();

        input.Dispose();

        return PostProcess(output, threshold);

    }


    private Tensor PreProcess(RenderTexture source)
    {
        return TextureConverter.RenderTextureToTensor(source);


    }

    private List<BoundingBoxDimensions> PostProcess(Tensor input, float threshold)
    {

        var inputShape = input.shape;
        for (int i = 0; i < input.shape[7]; i++)
        {

            var confidence = input[0, 0, 4, i];
            if (confidence < threshold)
            {
                continue;
            }
            Debug.Log(confidence.ToString());
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
        //return y;
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





