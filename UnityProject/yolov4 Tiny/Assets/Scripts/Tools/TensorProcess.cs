using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Barracuda;

static public class TensorProcess
{
    static public Tensor StridedSlice(this Tensor input, int[] starts, int[] ends)
    {
        Model model = new Model();
        var builder = new ModelBuilder(model);

        int[] strides = { 1, 1, 1, 1 };
        builder.StridedSlice("outputslice", "original_output", starts, ends, strides);

        var worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, builder.model);

        worker.Execute(input);


        var res = worker.PeekOutput();
        worker.Dispose();
        return res;
    }



}

