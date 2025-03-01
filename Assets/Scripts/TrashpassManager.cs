using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;

public class TrashpassManager : MonoBehaviour
{
    public int xp;
    public int id;
    public string Setkey;
    private const string ENCRYPT_PREF_KEY = "EncryptKey";
    public HomeScreenManager homeScreenManager;
    public Slider[] xpsliders;

    private void Start()
    {
        LoadSys();
        LoadXP();
    }
    public void AddXP(int amount)
    {
        xp += amount;
        SaveXP();
    }

    public void SaveXP()
    {
        StartCoroutine(SaveXPToServer());
    }

    private IEnumerator SaveXPToServer()
    {
        if (PlayerPrefs.HasKey(ENCRYPT_PREF_KEY))
        {
            Setkey = PlayerPrefs.GetString(ENCRYPT_PREF_KEY);
            string json = JsonUtility.ToJson(new { key = Setkey, xp = xp });
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            UnityWebRequest request = new UnityWebRequest("http://99.234.77.217:5002/update-xp", "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("XP updated successfully!");
            }
            else
            {
                Debug.LogError("Failed to update XP: " + request.error);
            }
        }
        else
        {
            Debug.LogError("No encryption key found.");
        }
    }

    public void LoadXP()
    {
        Debug.Log("Loading XP with key: " + Setkey);  
        xpsliders = FindObjectsByType<Slider>(FindObjectsSortMode.None);
        StartCoroutine(LoadXPFromServer());
    }

    private IEnumerator LoadXPFromServer()
    {
        if (PlayerPrefs.HasKey(ENCRYPT_PREF_KEY))
        {
            Setkey = PlayerPrefs.GetString(ENCRYPT_PREF_KEY);
            KeyData keyData = new KeyData { key = Setkey };
            Debug.Log("Key data: " + keyData.key);
            string json = JsonUtility.ToJson(keyData);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            Debug.Log("Sending key for validation: " + Setkey);
            Debug.Log("JSON payload: " + json);


            UnityWebRequest request = new UnityWebRequest("http://99.234.77.217:5002/get-xp", "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<XPResponse>(request.downloadHandler.text);
                xp = response.xp;
                Debug.Log("XP loaded successfully!");
                for (int i = 0; i < xpsliders.Length; i++)
                {
                    if (xpsliders[i].maxValue > xp)
                    {
                        xpsliders[i].value = xp;
                    }
                    else
                    {
                        xpsliders[i].value = xpsliders[i].maxValue;
                    }

                }
            }
            else
            {
                Debug.LogError("Failed to load XP: " + request.error);
            }
        }
        else
        {
            Debug.LogError("No encryption key found.");
        }
    }

    public void LoadSys()
    {
        if (PlayerPrefs.HasKey(ENCRYPT_PREF_KEY))
        {
            Setkey = PlayerPrefs.GetString(ENCRYPT_PREF_KEY);
        }
        else
        {
            Setkey = " ";
        }
    }

    public void LoginSys(string key)
    {
        if (PlayerPrefs.HasKey(ENCRYPT_PREF_KEY))
        {
            Setkey = key;
            PlayerPrefs.SetString(ENCRYPT_PREF_KEY, key);
            PlayerPrefs.Save();
            Debug.Log("key reset");
            LoadXP();
        }
        else
        {
            Setkey = key;
            PlayerPrefs.SetString(ENCRYPT_PREF_KEY, key);
            PlayerPrefs.Save();
            Debug.Log("key set");
            LoadXP();
        }
    }

    public void ValidateKey(string key)
    {
        StartCoroutine(ValidateKeyCoroutine(key));
    }

    private IEnumerator ValidateKeyCoroutine(string key)
    {
        KeyData keyData = new KeyData { key = key };
        Debug.Log("Key data: " + keyData.key);
        string json = JsonUtility.ToJson(keyData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        Debug.Log("Sending key for validation: " + key);
        Debug.Log("JSON payload: " + json);

        UnityWebRequest request = new UnityWebRequest("http://99.234.77.217:5002/validate-key", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<KeyValidationResponse>(request.downloadHandler.text);
            if (response.valid)
            {
                homeScreenManager.loginsucess();
            }
            else
            {
                Debug.LogError("Invalid key.");
            }
        }
        else
        {
            Debug.LogError("Failed to validate key: " + request.error);
            Debug.LogError("Response Code: " + request.responseCode);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
    }

    [System.Serializable]
    private class XPResponse
    {
        public int xp;
    }

    [System.Serializable]
    private class KeyValidationResponse
    {
        public bool valid;
    }

    [System.Serializable]
    private class KeyData
    {
        public string key;
    }
}