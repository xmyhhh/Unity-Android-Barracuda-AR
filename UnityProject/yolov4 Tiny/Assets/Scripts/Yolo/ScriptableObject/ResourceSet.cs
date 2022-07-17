using UnityEngine;
using Unity.Barracuda;




public abstract class ResourceSet : ScriptableObject
{
    public NNModel model;
    public float[] anchors = new float[12];

    public ComputeShader postProcessStage1;
    public ComputeShader postProcessStage2;
    public abstract InferenceScript InferenceCPU { get; }

    public abstract InferenceScript InferenceGPU { get; }

    public abstract string[] Label { get; }

}


