using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GivefreeCoin : MonoBehaviour
{
    public int coinAmount = 0;
    public TextMeshProUGUI coinText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin()
    {
        coinAmount += 1;
        coinText.text = coinAmount.ToString();
    }
}
