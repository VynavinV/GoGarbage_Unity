using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro;

public class AddCoinsManager : MonoBehaviour
{
    private const string ENCRYPT_PREF_KEY = "EncryptKey";
    private string Setkey;
    public TextMeshProUGUI[] coinTexts;
    private void Start()
    {
        Updatecoins();
    }
    public void AddCoins(int coins)
    {
        if (PlayerPrefs.HasKey(ENCRYPT_PREF_KEY))
        {
            Setkey = PlayerPrefs.GetString(ENCRYPT_PREF_KEY);
            StartCoroutine(AddCoinsToServer(coins));
        }
        else
        {
            Debug.LogError("No encryption key found.");
        }
    }

    public void Updatecoins()
    {
        StartCoroutine(GetCoinsFromServer());
    }

    private IEnumerator AddCoinsToServer(int coins)
    {
        string json = $"{{\"key\":\"{Setkey}\",\"coins\":{coins}}}";
        Debug.Log("JSON payload: " + json);  // Add debug log
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest("http://99.234.77.217:5002/add-coins", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Coins added successfully!");
            StartCoroutine(GetCoinsFromServer());
        }
        else
        {
            Debug.LogError("Failed to add coins: " + request.error);
        }
    }

    private IEnumerator GetCoinsFromServer()
    {
        if (PlayerPrefs.HasKey(ENCRYPT_PREF_KEY))
        {
            Setkey = PlayerPrefs.GetString(ENCRYPT_PREF_KEY);
            string json = $"{{\"key\":\"{Setkey}\"}}";
            Debug.Log("JSON payload: " + json);  // Add debug log
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            UnityWebRequest request = new UnityWebRequest("http://99.234.77.217:5002/get-coins", "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<CoinResponse>(request.downloadHandler.text);
                UpdateCoinTexts(response.coins);
            }
            else
            {
                Debug.LogError("Failed to get coins: " + request.error);
            }
        }
        else
        {
            Debug.LogError("No encryption key found.");
        }
    }

    private void UpdateCoinTexts(int coins)
    {
        foreach (var text in coinTexts)
        {
            text.text = coins.ToString();
        }
    }

    [System.Serializable]
    private class CoinResponse
    {
        public int coins;
    }
}
