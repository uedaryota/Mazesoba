﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{

    [SerializeField, Header("敵を踏んだ時飛ぶ高さ")] public float boundHeight;
    [SerializeField, Header("エネミーが死んだときに出すパーティクルオブジェクト")] GameObject enemyDeathObj;
    //[SerializeField, Header("エネミーが死んだときに出すパーティクルオブジェクト")] GameObject enemyDeathObj2;
    //[SerializeField, Header("エネミーが死んだときに出すパーティクルオブジェクト")] GameObject enemyDeathObj3;
   
    //敵を踏んだ時のフラグ
    public bool playerjump;
    private bool isDeadFlag; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    ///// 確率判定
    //public static bool Probability(float fPercent)
    //{
    //    float fProbabilityRate = UnityEngine.Random.value * 100.0f;

    //    if (fPercent == 100.0f && fProbabilityRate == fPercent)
    //    {
    //        return true;
    //    }
    //    else if (fProbabilityRate < fPercent)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        if(playerjump==true)
        {
            isDeadFlag = true;
        }
        if(isDeadFlag==true)
        {
            Destroy(gameObject);
            Instantiate(enemyDeathObj, transform.position, Quaternion.identity);           
        }
    }
}
