using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public struct BoundingBox
{
    public float x;
    public float y;
    public float height;
    public float width;
    public float confidence;
    public int classIndex;

    public BoundingBox(float _x, float _y, float _width, float _height,float _confidence, int _classIndex)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
        confidence = _confidence;
        classIndex = _classIndex;   
    }
}

public class DataProcess
{

    private BoundingBox[] NMS(BoundingBox[] rects, float[] prob, float threshold = 0.3f)
    {
        var rect_ranges = new List<BoundingBox>(rects);
        var rect_result = new List<BoundingBox>();
        var keep_result = new bool[rects.Length];

        for (int idx = 0; idx < rects.Length - 1; idx++)
        {

            var order = rect_ranges.GetRange(idx + 1, (rects.Length - idx) - 1);
            var iou = Get_iou(order.ToArray(), rects[idx]);
            for (int col = 0; col < iou.Count; col++)
            {
                if (iou[col] > threshold)
                {
                    /* get rect */
                    keep_result[idx + col] = false;
                    //rect_result.RemoveAt(idx);
                }
                else
                {
                    keep_result[idx + col] = true;
                }
            }
        }

        for (int idx = 0; idx < keep_result.Length; idx++)
        {
            if (keep_result[idx])
            {
                rect_result.Add(rect_ranges[idx]);
            }
        }

        return rect_result.ToArray();

    }
    private List<double> Get_iou(BoundingBox[] rects, BoundingBox box)
    {
        var rect_len = rects.Length;
        var xx1 = new List<double>();
        var yy1 = new List<double>();
        var union = new List<double>();

        for (int idx = 0; idx < rect_len; idx++)
        {
            xx1.Add(Math.Max(Math.Min(rects[idx].x + 0.5 * rects[idx].width, box.x + 0.5 * box.width) - Math.Max(rects[idx].x - 0.5 * rects[idx].width, box.x - 0.5 * box.width), 0));
            yy1.Add(Math.Max(Math.Min(rects[idx].y + 0.5 * rects[idx].height, box.y + 0.5 * box.height) - Math.Max(rects[idx].y - 0.5 * rects[idx].height, box.y - 0.5 * box.height), 0));
        }

        for (int idx = 0; idx < rect_len; idx++)
        {
            //inter.Add(xx1[idx] * yy1[idx]);
            var inter = (double)(xx1[idx] * yy1[idx]);
            union.Add(inter / (double)((rects[idx].width * rects[idx].height) + (box.width * box.height) - inter));
        }

        return union;
    }
}
