using System;
using System.Linq;
using System.Reactive.Linq;
using System.ComponentModel;
using OpenCV.Net;

namespace Bonsai.JonsUtils
{
    [Combinator]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Map channels or columns of matrix.")]
    public class MatrixMap
    {
        //Mat MatMap(Tuple<Mat, Mat> source)
        Mat MatMap(Mat source)
        {
            var output = new Mat(source.Size, source.Depth, source.Channels);
            var map = Map.Reshape(0, 1);

            // I dont know wtf is going on with this
            if (MapDimension == Dimension.ROWS)
            {
                for (int i = 0; i < map.Cols; i++)
                    CV.Copy(source.GetRow((int)map[i].Val0), output.GetRow(i));
            }
            else
            {
                for (int i = 0; i < map.Cols; i++)
                    CV.Copy(source.GetCol((int)map[i].Val0), output.GetCol(i));
            }

            return output;
        }

        public IObservable<Mat> Process(IObservable<Mat> source)
        {
            return source.Select(input => MatMap(input));
        }

        public enum Dimension
        {
            ROWS,
            COLS
        }

        [Description("Dimension to map. (e.g. 0 = rows.")]
        public Dimension MapDimension { get; set; } = Dimension.ROWS;

        [Description("The map. This is a vector of indicies to map either the rows or colums with.")]
        public Mat Map { get; set; }

    }
}
