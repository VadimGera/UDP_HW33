using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Sender : MonoBehaviour
{
    [SerializeField] private int port = 55551;
    [SerializeField] private int targetPort = 55552;

    private UdpClient udpClient = null;
    private string? previous = null;


    private void Start()
    {
        udpClient = new UdpClient(port);
    }

    private void Update()
    {
        var rotation = transform.rotation;

        var rotationMessage = $"{rotation.x}!{rotation.y}!{rotation.z}";
        if (previous == rotationMessage)
        {
            return;
        }
        
        var array = Encoding.ASCII.GetBytes(rotationMessage);
        udpClient.Send(array, array.Length, "localhost", targetPort);

        previous = rotationMessage;
    }
}