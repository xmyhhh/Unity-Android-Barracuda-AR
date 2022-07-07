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


        outputName = model.outputs;
    }
    public override BoundingBox[] RunInference(IWorker worker, RenderTexture source, float threshold)
    {

        var input = PreProcess(source);

        worker.Execute(input);

        input.Dispose();
        if (outputName.Count > 1)
        {
            Tensor[] output = new Tensor[outputName.Count];
            for (int i = 0; i < outputName.Count; i++)
            {
                output[i] = worker.PeekOutput(outputName[i]);
            }
            return PostProcessByLayer(output, threshold);
        }
        else
        {
            var output = worker.PeekOutput();
            return PostProcess(output, threshold);
        }

    }


    private Tensor PreProcess(RenderTexture source)
    {
        TextureConverter.Texture2DToPNG(TextureConverter.RenderTextureToTexture2D(source));

        return TextureConverter.ToTensor(source);
    }

    private BoundingBox[] PostProcess(Tensor input, float threshold)
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
                _classIndex: DataProcess.MaxValueIndex(classConfidence.AsFloats())
                ));

        }
        return DataProcess.NMS(output.ToArray());
    }

    private BoundingBox[] PostProcessByLayer(Tensor[] inputs, float threshold)
    {

        var output = new List<BoundingBox>();
        var anchorBoxArray = new List<float[]>();
        foreach (var input in inputs)
        {
            var boxNumH = input.shape[5];
            var boxNumW = input.shape[6];
            var boxNumC = input.shape[7];
            for (int i = 0; i < boxNumH; i++)
            {
                for (int j = 0; j < boxNumW; j++)
                {
                    for (var k = 0; k < boxNumC; k++)
                    {
                        float[] anchorBox = new float[9];
                        for (int l = 0; l < 9; l++)
                        {
                            anchorBox[l] = input[0, 0, 0, 0, i, j, l, k];
                        }
                        anchorBoxArray.Add(anchorBox);
                    }
                }
            }
        }


        return DataProcess.NMS(output.ToArray());
    }

    private void xywh2xyxy(int[] x)
    {
        var y = new int[4];
        y[0] = x[0] - x[2] / 2;  // top left x
        y[1] = x[1] - x[3] / 2;  // top left y
        y[2] = x[0] + x[2] / 2;  // bottom right x
        y[3] = x[1] + x[3] / 2;  // bottom right y

    }


}








