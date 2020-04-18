using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyScript : MonoBehaviour
{
    public GameObject steamPrefab;//蒸気ゲージを上げるためのアイテム
    //public int EnemyHp;//エネミーのHp

    //プレイヤー追跡に必要な変数
    [SerializeField] GameObject player;
    private bool encountFlag;//プレイヤーを発見したか
    Vector3 playerPos;//プレイヤーの現在位置
    float Enemy_Move_Speed = 0f;//追跡速度
    //float ChaseTime = 180f;

    //死んだときになる音を鳴らせるオブジェクト
    [SerializeField, Header("サウンドオブジェクト")] GameObject EnemyDownSound_Obj;


    void Start()
    {
        //EnemyHp = 1;
        this.player = GameObject.Find("Player");
    }



    void Update()
    {
        if (encountFlag)
        {
            playerPos = this.player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, playerPos, Enemy_Move_Speed);
            //if (ChaseTime <= 0)
            //{
                
            //    ChaseTime = 0.5f;
            //}
            //else
            //{
            //    ChaseTime -= 1;
                
            //}
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RPlayer" || other.tag == "MainCamera")
        {
            Instantiate(EnemyDownSound_Obj, transform.position,transform.rotation);
            Steam();
            Destroy(gameObject);
            
        }

        if (other.tag == "Searchcol")
        {
            encountFlag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Searchcol")
        {
            encountFlag = false;
        }
    }

    private void Steam()
    {
        GameObject steam = Instantiate(steamPrefab, transform.position, Quaternion.identity)
      as GameObject;
        Rigidbody bulletRigidbody = steam.GetComponent<Rigidbody>();
    }

}
