using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks {

    public static PhotonRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    private Player[] players;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playerInGame;
    public TMP_Text info;

    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayer;
    private float timeToStart;

    public void Awake()
    {
        if(PhotonRoom.room == null){
            PhotonRoom.room = this;
        }else{
            if(PhotonRoom.room != this){
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(PhotonRoom.room.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        info.text = "Player joined a room";
        players = PhotonNetwork.PlayerList;
        playersInRoom = players.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        if (MultiplayerSettings.multiplayerSettings.delayStart)
        {
            info.text = "Players in room " + playersInRoom + "/" + MultiplayerSettings.multiplayerSettings.maxPlayers;
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }else{
            startGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        info.text = "A new player joined";
        players = PhotonNetwork.PlayerList;
        playersInRoom++;
        if(MultiplayerSettings.multiplayerSettings.delayStart){
            info.text = "Players in room " + playersInRoom + "/" + MultiplayerSettings.multiplayerSettings.maxPlayers;
            if(playersInRoom > 1){
                readyToCount = true;
            }
            if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers){
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    void Start(){
        PV = GetComponent < PhotonView >();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayer = 2;
        timeToStart = startingTime;
    }

    void Update(){
        if(MultiplayerSettings.multiplayerSettings.delayStart){
            if(playersInRoom == 1){
                restartTimer();
            }
            if(!isGameLoaded){
                if(readyToStart){
                    atMaxPlayer -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayer;
                    timeToStart = atMaxPlayer;
                }else if(readyToCount){
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                Debug.Log("Time to start = " + timeToStart);
                if(timeToStart <= 0){
                    startGame();
                }
            }
        }
    }

    void startGame(){
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        if(MultiplayerSettings.multiplayerSettings.delayStart){
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
    }

    void restartTimer(){
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayer = 2;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode){
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene){
            isGameLoaded = true;
            if(MultiplayerSettings.multiplayerSettings.delayStart){
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }else{
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene(){
        playerInGame++;
        if(playerInGame == PhotonNetwork.PlayerList.Length){
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer(){
        if(myNumberInRoom == 1){
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), new Vector3(6, 1, -12), Quaternion.identity, 0);
        }else{
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), new Vector3(-6, 1, -12), Quaternion.identity, 0);
        }

    }

}
