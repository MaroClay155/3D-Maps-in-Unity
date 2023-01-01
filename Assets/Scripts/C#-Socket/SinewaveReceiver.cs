using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class sinwaveReceiver : MonoBehaviour
{
    static Socket listener;
    private CancellationTokenSource source;
    public ManualResetEvent allDone;
   
    private Color matColor;////////////old//////
    private byte[] ImageBytesArray;
    private RawImage image;
    private Texture2D tex;
    public static readonly int PORT = 1755;
    public static readonly int WAITTIME = 1;


    sinwaveReceiver() //class constructor
    {
        source = new CancellationTokenSource();
        allDone = new ManualResetEvent(false);
    }

    async void Start()
    {
        byte[] testarray = new byte[]
        {
            0x30, 0x32, 0x32, 0x32, 0xe7, 0x30, 0xaa, 0x7f, 0x32, 0x32, 0x32, 0x32, 0xf9, 0x40, 0xbc, 0x7f,
            0x03, 0x03, 0x03, 0x03, 0xe6, 0x03, 0x02, 0x05, 0x03, 0x03, 0x03, 0x03, 0xf4, 0x30, 0x03, 0x06,
            0x32, 0x32, 0x32, 0x32, 0xe6, 0xe6, 0xaa, 0x7f, 0x32, 0xf2, 0x02, 0xa8, 0xe7, 0xe6, 0xff, 0xff,
            0x03, 0x03, 0x03, 0xff, 0xe6, 0x40, 0xe6, 0x0f, 0x00, 0xff, 0x00, 0xaa, 0xe6, 0x40, 0xe6, 0xff,
            0x5b, 0x03, 0x03, 0x03, 0xe6, 0x6a, 0x0f, 0xe6, 0x03, 0x03, 0x03, 0xe6, 0xca, 0x68, 0x0f, 0xe6,
            0xaa, 0x94, 0x90, 0x40, 0xe6, 0x5b, 0xaf, 0x68, 0xe6, 0x00, 0xe6, 0xff, 0xca, 0x58, 0x0f, 0x20,
            0x00, 0x00, 0x00, 0xff, 0xe6, 0x40, 0x01, 0x2c, 0x00, 0xe6, 0x00, 0xaa, 0xdb, 0x41, 0xff, 0xff,
            0x00, 0x00, 0x00, 0xff, 0xe8, 0x40, 0x01, 0x1c, 0x00, 0xff, 0x00, 0xaa, 0xbb, 0x40, 0xff, 0xff,
        };


        image = GetComponent<RawImage>();
        tex = new Texture2D(16, 16, TextureFormat.PVRTC_RGBA4, false);
        tex.LoadRawTextureData(ImageBytesArray);///image byte array
        //tex.LoadRawTextureData(testarray);/// test byte array
        tex.Apply();
        await Task.Run(() => ListenEvents(source.Token));
        
    }

    void Update()
    {
        image.material.mainTexture = tex;
    }

    private void ListenEvents(CancellationToken token)
    {


        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, PORT);


        listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);


            while (!token.IsCancellationRequested)
            {
                allDone.Reset();

                print("Waiting for a connection... host :" + ipAddress.MapToIPv4().ToString() + " port : " + PORT);
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                while (!token.IsCancellationRequested)
                {
                    if (allDone.WaitOne(WAITTIME))
                    {
                        break;
                    }
                }

            }

        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    void AcceptCallback(IAsyncResult ar)
    {
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        allDone.Set();

        ImageObject img2 = new ImageObject();
        img2.workSocket = handler;
        handler.BeginReceive(img2.buffer, 0, ImageObject.BufferSize, 0, new AsyncCallback(ReadCallback), img2);
    }

    void ReadCallback(IAsyncResult ar)
    {
        ImageObject img1 = (ImageObject)ar.AsyncState;
        Socket handler = img1.workSocket;

        int read = handler.EndReceive(ar);

        if (read > 0)
        {
            //img1.imagestring.Append(Encoding.ASCII.GetString(img1.buffer, 0, read));/////oldzzz
            img1.imagestring += Encoding.ASCII.GetString(img1.buffer, 0, ImageObject.BufferSize);////newzzz
            handler.BeginReceive(img1.buffer, 0, ImageObject.BufferSize, 0, new AsyncCallback(ReadCallback), img1);
        }
        else
        {
            if (img1.imagestring.Length > 1)
            {
                //string content = img1.imagestring.ToString();/////oldzzz
                //print($"Read {content.Length} bytes from socket.\n Data : {content}");
                //SetColors(content);//////////old/////////////
                ///setImage(content);/////////////new/////////////oldzzz
                setImage(img1.imagestring);////newzzz
            }
            handler.Close();
        }
    }
    /*
    //Set color to the Material
    private void SetColors(string data)
    {

        string[] colors = data.Split(',');
        matColor = new Color()
        {
            r = float.Parse(colors[0]) / 255.0f,
            g = float.Parse(colors[1]) / 255.0f,
            b = float.Parse(colors[2]) / 255.0f,
            a = float.Parse(colors[3]) / 255.0f
        };
    }
    */
    
    private void setImage (string data)
    {
        //string[] imagedata = data.Split(',');
        ImageBytesArray = Encoding.ASCII.GetBytes(data);
    }
    
    
    private void OnDestroy()
    {
        source.Cancel();
    }

    public class ImageObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        //public StringBuilder imagestring = new StringBuilder();//////oldzzz
        public string imagestring = null;//////newzzz
    }
}

