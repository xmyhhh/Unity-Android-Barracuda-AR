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

    public static int Size
    {
        get
        {
            return 6 * sizeof(int);
        }
    }

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

    static public BoundingBox[] NMS(BoundingBox[] rects, float threshold = 0.2f)
    {
        var rectRanges = new List<BoundingBox>(rects);

        var sorted = SortObservationsByConfidence(rectRanges);

        var selected = new List<BoundingBox>();


        foreach (var observationA in sorted)
        {
            var shouldSelect = true;

            foreach (var observationB in selected)
            {
                if (!(GetIOU(observationA, observationB) > threshold)) continue;
                shouldSelect = false;
                break;
            }

            if (shouldSelect)
            {
                selected.Add(observationA);
            }
        }

        return selected.ToArray();

    }
    static private double GetIOU(BoundingBox rects, BoundingBox box)
    {

        double overlapArea_W;
        double overlapArea_H;

        double output;

        //先计算overlapArea区域的W和H

        overlapArea_W = (
            Math.Max(
                Math.Min(rects.x + rects.width, box.x + box.width) - Math.Max(rects.x, box.x), 0)
            );
        overlapArea_H = (
            Math.Max(
                Math.Min(rects.y + rects.height, box.y + box.height) - Math.Max(rects.y, box.y), 0)
            );


        var overlapArea = (double)(overlapArea_W * overlapArea_H);
        output = (overlapArea / (double)((rects.width * rects.height) + (box.width * box.height) - overlapArea));


        return output;
    }

    static private IEnumerable<BoundingBox> SortObservationsByConfidence(IEnumerable<BoundingBox> observations)
    {
        var observationList = observations.ToList();
        observationList.Sort((a, b) => (a.confidence > b.confidence ? -1 : 1));
        return observationList;
    }


    public static int MaxValueIndex(float[] floatArray)
    {
        var max = floatArray[0];
        int maxIndex = 0;
        for (int i = 1; i < floatArray.Length; i++)
        {
            if (floatArray[i] > max)
            {
                max = floatArray[i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }
}
