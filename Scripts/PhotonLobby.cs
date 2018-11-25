using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonLobby : MonoBehaviourPunCallbacks {

    public static PhotonLobby lobby;
    public GameObject playButton;
    public GameObject cancelButton;
    public TMP_Text info;
    RoomInfo[] rooms;

    private void Awake(){
        lobby = this;
    }
    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings();
	}

    public override void OnConnectedToMaster()
    {
        info.text = "Player has connected to Photon master server";
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnPlayButtonClicked(){
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnCancelButtonClicked(){
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join room but failed");
        createRoom();
    }

    private void createRoom(){
        int roomNumber = Random.Range(0, 100000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room:" + roomNumber, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a room but failed");
        createRoom();
    }

    public override void OnJoinedRoom()
    {
        info.text = "Player joined a room";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
