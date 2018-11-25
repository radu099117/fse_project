using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;

public class ButtonsScript : MonoBehaviour
{

    public TMP_InputField register_email;
    public TMP_InputField register_password;
    public TMP_InputField register_retype_password;
    public TMP_InputField signIn_email;
    public TMP_InputField signIn_password;
    public TMP_InputField nickname;
    public TMP_Text infoMessage;
    public GameObject user;

    public void clickOnRegisterButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void clickOnBackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void clickOnQuitButton()
    {
        Application.Quit();
    }

    public void registerAccount()
    {
        string email = register_email.text;
        string password = register_password.text;
        string retype_password = register_retype_password.text;
        if(!string.IsNullOrEmpty(email)){
            if(!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(retype_password)){
                if(password.Equals(retype_password)){
                    if (password.Length >= 8)
                    {
                        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
                        {
                            if (task.IsCanceled)
                            {
                                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                                return;
                            }
                            if (task.IsFaulted)
                            {
                                infoMessage.faceColor = new Color32(255, 0, 0, 255);
                                AggregateException ex = task.Exception as AggregateException;
                                if (ex != null)
                                {
                                    Firebase.FirebaseException fbEx = null;
                                    foreach (Exception e in ex.InnerExceptions)
                                    {
                                        fbEx = e as Firebase.FirebaseException;
                                        if (fbEx != null)
                                            break;
                                    }

                                    if (fbEx != null)
                                    {
                                        infoMessage.text = fbEx.Message;
                                    }
                                }
                                return;
                            }

                            // Firebase user has been created.
                            Firebase.Auth.FirebaseUser newUser = task.Result;
                            infoMessage.faceColor = new Color32(0, 255, 0, 255);
                            infoMessage.text = "Account created Successfuly";
                        });
                    }
                    else{
                        infoMessage.faceColor = new Color32(255, 0, 0, 255);
                        infoMessage.text = "Password must be at least 8 characters long";
                    }
                }
                else{
                    infoMessage.faceColor = new Color32(255, 0, 0, 255);
                    infoMessage.text = "Password do not match!";
                }
            }else{
                infoMessage.faceColor = new Color32(255, 0, 0, 255);
                infoMessage.text = "Password fields cannot be empty!";
            }
        }else{
            infoMessage.faceColor = new Color32(255, 0, 0, 255);
            infoMessage.text = "Email cannot be empty!";
        }
    }


    public void signIn(){
        string email = signIn_email.text;
        string password = signIn_password.text;
        if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password)){
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            UserLogged.setFirebaseAuth(auth);
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    infoMessage.faceColor = new Color32(255, 0, 0, 255);
                    AggregateException ex = task.Exception as AggregateException;
                    if (ex != null)
                    {
                        Firebase.FirebaseException fbEx = null;
                        foreach (Exception e in ex.InnerExceptions)
                        {
                            fbEx = e as Firebase.FirebaseException;
                            if (fbEx != null)
                                break;
                        }

                        if (fbEx != null)
                        {
                            infoMessage.text = fbEx.Message;
                        }
                    }
                    return;
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;
                infoMessage.faceColor = new Color32(0, 255, 0, 255);
                infoMessage.text = "Sign In successful!";
                FirebaseUser currentUser = auth.CurrentUser;
                if(currentUser != null){
                    UserLogged.setLoggedUser(currentUser);
                    String nickname = currentUser.DisplayName;
                    if(string.IsNullOrEmpty(nickname)){
                        DontDestroyOnLoad(this.user);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
                    }
                    else{
                        DontDestroyOnLoad(this.user);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
                    }
                }
            });
        }
        else{
            infoMessage.faceColor = new Color32(255, 0, 0, 255);
            infoMessage.text = "Email or Password cannot be empty!";
        }
    }

    public void clickOnUpdateAvatar(){
        String nickname = this.nickname.text;
        FirebaseUser currentUser = UserLogged.getLoggedUser();
        if(!string.IsNullOrEmpty(nickname)){
            if(currentUser != null){
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    DisplayName = nickname,
                };
                currentUser.UpdateUserProfileAsync(profile).ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        infoMessage.faceColor = new Color32(255, 0, 0, 255);
                        AggregateException ex = task.Exception as AggregateException;
                        if (ex != null)
                        {
                            Firebase.FirebaseException fbEx = null;
                            foreach (Exception e in ex.InnerExceptions)
                            {
                                fbEx = e as Firebase.FirebaseException;
                                if (fbEx != null)
                                    break;
                            }

                            if (fbEx != null)
                            {
                                infoMessage.text = fbEx.Message;
                            }
                        }
                        return;
                    }

                    infoMessage.faceColor = new Color32(0, 255, 0, 255);
                    infoMessage.text = "User profile updated successfuly!";
                    GameObject gameObj = GameObject.Find("User");
                    DontDestroyOnLoad(gameObj);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                });
            }
        }else{
            infoMessage.faceColor = new Color32(255, 0, 0, 255);
            infoMessage.text = "Please enter a nickname!";
        }
    }

    public void signOutUser(){
        FirebaseAuth auth = UserLogged.getFirebaseAuth();
        auth.SignOut();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

}
