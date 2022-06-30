using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;


[CreateAssetMenu(fileName = "YoloV5s Inference GPU",
                 menuName = "ScriptableObjects/Inference/YoloV5s Inference GPU")]
public class InferenceScriptYoloV5sGPU: InferenceScript
{
    public override void RunInference(NNModel model, Texture source, float threshold)
    {
        Debug.Log("ScriptableObjects/YoloV5s Inference GPU");
    }
}
