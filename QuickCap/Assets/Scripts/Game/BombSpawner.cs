using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BombSpawner : MonoBehaviourPun
{
    private string bombLocation = "Bomb";

    private void Start()
    {
        Debug.Log("Started Bombing");
        StartCoroutine(DropBombs());
    }

    IEnumerator DropBombs()
    {
        while (true)
        {
            Debug.Log("Airstrike");
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            Vector3 spawn = new Vector3(Random.Range(-9f, 9f), 15, Random.Range(-9f, 9f));
            photonView.RPC("SpawnBomb", RpcTarget.All, spawn);
        }
    }

    [PunRPC]
    public void SpawnBomb(Vector3 spawnPoint)
    {
        PhotonNetwork.Instantiate(bombLocation, spawnPoint, Quaternion.Euler(-90, 0, 0));
    }

}
