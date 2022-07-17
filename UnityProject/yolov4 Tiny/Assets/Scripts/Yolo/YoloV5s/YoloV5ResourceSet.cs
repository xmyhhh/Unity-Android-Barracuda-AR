using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;

[CreateAssetMenu(fileName = "YoloV5 Detector",
                 menuName = "ScriptableObjects/YoloV5 Resource Set")]
class YoloV5ResourceSet : ResourceSet
{
    public override InferenceScript InferenceCPU => new InferenceScriptYoloV5sCPU();

    public override InferenceScript InferenceGPU => new InferenceScriptYoloV5sGPU();

    public override string[] Label => ClassName.class4;
}

