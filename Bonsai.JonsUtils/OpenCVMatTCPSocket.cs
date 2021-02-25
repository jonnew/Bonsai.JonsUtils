using System;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using OpenCV.Net;
//using System.Reactive.Disposables;

namespace Bonsai.JonsUtils
{
    [Description("Sends a 2D Open CV Mat to a TCP socket.")]
    public class OpenCVMatTCPSocket : Sink<Mat>
    {
        [Description("Address")]
        public string addr { get; set; } = "localhost";

        [Description("Port number. Changing while running has no effect.")]
        public int port { get; set; } = 5000;

        // Receiver has to provide number of rows and cols
        public override IObservable<Mat> Process(IObservable<Mat> source)
        {
            return Observable.Using(
                () =>
                {
                    var c = new TcpClient();
                    c.Connect(addr, port);
                    return c.GetStream();
                    // Is c being disposed?
                    //return new CompositeDisposable(c, s);
                },
                s =>
                {
                    return source.Do(value =>
                    {
                        var num_bytes = value.ElementSize * value.Cols * value.Rows;
                        var data = new byte[num_bytes];
                        Marshal.Copy(value.Data, data, 0, num_bytes);
                        s.Write(data, 0, num_bytes);
                    });
                });
        }
    }
}