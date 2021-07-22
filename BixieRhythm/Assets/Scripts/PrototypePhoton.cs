using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class PrototypePhoton : MonoBehaviourPunCallbacks
{
    public Button Int;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Connect();
    }

    private void Initialize()
    {
        //_leaveRoomButton.interactable = false;
    }

    private void Connect()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 5000);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        PhotonNetwork.JoinRandomOrCreateRoom();
        //PhotonNetwork.JoinOrCreateRoom("Test");
        //if(Photon.OnJoi)
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient){
            Int.interactable = true;
        }
        //_statusField.text = "Joined " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        //_leaveRoomButton.interactable = true;

        Debug.Log("Current Players: "+ PhotonNetwork.CurrentRoom.PlayerCount);
        //playerCount.GetComponent<NeverDestroy>().playerIndex = PhotonNetwork.CurrentRoom.PlayerCount;
        //NeverDestroy index = playerCount.GetComponent<NeverDestroy>();

    }

    public override void OnLeftRoom()
    {
        Debug.Log("LeftRoom");

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {
        Debug.Log("Player Entered Room");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        Debug.Log("Player Left Room");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("Test");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartGamePressed()
    {
        PhotonNetwork.LoadLevel("R1");
    }

}
