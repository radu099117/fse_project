using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float x, y, z, x1, y1, z1;
    private Vector3 pointA;
    private Vector3 pointB;

    private void Start()
    {
        pointA = new Vector3(x, y, z);
        pointB = new Vector3(x1, y1, z1);
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        movement();
	}

    void movement(){
        if(!this.gameObject.CompareTag("Collectable"))
            transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time, 1));
    }
}
