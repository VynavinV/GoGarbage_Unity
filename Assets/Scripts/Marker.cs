using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class Marker : MonoBehaviour
{
    public string imageurl;
    public ObjectManager image;
    public Transform player; // Reference to the player or camera
    private bool imageLoaded = false;
    public GameObject imgobj;

    public void SetImage(string url)
    {
        imageurl = url;
        Debug.Log("Image URL set to: " + imageurl);
    }

    private void Start()
    {
        // Find the raw image in the scene
        imgobj = GameObject.Find("Global");
        image = imgobj.GetComponent<ObjectManager>();
        // Find the player or camera in the scene
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        
        
        // Check the distance between the marker and the player or camera
        if (!imageLoaded && Vector3.Distance(transform.position, player.position) <= 30f)
        {
            
            image.activate();

            StartCoroutine(LoadImage(imageurl));
            imageLoaded = true;
        }

        //if too far, deactivate the image
        if (imageLoaded && Vector3.Distance(transform.position, player.position) > 30f)
        {
            image.deactivate();
            imageLoaded = false;
        }
    }

    private IEnumerator LoadImage(string url)
    {
        Debug.Log("Loading image from: " + url);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://99.234.77.217:5002" + url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            image.SetTexture(texture);
        }
        else
        {
            Debug.LogError($"Failed to load image from {url}: {www.error}");
        }
    }
}
