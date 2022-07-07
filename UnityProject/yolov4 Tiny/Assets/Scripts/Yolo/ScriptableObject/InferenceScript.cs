using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;

public abstract class InferenceScript : ScriptableObject
{


    public abstract void InitInference(Model model);

    public abstract BoundingBox[] RunInference(IWorker worker, RenderTexture source, float threshold);



}
