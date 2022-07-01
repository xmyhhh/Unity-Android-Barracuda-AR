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
    List<string> outputNames;
    List<Tensor> outputs;
    public override void InitInference(ResourceSet resourceSet)
    {
        base.InitInference(resourceSet);
        worker = model.CreateWorker(WorkerFactory.Device.CPU);

        shape = model.inputs[0].shape;
        width = shape[6];
        height = shape[6];

        outputNames = model.outputs;
        outputs = new List<Tensor>();
    }
    public override void RunInference(RenderTexture source, float threshold)
    {
        outputs.Clear();
        worker.Execute(TextureConverter.RenderTextureToTensor(source, width, height));
        foreach (var outputName in outputNames){
            outputs.Add(worker.PeekOutput(outputName));
        }




        Debug.Log("ScriptableObjects/YoloV5s Inference CPU");
    }



}
