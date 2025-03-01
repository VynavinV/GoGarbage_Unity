using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public RawImage Obj;
    public GameObject GameObject;

    public void deactivate()
    {
        GameObject.SetActive(false);
    }

    public void activate()
    {
        GameObject.SetActive(true);
    }

    public void SetTexture(Texture2D texture)
    {
        Obj.texture = texture;
    }


}
