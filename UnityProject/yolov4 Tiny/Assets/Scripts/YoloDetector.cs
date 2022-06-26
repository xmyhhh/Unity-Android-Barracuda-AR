using UnityEngine;
using UnityEngine.UI;

using YoloV4Tiny;
using TMPro;


sealed class YoloDetector : MonoBehaviour
{

    public RenderTexture cameraTexture = null;
    [SerializeField, Range(0, 1)] float threshold = 0.5f;
    [SerializeField] ResourceSet mode = null;
    [SerializeField] RawImage preview = null;
    [SerializeField] Marker markerPrefab = null;



    ObjectDetector _detector;
    Marker[] _markers = new Marker[15];


    void Start()
    {
        //_source = webCamera.CameraTexture;
       
        _detector = new ObjectDetector(mode);
        for (var i = 0; i < _markers.Length; i++)
            _markers[i] = Instantiate(markerPrefab, preview.transform);

        int a = 0;
    }

    void OnDisable()
      => _detector.Dispose();

    void OnDestroy()
    {
        for (var i = 0; i < _markers.Length; i++) Destroy(_markers[i]);
    }

    int frameCount;
    float time;
    float pollingTime;
    public TextMeshProUGUI FpsText;
    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if(time > pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsText.text = frameRate.ToString()+" FPS";
        }

        _detector.ProcessImage(cameraTexture, threshold);

        var i = 0;
        foreach (var d in _detector.Detections)
        {
            if (i == _markers.Length) break;
            _markers[i++].SetAttributes(d);
        }
        for (; i < _markers.Length; i++) _markers[i].Hide();

        //_preview.texture = _source;
    }

}
