using OpenCV.Net;
using System;

namespace Bonsai.JonsUtils
{
    class CircularMatBuffer
    {
        int offset;
        readonly Mat samples;

        public CircularMatBuffer(Mat template, int count)
        {
            if (count > 0)
            {
                samples = Mat.Zeros(template.Rows, count, template.Depth, template.Channels);
            }
        }

        public Mat Samples
        {
            get { return samples; }
        }

        public void Update(Mat source)
        {
            var windowElements = Math.Min(source.Cols, samples.Cols - offset);

            if (samples != null && windowElements > 0)
            {
                using (var dataSubRect = source.GetSubRect(new Rect(0, 0, windowElements, source.Rows)))
                using (var windowSubRect = samples.GetSubRect(new Rect(offset, 0, windowElements, samples.Rows)))
                {
                    CV.Copy(dataSubRect, windowSubRect);
                }

                offset += windowElements;
            }

            var remainder = source.Cols - windowElements;

            if (samples != null && 
                windowElements > 0 &&
                remainder != 0) // End of buffer
            {
                offset = 0; // reset offset
                using (var dataSubRect = source.GetSubRect(new Rect(windowElements, 0, remainder, source.Rows)))
                using (var windowSubRect = samples.GetSubRect(new Rect(offset, 0, remainder, samples.Rows)))
                {
                    CV.Copy(dataSubRect, windowSubRect);
                }

                offset += remainder;

            } else if (offset >= samples.Cols) 
            {
                offset = 0;
            }
        }
    }
}
