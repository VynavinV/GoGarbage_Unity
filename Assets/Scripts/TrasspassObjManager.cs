using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrasspassObjManager : MonoBehaviour
{
    public GameObject[] items;
    public Transform petArea;
    public Transform center;
    public int xp;
    private const string XP_PREF_KEY = "PlayerXP";

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LoadXP()
    {
        if (PlayerPrefs.HasKey(XP_PREF_KEY))
        {
            xp = PlayerPrefs.GetInt(XP_PREF_KEY);
        }
        else
        {
            xp = 0;
        }
    }

}
