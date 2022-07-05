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
    public override List<BoundingBox> RunInference(IWorker worker, RenderTexture source, float threshold)
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

    private List<BoundingBox> PostProcess(Tensor input, float threshold)
    {

        var output = new List<BoundingBox>();
        var inputShape = input.shape;
        for (int i = 0; i < input.shape[7]; i++)
        {

            var confidence = input[0, 0, 4, i];
            if (confidence < threshold)
            {
                continue;
            }

            var builder = new ModelBuilder();
            int[] starts = { 0, 0, 5, i };
            int[] ends = { 0, 0, 85, i };
            int[] strides = { 1, 1, 1, 1 };
            builder.StridedSlice("outputslice", "original_output", starts, ends, strides);

            var worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, builder.model);

            worker.Execute(input);

            var classConfidence = worker.PeekOutput();


            output.Add(new BoundingBox(
                _x: input[0, 0, 0, i],
                _y: input[0, 0, 1, i],
                _width: input[0, 0, 2, i],
                _height: input[0, 0, 3, i],
                _confidence: input[0, 0, 4, i],
                _classIndex: classConfidence.ArgMax()[0]
                ));

        }


        return output;
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





