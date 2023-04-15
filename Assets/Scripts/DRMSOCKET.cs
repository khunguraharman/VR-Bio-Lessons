using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Text;
using System;


public class DRMSOCKET : MonoBehaviour
{
    ClientWebSocket drm_socket = new ClientWebSocket();
    // Start is called before the first frame update
    async void Awake()
    {        
        await drm_socket.ConnectAsync(new System.Uri("wss://anatomicus.ca/wss/verify_client_connectivity/"), System.Threading.CancellationToken.None);
        string message = "An Anatomicus Client just Connected!";
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        await drm_socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);

        var session_message = JsonUtility.ToJson(MainMenu.login_session);
        Debug.Log("the session ID Json" + session_message);
        messageBytes = Encoding.UTF8.GetBytes(session_message);
        await drm_socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
    }

    private void FixedUpdate()
    {
        /*
        if (drm_socket.State == WebSocketState.Open)
        {
            Debug.Log("WebSocket connection successful!");
        }   
        */
    }
    
}
