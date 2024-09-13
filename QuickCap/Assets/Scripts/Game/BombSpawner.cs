using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BombSpawner : MonoBehaviour
{
    private string bombLocation = "Bomb";

    private void Start()
    {
        Debug.Log("Started Bombing");
        StartCoroutine("DropBombs");
    }

    IEnumerable DropBombs()
    {
        Debug.Log("Airstrike");
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        Vector3 spawn = new Vector3(Random.Range(-9f,9f),15, Random.Range(-9f, 9f));
        PhotonNetwork.Instantiate(bombLocation, spawn, Quaternion.identity);
    }

}
