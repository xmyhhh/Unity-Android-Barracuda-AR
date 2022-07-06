using UnityEngine;
using Unity.Barracuda;



[CreateAssetMenu(fileName = "YoloDetector",
                 menuName = "ScriptableObjects/Yolo Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public NNModel model;
    public float[] anchors = new float[12];

    public InferenceScript inferenceCPU;

    public InferenceScript inferenceGPU;

    public ClassNameType classNameType;

    public string[] Label
    {
        get
        {
            switch (classNameType)
            {
                case ClassNameType.COCO80:
                    return ClassName.COCO80;
                    break;
                default:
                    return ClassName.YoloV4Tiny;
                    break;
            }
        }
    }
}


