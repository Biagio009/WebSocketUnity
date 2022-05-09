using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class SocketManager : MonoBehaviour
{
    WebSocket socket;
    public GameObject clientPlayer;
    public PlayerData playerData;
    //public PlayerData player2Data;
    public GameObject serverPlayer;
    public string clientId;
    public PlayerData jsonObj;

    // Start is called before the first frame update
    void Start()
    {
        socket = new WebSocket("ws://localhost:8080");
        socket.Connect();

        socket.OnMessage += (sender, e) =>
        {
            //controlla se data è testo
            if (e.IsText)
            {
                /*var data = e.Data.Split(' ');
                if (e.Data.Split(' ')[0].Trim()=="id" && clientId == "")
                {
                    clientId = e.Data.Split(' ')[1].Trim();
                }*/
                //Debug.Log("IsText");
                //Debug.Log(e.Data);

                try
                {
                    //jsonObj = JsonUtility.FromJson<PlayerData>(e.Data);
                    //var jsonObj = JsonConvert.DeserializeObject<PlayerData>(e.Data);
                }
                catch (System.Exception ex)
                {

                    Debug.Log(ex.Message);
                }


                //ottengo id dal server
                /*if ((string)jsonObj.id!=clientId)
                {
                    //converto i data json in data object del giocatore
                    serverPlayer.transform.SetPositionAndRotation(new Vector3(playerData.xPos, playerData.yPos, playerData.zPos), Quaternion.identity);
                    Debug.Log("Id Giocatore: " + playerData.id);
                    return;
                }*/

                

                //Get Initial Data server ID data (From intial serverhandshake
                if (jsonObj.id != null)
                {
                    playerData.id = e.Data;
                    //Convert Intial player data Json (from server) to Player data object
                    //PlayerData tempPlayerData = JsonUtility.FromJson<PlayerData>(e.Data);
                    //playerData = tempPlayerData;
                    Debug.Log("player ID is " + playerData.id);
                    /*if (playerData.id=="0")
                    {
                        GameObject.Find("Player1").SetActive(true);
                    }
                    else if (playerData.id == "1")
                    {
                        GameObject.Find("Player2").SetActive(true);
                    }*/
                    return;
                }


                if (true)
                {

                }

            }
        };


        //se la conessione si chiude
        socket.OnClose += (sender, e) =>
         {
             Debug.Log(e.Code);
             Debug.Log(e.Reason);
             Debug.Log("Connessione chiusa");
         };
    }

    // Update is called once per frame
    void Update()
    {
        //client to server
        if (socket==null)
        {
            return;
        }

        //manda dati del player al server
        if (clientPlayer!= null && playerData.id == "0")
        {
            //ottengo la posizione e rotazione
            playerData.xPos = GameObject.Find("Player1").transform.position.x;
            playerData.yPos = GameObject.Find("Player1").transform.position.y;
            playerData.zPos = GameObject.Find("Player1").transform.position.z;

            //ottengo il time stamp
            System.DateTime start = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - start).TotalSeconds;
            //Debug.Log(timestamp);
            playerData.timestamp = timestamp;

            string playerDataJSON = JsonConvert.SerializeObject(playerData);
            socket.Send(playerDataJSON);
           
        }

        if (clientPlayer != null && playerData.id == "1")
        {

            //ottengo la posizione e rotazione
            playerData.xPos = GameObject.Find("Player2").transform.position.x;
            playerData.yPos = GameObject.Find("Player2").transform.position.y;
            playerData.zPos = GameObject.Find("Player2").transform.position.z;

            

            //ottengo il time stamp
            System.DateTime start = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - start).TotalSeconds;
            //Debug.Log(timestamp);
            playerData.timestamp = timestamp;

            string playerDataJSON = JsonConvert.SerializeObject(playerData);
            socket.Send(playerDataJSON);

        }


        //manda un messaggio con la key m
        if (Input.GetKeyDown(KeyCode.M))
        {
            string messaggioJSON = "{\"messaggio\": \"Ho mandato un messaggio\"}";
            socket.Send(messaggioJSON);
            Debug.Log(messaggioJSON);

        }

    }

    public void MovePlayer1()
    {

    }

    private void OnDestroy()
    {
        //chiuse il socket quando si esce dall'applicazione
        socket.Close();
    }
}
