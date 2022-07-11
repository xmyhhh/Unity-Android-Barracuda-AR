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

    List<string> outputName;

    Tensor output;
    public override void InitInference(Model model)
    {
        shape = model.inputs[0].shape;
        width = shape[5];
        height = shape[6];

    }
    public override BoundingBox[] RunInference(IWorker worker, RenderTexture source, float threshold)
    {

        var input = PreProcess(source);

        worker.Execute(input);

        input.Dispose();

        var output = worker.PeekOutput();
        return PostProcess(output, threshold);

    }


    public override Tensor PreProcess(RenderTexture source)
    {
        if (source.width == width && source.height == height)
        {
            return TextureConverter.ToTensor(source);
        }
        else
        {
            TextureConverter.Texture2DToPNG(TextureConverter.RenderTextureToTexture2D(source));

            return TextureConverter.ToTensor(source);
        }

    }

    public override BoundingBox[] PostProcess(Tensor input, float threshold)
    {

        var output = new List<BoundingBox>();

        for (int i = 0; i < input.shape[7]; i++)
        {

            var confidence = input[0, 0, 4, i];
            if (confidence < threshold)
            {
                continue;
            }

            var builder = new ModelBuilder();
            int[] starts = { 0, 0, 5, i };
            int[] ends = { 0, 0, 9, i };
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
                _classIndex: DataProcess.MaxValueIndex(classConfidence.AsFloats())
                ));

        }
        return DataProcess.NMS(output.ToArray());
    }


}








