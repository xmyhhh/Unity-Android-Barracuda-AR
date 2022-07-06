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

    public RenderTexture inputTexture = null;
    [SerializeField, Range(0, 1)] float threshold = 0.5f;
    [SerializeField] ResourceSet resourceSet = null;
    public InferenceDeviceType inferenceDeviceType = InferenceDeviceType.CPU;
    [SerializeField] RawImage preview = null;
    [SerializeField] Marker markerPrefab = null;

    InferenceScript inferenceScript;

    Marker[] markers = new Marker[50];

    Model model;
    IWorker worker;


    void Start()
    {

        // Marker populating
        for (var i = 0; i < markers.Length; i++)
        {
            markers[i] = Instantiate(markerPrefab, preview.transform);
            markers[i].SetLabel(resourceSet.Label);
        }

        if (inferenceDeviceType == InferenceDeviceType.GPU)
        {
            inferenceScript = resourceSet.inferenceGPU;
        }
        else
        {
            inferenceScript = resourceSet.inferenceCPU;
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

        var predict = inferenceScript.RunInference(worker, (inputTexture), threshold);

        // Marker update
        var i = 0;

        foreach (var box in predict)
        {
            if (i == markers.Length) break;
            markers[i++].SetAttributes(box);
            //markers[49].SetAttributes(new BoundingBox(0,0,640,480,1,0));
        }

        for (; i < markers.Length; i++) markers[i].Hide();
        //markers[49].Show();
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }
}
