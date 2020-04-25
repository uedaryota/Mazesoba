using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreLineSprict : MonoBehaviour
{
    private bool ChangeEnd;
    private SpriteRenderer sprite;
    [SerializeField, Header("変更後の赤の濃度(0～1.0f)")] float Red = 1.0f;
    [SerializeField, Header("変更後の緑の濃度(0～1.0f)")] float Green = 1.0f;
    [SerializeField, Header("変更後の青の濃度(0～1.0f)")] float Blue = 1.0f;
    [SerializeField, Header("変更後の透明度(0～1.0f)")] float Alpha = 1.0f;
    [SerializeField, Header("Playerオブジェクト")] GameObject player;
    [SerializeField, Header("消えるまでの距離")] int interval = 300;
    public int LineScore = 0;
    private int metor = 0;
    private int count = 0;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            metor++;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            metor--;
        }
        
        if (metor % interval == 0)
        {
            count++;
            if (count == 3)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            sprite.color = new Color(Red, Green, Blue, Alpha);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
