using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameUI : MonoBehaviour
{
    //PDF bug changed data type to PlayerUIContainer class
    public PlayerUIContainer[] playerContainers;
    public TextMeshProUGUI winText;


    //instance - one existing version of the class
    public static GameUI instance;

    private void Awake()
    {
        //instance is an instance of this script, What's the point of this?
        instance = this;
    }

    private void Start()
    {
        //why the function
        InitializePlayerUI();
    }

    private void InitializePlayerUI()
    {
        //loops through all of the players
        for(int x = 0; x < playerContainers.Length; x++)
        {
            PlayerUIContainer container = playerContainers[x]; //loops through each container in list, consider for each loop

            //enables UI based on player count
            if (x < PhotonNetwork.PlayerList.Length)
            {
                container.obj.SetActive(true); //Makes box visible
                container.nameText.text = PhotonNetwork.PlayerList[x].NickName;
                container.fillText.text = PhotonNetwork.PlayerList[x].NickName;//Updates TMP text
                //container.hatTimeSlider.maxValue = GameManager.instance.timeToWin; //Makes sure the box fills properly
            }
            else
            {
                container.obj.SetActive(false);
            }
        }
    }

    private void Update()
    {
        //why is this a function
        UpdatePlayerUI();
    }

    private void UpdatePlayerUI()
    {
        //for every player PhotonNetwork.PlayerList is the apropriate length
        for (int x = 0; x < PhotonNetwork.PlayerList.Length ; x++)
        {
            //Debug.Log(x);
            //Debug.Log("length: " + playerContainers.Length);
            //only updates for real player
            if (GameManager.instance.players[x] != null)
            {
                //updates the slider every frame based on the playercontrollers hat time value
                //playerContainers[x].hatTimeSlider.value = GameManager.instance.players[x].curHatTime;
                playerContainers[x].mask.fillAmount = (GameManager.instance.players[x].curHatTime / GameManager.instance.timeToWin); // reveals the colored text based on the ratio of cur time to win time
            }
        }
    }

    public void SetWinText (string winnerName)
    {
        winText.gameObject.SetActive(true);
        winText.text = winnerName + " wins!";
    }
}

[System.Serializable]
public class PlayerUIContainer
{
    public GameObject obj;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI fillText;
    //public Slider hatTimeSlider;
    public Image mask;
}
