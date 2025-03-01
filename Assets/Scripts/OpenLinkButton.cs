using UnityEngine;
using UnityEngine.UI;

public class OpenLinkButton : MonoBehaviour
{
    public void OpenLink(string url)
    {
        // Ensure the URL has the proper protocol (http:// or https://)
        Application.OpenURL(url);
    }
}
