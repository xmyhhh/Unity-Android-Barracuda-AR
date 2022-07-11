using UnityEngine;
using UnityEngine.UI;

using Unity.Barracuda;
using TMPro;
using System.Collections;

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

    Marker[] markers = new Marker[15];

    Model model;
    IWorker worker;

    Tensor input;
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
        //worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, model);

        worker = model.CreateWorker();

        StartCoroutine(InferenceCoroutine());
        //input = inferenceScript.PreProcess(inputTexture);
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


        //var output = worker.Execute(input).PeekOutput();

    }

    IEnumerator InferenceCoroutine()
    {
        while (true)
        {
            // convert texture into Tensor of shape [1, imageToRecognise.height, imageToRecognise.width, 3]
            using (var input = inferenceScript.PreProcess(inputTexture))
            {
                // execute neural network with specific input and get results back
                var output = worker.Execute(input).PeekOutput();

                //// allow main thread to run until neural network execution has finished
                yield return new WaitForCompletion(output);

                var predict = inferenceScript.PostProcess(output, threshold);


                var i = 0;

                foreach (var box in predict)
                {
                    if (i == markers.Length) break;
                    markers[i++].SetAttributes(box);
                }

                for (; i < markers.Length; i++) markers[i].Hide();
            }
        }
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }
}
