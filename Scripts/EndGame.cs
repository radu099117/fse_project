using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class EndGame : MonoBehaviour{

    public static EndGame endGame;
    public Transform canvas;
    public TMP_Text text;
    // Update is called once per frame

    private void Start()
    {
        if (EndGame.endGame == null)
        {
            EndGame.endGame = this;
        }
        else
        {
            if (EndGame.endGame != this)
            {
                Destroy(EndGame.endGame.gameObject);
                EndGame.endGame = this;
            }
        }
        DontDestroyOnLoad(EndGame.endGame.gameObject);
    }

    public void showEndGameBg(bool gameWon){
        if(gameWon){
            text.text = "YOU WON!!";
        }else{
            text.text = "YOU LOST!!";
        }
        canvas.gameObject.SetActive(true);
    }

    public void quitGame(){
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(3);
    }
}
