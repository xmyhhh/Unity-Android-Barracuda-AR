using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;

public abstract class InferenceScript : ScriptableObject
{
    protected Model model;
    protected IWorker worker;


    public virtual void InitInference(ResourceSet resourceSet)
    {
        model = ModelLoader.Load(resourceSet.model);
    }
    public abstract void RunInference(RenderTexture source, float threshold);
}
