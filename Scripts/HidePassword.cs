using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HidePassword : MonoBehaviour {

    public TMP_InputField passwordField1;
    public TMP_InputField passwordField2;

    // Use this for initialization
    void Start () {
        passwordField1.inputType = TMP_InputField.InputType.Password;
        if (passwordField2 != null)
            passwordField2.inputType = TMP_InputField.InputType.Password;
	}

}
