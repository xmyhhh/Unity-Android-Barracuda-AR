using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;

public abstract class InferenceScript : ScriptableObject
{
    public abstract void RunInference(NNModel model, Texture source, float threshold);
}
