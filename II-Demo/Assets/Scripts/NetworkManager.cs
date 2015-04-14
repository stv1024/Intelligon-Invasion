using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    private const int Port = 12345;

    public InputField IptIp;

    public void OnSetupServerClick()
    {
        var err = Network.InitializeServer(16, Port, true);
        Debug.Log("Init Server. Err : " + err);
    }
    public void OnConnectServerClick()
    {
        var err = Network.Connect(IptIp.text, Port);
        Debug.Log("Connect Server. Err : " + err);
    }

    void OnPlayerConnected(NetworkPlayer networkPlayer)
    {
        Debug.Log("Player connected from " + networkPlayer.ipAddress +
                  ":" + networkPlayer.port);
        
        //GameManager.Instance.StartGame();
    }

    void OnConnectedToServer()
    {
        Debug.Log("OnConnectedToServer");
        //GameManager.Instance.StartGame();
    }
}
