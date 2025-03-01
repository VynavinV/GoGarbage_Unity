using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;

public class ButtonSwitcher : MonoBehaviour
{
    public Slider Slider;
    public Button Button;
    public int id;
    public bool awarded;
    public AddCoinsManager addCoinsManager;
    public int coins;
    void Start()
    {
        CheckRewardNoClaim();
    }

    private void Update()
    {
        if (Slider.value == Slider.maxValue && awarded == false)
        {
            Button.interactable = true;
            Slider.gameObject.SetActive(false);
            
        }
        else
        {
            Slider.gameObject.SetActive(true);

        }
    }

    public void CheckReward()
    {
        string key = PlayerPrefs.GetString("EncryptKey");
        int rewardId = id;

        StartCoroutine(CheckRewardCoroutine(key, rewardId));
    }

    private IEnumerator CheckRewardCoroutine(string key, int rewardId)
    {
        Debug.Log("Checking reward");
        KeyData keyData = new KeyData { key = key, reward_id = rewardId };
        string jsonData = JsonUtility.ToJson(keyData);

        using (UnityWebRequest www = new UnityWebRequest("http://99.234.77.217:5002/check-reward", "POST"))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                KeyValidationResponse response = JsonUtility.FromJson<KeyValidationResponse>(www.downloadHandler.text);
                if (response.valid)
                {
                    Debug.Log("Already there");
                    awarded = true;
                    Button.interactable = false;
                }
                else
                {
                    Debug.Log("Awarded");
                    awarded = true;
                    Button.interactable = false;
                    addCoinsManager.AddCoins(coins);
                    StartCoroutine(AddRewardToServer(key, rewardId));

                }
            }
        }
    }

    public void CheckRewardNoClaim()
    {
        string key = PlayerPrefs.GetString("EncryptKey");
        int rewardId = id;

        StartCoroutine(CheckRewardNoClaimCoroutine(key, rewardId));
    }

    private IEnumerator CheckRewardNoClaimCoroutine(string key, int rewardId)
    {
        Debug.Log("Checking reward");
        KeyData keyData = new KeyData { key = key, reward_id = rewardId };
        string jsonData = JsonUtility.ToJson(keyData);

        using (UnityWebRequest www = new UnityWebRequest("http://99.234.77.217:5002/check-reward", "POST"))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                KeyValidationResponse response = JsonUtility.FromJson<KeyValidationResponse>(www.downloadHandler.text);
                if (response.valid)
                {
                    Debug.Log("Already there");
                    awarded = true;
                    Button.interactable = false;
                }
                else
                {
                    Debug.Log("not claimed");
                    awarded = false;
                    //Button.interactable = false;
                    //StartCoroutine(AddRewardToServer(key, rewardId));
                }
            }
        }
    }

    private IEnumerator AddRewardToServer(string key, int rewardId)
    {
        RewardData rewardData = new RewardData { key = key, reward_id = rewardId };
        string jsonData = JsonUtility.ToJson(rewardData);

        using (UnityWebRequest www = new UnityWebRequest("http://99.234.77.217:5002/add-reward", "POST"))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Reward added to server");
            }
        }
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
        public int reward_id;
    }

    [System.Serializable]
    private class XPResponse
    {
        public int xp;
    }

    [System.Serializable]
    private class RewardData
    {
        public string key;
        public int reward_id;
    }
}