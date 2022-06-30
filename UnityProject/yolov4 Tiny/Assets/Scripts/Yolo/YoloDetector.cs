using UnityEngine;
using UnityEngine.UI;

using YoloV4Tiny;
using TMPro;
public enum InferenceDeviceType
{
    CPU,
    GPU
}


sealed class YoloDetector : MonoBehaviour
{

    public RenderTexture cameraTexture = null;
    [SerializeField, Range(0, 1)] float threshold = 0.5f;
    [SerializeField] ResourceSet resourceSet = null;
    public InferenceDeviceType inferenceDeviceType = InferenceDeviceType.CPU;
    [SerializeField] RawImage preview = null;
    [SerializeField] Marker markerPrefab = null;

    InferenceScript inferenceScript;

    Marker[] _markers = new Marker[15];

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

        inferenceScript.RunInference(resourceSet.model, cameraTexture, threshold);

    }

}
