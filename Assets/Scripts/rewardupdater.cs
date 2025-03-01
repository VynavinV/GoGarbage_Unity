using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rewardupdater : MonoBehaviour
{
    public ButtonSwitcher[] buttonSwitchers;
    // Start is called before the first frame update
    void Start()
    {
        //find all buttonswitchers
        buttonSwitchers = FindObjectsByType<ButtonSwitcher>(FindObjectsSortMode.None);

    }

    public void updaterewards()
    {
        foreach (ButtonSwitcher buttonSwitcher in buttonSwitchers)
        {
            buttonSwitcher.awarded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
