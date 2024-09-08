using System.Collections;
using UnityEngine;
using Photon.Pun;

public class OutOfBounds : MonoBehaviourPunCallbacks
{
    public GameObject hatStand;
    public float respawnTime;

    private Transform[] spawns;
    private IEnumerator respawn;

    private void Start()
    {
        hatStand = GameObject.FindGameObjectWithTag("HatStand");
        spawns = GameManager.instance.spawnPoints;
    }

    //uses the IEnumerator type to have a respawn time
    private IEnumerator OnCollisionEnter(Collision collision)
    {

        if (!photonView.IsMine)
        {
            yield break;
        }

        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.SetActive(false);
            //only calls if the player has the hat
            if (GameManager.instance.GetPlayer(collision.gameObject).id == GameManager.instance.playerWithHat)
            {
                GameManager.instance.photonView.RPC("ResetHat", RpcTarget.All);
            }

            respawn = Respawn(collision.gameObject);
            yield return StartCoroutine(respawn);
        }
    }

    IEnumerator Respawn(GameObject player)
    {
        yield return new WaitForSeconds(respawnTime);

        player.SetActive(true);
        player.transform.position = spawns[Random.Range(0, spawns.Length)].position;
    }


}
