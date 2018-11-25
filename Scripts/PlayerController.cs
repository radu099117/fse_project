using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour {

    private PhotonView PV;
    private CharacterController CC;
    public float speed;
    private Vector3 initPosition;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        CC = GetComponent<CharacterController>();
        initPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if(PV.IsMine){
            movement();
        }
	}

    private void movement(){
        if(SystemInfo.deviceType == DeviceType.Desktop){
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb.AddForce(movement * speed);
        }else{
            Vector3 movement = new Vector3(Input.acceleration.x, 0.0f, Input.acceleration.y);
            rb.AddForce(movement * speed);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Trap")){
            dieAndRespawn();
        }
        if(other.gameObject.CompareTag("Collectable")){
            other.gameObject.SetActive(false);
            if (PV.IsMine)
            {
                EndGame.endGame.showEndGameBg(true);
            }
            else
            {
                EndGame.endGame.showEndGameBg(false);
            }
        }
    }

    private void dieAndRespawn(){
        this.gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        this.transform.position = initPosition;
        this.gameObject.SetActive(true);
    }
}
