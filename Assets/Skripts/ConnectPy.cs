using UnityEngine;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine.UI;
using System;

public struct Parameters
{
    public List<float> EffectorPosition;
    public List<float> EffectorOrientation;
}


public class ConnectPy : MonoBehaviour
{
    public InputField Infobox;

    const string api = "API-2";
    Socket sender;
    byte[] messageHolder;
    const int messageLength = 12000;

    bool init = false;
    int index = 0;

    Parameters paramerters = new Parameters();
    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(20, 20, 120, 60), "CONNECT"))
    //    {
    //        if (!init)
    //            Initial();
    //        else
    //            Debug.Log("Socket has Initial");
    //    }
    //    if (GUI.Button(new Rect(20, 130, 120, 60), "IKP"))
    //    {
    //        if (init)
    //        {
    //            SendParameters(paramerters);
    //            new Thread(() =>
    //            {
    //                Receive();
    //            }).Start();
    //        }
    //    }
    //    if (GUI.Button(new Rect(20, 240, 120, 60), "ACTION"))
    //    {
    //        if (init)
    //        {
    //            SendString("ACTION");
    //            new Thread(() => 
    //            {
    //                Receive();
    //            }).Start();
    //        }
    //    }
    //    if (GUI.Button(new Rect(20, 350, 120, 60), "EXIT"))
    //    {
    //        SendString("EXIT");
    //        OnApplicationQuit();
    //    }
    //}


    public void Initial()
    {
        init = true;
        messageHolder = new byte[messageLength];

        // Create a TCP/IP  socket
        sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sender.Connect("localhost", 5006);
        paramerters.EffectorPosition = new List<float>();
        paramerters.EffectorPosition.Add(0);
        paramerters.EffectorPosition.Add(0);
        paramerters.EffectorPosition.Add(-500);
        paramerters.EffectorOrientation = new List<float>();
        paramerters.EffectorOrientation.Add(0);
        paramerters.EffectorOrientation.Add(0);
        paramerters.EffectorOrientation.Add(0);
        Infobox.text += "***   socket is init   ***" + Environment.NewLine;
        Debug.Log("***   socket is init   ***");
    }


    public void SendParameters()
    {
        string envMessage = JsonConvert.SerializeObject(paramerters, Formatting.Indented);
        Infobox.text += envMessage;
        sender.Send(Encoding.UTF8.GetBytes(envMessage));
        new Thread(() =>
                {
                    Receive();
                }).Start();

    }

    public void SendString(string str)
    {
        try
        {
            Debug.Log("send:" + str);
            sender.Send(AppendLength(Encoding.UTF8.GetBytes(str)));
        }
        catch (SocketException e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    private byte[] AppendLength(byte[] input)
    {
        byte[] newArray = new byte[input.Length + 4];
        input.CopyTo(newArray, 4);
        System.BitConverter.GetBytes(input.Length).CopyTo(newArray, 0);
        return newArray;
    }

    private void Receive()
    {
        int location = sender.Receive(messageHolder);
        string message = Encoding.UTF8.GetString(messageHolder, 0, location);
        Debug.Log("recv: " + message);
        Infobox.text += "recv: " + message;
    }

    public void OnApplicationQuit()
    {
        if (init && sender != null)
        {
            Debug.Log("Socket is closing");
            sender.Close();
        }
    }

}
