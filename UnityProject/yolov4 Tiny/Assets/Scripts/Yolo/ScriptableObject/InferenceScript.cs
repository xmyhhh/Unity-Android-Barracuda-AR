using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;

public abstract class InferenceScript
{
    protected IWorker worker;
    protected Model model;
    protected float threshold;
    protected ResourceSet resourceSet;
    public virtual void InitInference(ResourceSet resourceSet, float threshold)
    {
        model = ModelLoader.Load(resourceSet.model);
        worker = model.CreateWorker();
        this.threshold = threshold;
        this.resourceSet = resourceSet;
    }

    public abstract BoundingBox[] RunInference(RenderTexture source);

    public abstract Tensor PreProcess(RenderTexture source);

    public abstract BoundingBox[] PostProcess(Tensor input, float threshold);

    public virtual void DisposeObjects()
    {
        worker?.Dispose();
    }
}
