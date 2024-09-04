using UnityEngine;
using Photon.Pun;

public class OutOfBounds : MonoBehaviourPun
{
    private Transform[] spawns;

    private void Start()
    {
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
        }
    }
}
