using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using System.IO;


public class InferenceScriptYoloV5sGPU : InferenceScript
{
    int[] inputShape;

    int inputWidth;
    int inputHeight;

    int kernelIndex;

    ComputeShader postProcessStage1;
    ComputeShader postProcessStage2;

    (ComputeBuffer workerOuputFloatArray,

    ComputeBuffer postProcessStage1_OutputBuffer,

    ComputeBuffer postProcessStage1_CountRead) buffer;

    private DetectionCache readCache;

    public override void InitInference(ResourceSet resourceSet, float threshold)
    {
        base.InitInference(resourceSet, threshold);
        inputShape = model.inputs[0].shape;
        inputWidth = inputShape[5];
        inputHeight = inputShape[6];

        AllocateObjects();

        postProcessStage1 = resourceSet.postProcessStage1;
        kernelIndex = postProcessStage1.FindKernel("CSMain");
        postProcessStage1.SetInt("classCount", 4);
        postProcessStage1.SetFloat("threshold", threshold);
        postProcessStage1.SetBuffer(kernelIndex, "Output", buffer.postProcessStage1_OutputBuffer);

        postProcessStage1.SetBuffer(kernelIndex, "Input", buffer.workerOuputFloatArray);

    }
    public override BoundingBox[] RunInference(RenderTexture source)
    {

        var input = PreProcess(source);

        worker.Execute(input);

        var workerOutput = worker.PeekOutput("output").ToReadOnlyArray();
        buffer.workerOuputFloatArray.SetData(workerOutput);

        postProcessStage1.Dispatch(kernelIndex, 63, 1, 1);

        ComputeBuffer.CopyCount(buffer.postProcessStage1_OutputBuffer, buffer.postProcessStage1_CountRead, 0);
        buffer.postProcessStage1_OutputBuffer.SetCounterValue(0);

        readCache.Invalidate();
        input.Dispose();
        //return DataProcess.NMS(output.ToArray());
        return DataProcess.NMS(readCache.Cached);

    }


    public override Tensor PreProcess(RenderTexture source)
    {
        if (source.width == inputWidth && source.height == inputHeight)
        {
            return TextureConverter.ToTensor(source);
        }
        else
        {
            TextureConverter.Texture2DToPNG(TextureConverter.RenderTextureToTexture2D(source));

            return TextureConverter.ToTensor(source);
        }


    }

    private void AllocateObjects()
    {
        buffer.workerOuputFloatArray = new ComputeBuffer(6300 * 9, sizeof(float));
        buffer.postProcessStage1_OutputBuffer = new ComputeBuffer(256, BoundingBox.Size, ComputeBufferType.Append);
        buffer.postProcessStage1_CountRead = new ComputeBuffer(1, sizeof(uint), ComputeBufferType.Raw);

        readCache = new DetectionCache(buffer.postProcessStage1_OutputBuffer, buffer.postProcessStage1_CountRead);
    }

    public override void DisposeObjects()
    {
        base.DisposeObjects();
        buffer.workerOuputFloatArray?.Dispose();
        buffer.postProcessStage1_OutputBuffer?.Dispose();
        buffer.postProcessStage1_CountRead?.Dispose();
    }


    public override BoundingBox[] PostProcess(Tensor input, float threshold)
    {
        throw new System.NotImplementedException();
    }
}







class DetectionCache
{
    private ComputeBuffer dataBuffer;

    private ComputeBuffer countBuffer;

    private BoundingBox[] cached;

    private int[] countRead = new int[1];

    public BoundingBox[] Cached => cached ?? UpdateCache();

    public DetectionCache(ComputeBuffer data, ComputeBuffer count)
    {
        dataBuffer = data;
        countBuffer = count;
    }

    public void Invalidate()
    {
        cached = null;
    }

    public BoundingBox[] UpdateCache()
    {
        countBuffer.GetData(countRead, 0, 0, 1);
        int num = countRead[0];
        cached = new BoundingBox[num];
        dataBuffer.GetData(cached, 0, 0, num);
        return cached;
    }
}
