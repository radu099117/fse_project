using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour {

    private PhotonView PV;
    private GameObject camera;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        camera = GameObject.FindWithTag("MainCamera");
        PV = GetComponent<PhotonView>();
        if(PV.IsMine){
            if (transform.position.x > 0)
            {
                camera.transform.SetPositionAndRotation(new Vector3(6, 10, -20), camera.transform.rotation);
            }
            else
            {
                camera.transform.SetPositionAndRotation(new Vector3(-6, 10, -20), camera.transform.rotation);
            }
        }

        offset = camera.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(PV.IsMine){
            moveCamera();
        }
    }

    void moveCamera()
    {
        camera.transform.position = transform.position + offset;
    }


}
