using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreLineRespawn : MonoBehaviour
{
    [SerializeField, Header("ScoreLineオブジェクト")] Object Line;
    [SerializeField, Header("スコアラインの間隔設定")] int interval = 300;
    [SerializeField, Header("Playerオブジェクト")] GameObject player;
    [SerializeField, Header("生成位置をどれだけ放すか")] float x = 20;
    [SerializeField, Header("Lineのy座標")] float y = 6;
    private int metor = 1;
    private int a;
    
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + x, y, 0);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            metor++;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            metor--;
        }

        if(metor % interval == 0)
        {
            Instantiate(Line, transform.position, Quaternion.identity);
        }
    }
}
