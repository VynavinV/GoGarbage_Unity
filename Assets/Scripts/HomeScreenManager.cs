using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeScreenManager : MonoBehaviour
{
    public TrashpassManager trashpassManager;
    public string key;
    public TMP_InputField keyInput;
    public GameObject Loginscreen;
    public GameObject Homescreen;
    public TextMeshProUGUI xpCounter;

    private void FixedUpdate()
    {
        xpCounter.text = "XP: " + trashpassManager.xp;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("EncryptKey"))
        {
            Loginscreen.SetActive(false);
            Homescreen.SetActive(true);
        }
    }

    public void login()
    {
        key = keyInput.text;
        trashpassManager.LoginSys(key);
        trashpassManager.ValidateKey(key);
    }

    public void loginsucess()
    {
        Debug.Log("Login Sucessful");
        Loginscreen.SetActive(false);
        Homescreen.SetActive(true);
    }
}
