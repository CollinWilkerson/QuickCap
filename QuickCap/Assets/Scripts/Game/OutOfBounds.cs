using UnityEngine;
using Photon.Pun;

public class OutOfBounds : MonoBehaviourPunCallbacks
{
    public GameObject hatStand;
    private Transform[] spawns;

    private void Start()
    {
        hatStand = GameObject.FindGameObjectWithTag("HatStand");
        spawns = GameManager.instance.spawnPoints;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!photonView.IsMine)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = spawns[Random.Range(0, spawns.Length)].position;
            //only calls if the player has the hat
            if (GameManager.instance.GetPlayer(collision.gameObject).id == GameManager.instance.playerWithHat)
            {
                GameManager.instance.photonView.RPC("ResetHat", RpcTarget.All);
            }
        }
    }
}
