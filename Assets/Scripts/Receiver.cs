using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

public class Receiver : MonoBehaviour
{
    [SerializeField] private int port = 55552;
    [SerializeField] private bool async = true;

    private UdpClient? udpClient;

    private void Start()
    {
        udpClient = new UdpClient(port);
        if (async)
        {
            UpdateRotationAsync();
        }
    }

    private void Update()
    {
        IPEndPoint? endPoint = null;
        while (async == false)
        {
            Apply(udpClient!.Receive(ref endPoint));
        }
    }

    [ContextMenu(nameof(UpdateRotationAsync))]
    private async void UpdateRotationAsync()
    {
        udpClient ??= new UdpClient(port);
        while (Application.isPlaying)
        {
            var data = await udpClient.ReceiveAsync();
            Apply(data.Buffer!);
        }
    }

    private void Apply(byte[] array)
    {
        var message = Encoding.ASCII.GetString(array);
        var split = message.Split('!')!;
        var x = float.Parse(split[0]);
        var y = float.Parse(split[1]);
        var z = float.Parse(split[2]);
        transform.rotation = Quaternion.Euler(x, y, z);
        print(transform.rotation);
    }
}