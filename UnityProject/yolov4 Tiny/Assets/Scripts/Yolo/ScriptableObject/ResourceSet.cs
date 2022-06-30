using UnityEngine;
using Unity.Barracuda;



[CreateAssetMenu(fileName = "YoloDetector",
                 menuName = "ScriptableObjects/Yolo Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public NNModel model;
    public float[] anchors = new float[12];

    public InferenceScript InferenceCPU;

    public InferenceScript InferenceGPU;

}


