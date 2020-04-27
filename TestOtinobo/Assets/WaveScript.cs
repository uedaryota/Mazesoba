using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveScript : MonoBehaviour
{
    public PlayerScript player;
    public int number;
    private int score;
    private int wave = 1;
    Text text;

    void Start()
    {
        text = GameObject.Find("WaveText").GetComponent<Text>();//テキストを探す
    }
    
    void Update()
    {
        score = player.scoreline;
        if (score % number == 0)
        {
            wave = score / number + 1;
        }
       text.text = wave + "Wave";
    }
}
