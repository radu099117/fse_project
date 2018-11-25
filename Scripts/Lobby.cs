using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;

public class Lobby : MonoBehaviour {

    public TMP_Text profileName;
    private FirebaseUser user;

	// Use this for initialization
	void Start () {
        user = UserLogged.getLoggedUser();
        profileName.text = user.DisplayName;
	}
}
