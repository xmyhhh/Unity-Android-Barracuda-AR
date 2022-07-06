using UnityEngine;
using UnityEngine.UI;
using YoloV4Tiny;

public class Marker : MonoBehaviour
{
    RectTransform parent;
    RectTransform xform;
    Image panel;
    Text label;

    string[] labels;

    void Start()
    {
        xform = GetComponent<RectTransform>();
        parent = (RectTransform)xform.parent;
        panel = GetComponent<Image>();
        label = GetComponentInChildren<Text>();
    }

    public void SetLabel(string[] _labels)
    {
        labels = _labels;
    }

    public void SetAttributes(in BoundingBox d)
    {
        var rect = parent.rect;

        // Bounding box position
        var x = d.x - d.width / 2;
        var y = 480  -(d.y - d.height / 2);
        var w = d.width;
        var h = d.height ;

        xform.anchoredPosition = new Vector2(x, y);
        xform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        xform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        // Label (class name + score)
        var name = labels[(int)d.classIndex];
        Debug.Log(name);
        Debug.Log($"x:{d.x},y:{d.y},width:{d.width},height:{d.height}");
        label.text = $"{name} {(int)(d.confidence * 100)}%";

        // Panel color
        var hue = d.classIndex * 0.073f % 1.0f;
        var color = Color.HSVToRGB(hue, 1, 1);
        color.a = 0.4f;
        panel.color = color;

        // Enable
        gameObject.SetActive(true);
    }

    public void Hide()
      => gameObject.SetActive(false);
    public void Show()
  => gameObject.SetActive(true);
}
