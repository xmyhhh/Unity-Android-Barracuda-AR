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

    public BoundingBox(float _x, float _y, float _width, float _height, float _confidence, int _classIndex)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
        confidence = _confidence;
        classIndex = _classIndex;
    }
}

static public class DataProcess
{

    static public BoundingBox[] NMS(BoundingBox[] rects, float threshold = 0.3f)
    {
        var rectRanges = new List<BoundingBox>(rects);
        var rectResult = new List<BoundingBox>();
        var keepResult = new bool[rects.Length];

        for (int idx = 0; idx < rects.Length - 1; idx++)
        {

            var order = rectRanges.GetRange(idx + 1, (rects.Length - idx) - 1);
            var iou = GetIOU(order.ToArray(), rects[idx]);
            for (int col = 0; col < iou.Count; col++)
            {
                if (iou[col] > threshold)
                {
                    /* get rect */
                    keepResult[idx + col] = false;
                    //rect_result.RemoveAt(idx);
                }
                else
                {
                    keepResult[idx + col] = true;
                }
            }
        }

        for (int idx = 0; idx < keepResult.Length; idx++)
        {
            if (keepResult[idx])
            {
                rectResult.Add(rectRanges[idx]);
            }
        }

        return rectResult.ToArray();

    }
    static private List<double> GetIOU(BoundingBox[] rects, BoundingBox box)
    {
        var rectLen = rects.Length;
        var overlapArea_W = new List<double>();
        var overlapArea_H = new List<double>();

        var output = new List<double>();

        //先计算overlapArea区域的W和H
        for (int idx = 0; idx < rectLen; idx++)
        {
            overlapArea_W.Add(
                Math.Max(
                    Math.Min(rects[idx].x + rects[idx].width, box.x + box.width) - Math.Max(rects[idx].x, box.x), 0)
                );
            overlapArea_H.Add(
                Math.Max(
                    Math.Min(rects[idx].y + rects[idx].height, box.y + box.height) - Math.Max(rects[idx].y, box.y), 0)
                );
        }

        for (int idx = 0; idx < rectLen; idx++)
        {

            var overlapArea = (double)(overlapArea_W[idx] * overlapArea_H[idx]);
            output.Add(overlapArea / (double)((rects[idx].width * rects[idx].height) + (box.width * box.height) - overlapArea));
        }

        return output;
    }
}
