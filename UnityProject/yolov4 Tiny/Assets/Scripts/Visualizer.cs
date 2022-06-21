using UnityEngine;
using UnityEngine.UI;
using Klak.TestTools;
using YoloV4Tiny;
using TMPro;
sealed class Visualizer : MonoBehaviour
{
    #region Editable attributes

    //[SerializeField] ImageSource _source = null;
    Texture2D _source = null;
    [SerializeField, Range(0, 1)] float _threshold = 0.5f;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] RawImage _preview = null;
    [SerializeField] Marker _markerPrefab = null;
    //[SerializeField] WebCamera webCamera = null;
    [SerializeField] UVCCamera uvcCamera = null;

    #endregion

    #region Internal objects

    ObjectDetector _detector;
    Marker[] _markers = new Marker[15];

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        //_source = webCamera.CameraTexture;
        _source = uvcCamera.tempTexture2D;
        _detector = new ObjectDetector(_resources);
        for (var i = 0; i < _markers.Length; i++)
            _markers[i] = Instantiate(_markerPrefab, _preview.transform);

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

        _detector.ProcessImage(_source, _threshold);

        var i = 0;
        foreach (var d in _detector.Detections)
        {
            if (i == _markers.Length) break;
            _markers[i++].SetAttributes(d);
        }
        for (; i < _markers.Length; i++) _markers[i].Hide();

        _preview.texture = _source;
    }

    #endregion
}
