using OpenCV.Net;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Bonsai.JonsUtils
{
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Circular OpenCV Mat buffer.")]
    public class CircularBuffer
    {
        [Range(1, int.MaxValue)]
        [Description("The number of elements in the circular buffer.")]
        public int Count { get; set; } = 1;

        public IObservable<Mat> Process(IObservable<Mat> source)
        {
            CircularMatBuffer buffer = null;
            var count = Count; // freeze the size of buffer

            return source.Select(input =>
            {
                if (buffer == null)
                {
                    buffer = new CircularMatBuffer(input, count);
                }

                if (input.Cols >= count)
                {
                    throw new WorkflowRuntimeException("Input column dimension must be smaller than Count.");
                }

                buffer.Update(input);
                return buffer.Samples;
            });
        }
    }
}