using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public float moveSpeed;
    public float jumpForce;
    public GameObject hatObject;

    [Header("Components")]
    public Rigidbody rig;
    public Player photonPlayer;

    [SerializeField]
    public float curHatTime = 0;

    private void Update()
    {
        if (photonView.IsMine)
        {
            //standard jump and move controls
            Move();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump();
            }

            //time hat is on
            if (hatObject.activeInHierarchy)
            {
                curHatTime += Time.deltaTime; //This is where the player gains hat time. Comminicated to other clients in OnPhotonSerializeView
            }
        }

        //Master client checks for all
        if (PhotonNetwork.IsMasterClient)
        {
            //timeToWin is just a set number, set in GameManager
            //curHatTime is a float
            if(curHatTime >= GameManager.instance.timeToWin && !GameManager.instance.gameEnded)
            {
                //Debug.Log("Game Won");
                GameManager.instance.gameEnded = true;
                GameManager.instance.photonView.RPC("WinGame", RpcTarget.All, id);
            }
        }
    }

    private void Move()
    {
        //standard controls
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;
        //establishes a velocity based on the player inputs
        rig.linearVelocity = new Vector3(x, rig.linearVelocity.y, z);
    }

    private void TryJump()
    {
        //raycasts from the player's position downward 0.7 units. if the raycast hits, the player jumps
        Ray ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, 0.7f))
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    [PunRPC]
    public void Initialize(Player player)
    {
        //helps to distinguish players
        photonPlayer = player;
        id = player.ActorNumber;
        //Debug.Log(player.ActorNumber);

        GameManager.instance.players[id - 1] = this; //sets the game manager to an index in relation to its Photon id

        //photonView is part of the MonoBehaviorPun extension so i changed that
        //Also why would i want to make my client kenimatic? another tutorial whoopsie
        if (!photonView.IsMine)
        {
            rig.isKinematic = true; // game physics should only affect the clients individually
        }

        
        //Gives player 1 the hat CHANGE IN PR
        /*
        if (id==1)
        {
            GameManager.instance.GiveHat(id, true);
        }
        */
    }

    public void SetHat(bool hasHat)
    {
        hatObject.SetActive(hasHat);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //Debug.Log("collision");
        if (collision.gameObject.CompareTag("Player")) //if we hit a player
        {
            //Debug.Log("hit player");
            if (GameManager.instance.GetPlayer(collision.gameObject).id == GameManager.instance.playerWithHat) //checks if the player has the hat
            {
                if (GameManager.instance.CanGetHat()) //checks if invicibility time is over
                {
                    GameManager.instance.photonView.RPC("GiveHat", RpcTarget.All, id, false);
                }
            }
        }

        if (collision.gameObject.CompareTag("HatStand"))
        {
            GameManager.instance.photonView.RPC("GiveHat", RpcTarget.All, id, true);
            collision.gameObject.SetActive(false);
        }
    }

    //sends and recives the Hat time data
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Debug.Log("Write");
            stream.SendNext(curHatTime);
        }
        else //if (stream.IsReading)
        {
            //Debug.Log("Read");
            curHatTime = (float)stream.ReceiveNext();
        }
    }
}
