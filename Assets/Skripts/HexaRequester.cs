using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public struct IKPParameters
{
    public string Type;
    public List<float> EndEffectorPosition;
    public List<float> EndEffectorOrientation;
}

public struct FKPParameters
{
    public string Type;
    public List<float> AcuatorParams;
}

public struct IKPResult
{
    public List<float> AcuatorAngels;
}

/// <summary>
///     Example of requester who only sends Hello. Very nice guy.
///     You can copy this class and modify Run() to suits your needs.
///     To use this class, you just instantiate, call Start() when you want to start and Stop() when you want to stop.
/// </summary>
public class HexaRequester : RunAbleThread
{
    
    /// <summary>
    ///     Request Hello message to server and receive message back. Do it 10 times.
    ///     Stop requesting when Running=false.
    /// </summary>
    protected override void Run()
    {
        ForceDotNet.Force(); // this line is needed to prevent unity freeze after one use, not sure why yet
        using (RequestSocket client = new RequestSocket())
        {
            client.Connect("tcp://localhost:5555");
        }
        NetMQConfig.Cleanup(); // this line is needed to prevent unity freeze after one use, not sure why yet
    }

    public List<float> ReqIKP(List<float> IKPParameters)
    {
        ForceDotNet.Force();
        IKPResult result = new IKPResult();
        using (RequestSocket client = new RequestSocket())
        {
            client.Connect("tcp://localhost:5555");

            IKPParameters IKPParams = new IKPParameters();
            IKPParams.Type = "IKP";
            IKPParams.EndEffectorPosition = new List<float>();
            IKPParams.EndEffectorOrientation = new List<float>();
            for (int i = 0; i < 3; i++)
            {
                IKPParams.EndEffectorPosition.Add(IKPParameters[i]);
                IKPParams.EndEffectorOrientation.Add(IKPParameters[i + 3]);
            }
            string reqIKP = JsonConvert.SerializeObject(IKPParams, Formatting.Indented);
            client.SendFrame(reqIKP);

            string message = null;
            bool gotMessage = false;
            while (Running)
            {
                gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
                if (gotMessage) break;
            }
            result = JsonConvert.DeserializeObject<IKPResult>(message);
            if (gotMessage) Debug.Log("Received " + message);

        }
        NetMQConfig.Cleanup();
        return result.AcuatorAngels;
    }
}