using System;
using Firebase;
using Firebase.Auth;
using UnityEngine;
public class UserLogged : MonoBehaviour{
    private static FirebaseUser loggedUser;
    private static FirebaseAuth auth;

    public static void setLoggedUser(FirebaseUser user){
        loggedUser = user;
    }

    public static FirebaseUser getLoggedUser(){
        return loggedUser;
    }

    public static void setFirebaseAuth(FirebaseAuth authentification){
        auth = authentification;
    }

    public static FirebaseAuth getFirebaseAuth(){
        return auth;
    }
}
