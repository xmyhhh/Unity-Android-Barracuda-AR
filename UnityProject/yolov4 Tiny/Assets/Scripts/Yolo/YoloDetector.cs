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
            inferenceScript = resourceSet.InferenceGPU;
        }
        else
        {
            inferenceScript = resourceSet.InferenceCPU;
        }


        inferenceScript.InitInference(resourceSet, threshold);

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

        var i = 0;

        foreach (var box in inferenceScript.RunInference(inputTexture))
        {
            if (i == markers.Length) break;
            markers[i++].SetAttributes(box);
        }

        for (; i < markers.Length; i++) markers[i].Hide();
    }

    void OnDestroy()
    {
        inferenceScript.DisposeObjects();
    }
}
