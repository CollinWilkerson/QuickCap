using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Stats")]
    public bool gameEnded = false;
    public float timeToWin; //remaining hat time
    public float invincibleDuration; //time between hat vulnerability
    private float hatPickupTime; // time hat was picked up

    [Header("Players")]
    public string playerPrefabLocation; //path to player prefab
    public Transform[] spawnPoints; //array of possible spawns could possibly vector3s
    public PlayerController[] players; //array of players
    public int playerWithHat; //index of the player with hat
    private int playersInGame;

    public static GameManager instance;

    private void Awake()
    {
        instance = this; //I think this sets this as a common point for all clients.
    }

    private void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.All); //Im guessing this calls ImInGame on all systems
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++; //adds a player for every player that joins

        if (playersInGame == PhotonNetwork.PlayerList.Length) //if we have the same amount of players as the photonNetwork says we should, spawn the players
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        //creates a player across the network out of playerPrefabLocation at a random spawnPoints position at the player prefab's rotation
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        PlayerController playerScript = playerObj.GetComponent<PlayerController>();

        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer); //runs the Initialize function in the player script
    }

    public PlayerController GetPlayer(int playerId)
    {
        //gets the first x that has the id attribute of playerId
        return players.First(x => x.id == playerId);
    }

    public PlayerController GetPlayer(GameObject playerObject)
    {
        return players.First(x => x.gameObject == playerObject);
    }

    [PunRPC]
    public void GiveHat(int playerID, bool initialGive)
    {
        //take hat
        if (!initialGive)
        {
            GetPlayer(playerWithHat).SetHat(false); //
        }

        playerWithHat = playerID; //defines which player has the hat
        GetPlayer(playerID).SetHat(true); //activates hat for player who picked it up
        hatPickupTime = Time.time; //sets the time picked up to the current time
    }

    public bool CanGetHat()
    {
        //timer doesn't reset between pickups so the hatPickupTime allows us to measure when the invincibility has expired.
        if (Time.time > hatPickupTime + invincibleDuration)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    private void WinGame (int playerId)
    {
        gameEnded = true;
        PlayerController player = GetPlayer(playerId);
        //UI shows whos won
        GameUI.instance.SetWinText(player.photonPlayer.NickName);

        Invoke("GoBackToMenu", 3.0f);
    }

    private void GoBackToMenu()
    {
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.ChangeScene("Menu");
    }
}
