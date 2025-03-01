using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderManager : MonoBehaviour
{
    public TrashpassManager trashpassManager;
    // Start is called before the first frame update
    void Start()
    {
        trashpassManager = GameObject.Find("Game Manager").GetComponent<TrashpassManager>();
        trashpassManager.LoadXP();
    }
}
