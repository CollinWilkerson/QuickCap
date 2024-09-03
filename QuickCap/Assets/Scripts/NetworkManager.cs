using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//the thing that makes photon work

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    private void Awake()
    {
        //if an instance that isn't this one exists give up?
        if(instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            //makes the instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //connects to photon server
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    //photons change scene
    [PunRPC]
    //identifies this function as a remote procedure call, meaning it can operate across clients
    public void ChangeScene (string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    /*
    public override void OnConnectedToMaster()
    {
        CreateRoom("testroom");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("created room: " + PhotonNetwork.CurrentRoom.Name);
    }
    */
}
