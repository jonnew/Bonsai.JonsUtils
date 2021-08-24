using OpenCV.Net;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Runtime.InteropServices;

namespace Bonsai.JonsUtils
{
    [Description("Sends a 2D Open CV Mat to a datagram (UDP) socket.")]
    public class OpenCVMatUDPClient : Sink<Mat>
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
                    var u = new UdpClient();
                    u.Connect(addr, port);
                    return u;
                },
                u =>
                {
                    return source.Do(value =>
                    {
                        var num_bytes = value.ElementSize * value.Cols * value.Rows;
                        var data = new byte[num_bytes];
                        Marshal.Copy(value.Data, data, 0, num_bytes);
                        u.Send(data, num_bytes);
                    });
                });
        }
    }
}