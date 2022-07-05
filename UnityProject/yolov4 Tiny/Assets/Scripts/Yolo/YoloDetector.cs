using UnityEngine;
using UnityEngine.UI;

using Unity.Barracuda;
using TMPro;
public enum InferenceDeviceType
{
    CPU,
    GPU
}


sealed class YoloDetector : MonoBehaviour
{

    public Texture2D inputTexture = null;
    [SerializeField, Range(0, 1)] float threshold = 0.5f;
    [SerializeField] ResourceSet resourceSet = null;
    public InferenceDeviceType inferenceDeviceType = InferenceDeviceType.CPU;
    [SerializeField] RawImage preview = null;
    [SerializeField] Marker markerPrefab = null;

    InferenceScript inferenceScript;

    Marker[] _markers = new Marker[15];

    Model model;
    IWorker worker;


    void Start()
    {
        if (inferenceDeviceType == InferenceDeviceType.GPU)
        {
            inferenceScript = resourceSet.InferenceGPU;
        }
        else
        {
            inferenceScript = resourceSet.InferenceCPU;
        }
        model = ModelLoader.Load(resourceSet.model);

        inferenceScript.InitInference(model);

        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

    }

    int frameCount;
    float time;
    float pollingTime;
    public TextMeshProUGUI FpsText;
    void Update()
    {
        #region FPS
        time += Time.deltaTime;
        frameCount++;
        if (time > pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsText.text = frameRate.ToString() + " FPS";
        }
        #endregion


        inferenceScript.RunInference(worker, TextureConverter.Texture2DToRenderTexture(inputTexture), threshold);

    }

    void OnDestroy()
    {
        worker?.Dispose();
    }
}
