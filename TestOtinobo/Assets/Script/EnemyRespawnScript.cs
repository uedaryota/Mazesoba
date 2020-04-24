using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnScript : MonoBehaviour
{

    [SerializeField, Header("EnemyBoxを入れるリスト")] List<GameObject> enemyBoxes;
    [SerializeField, Header("Player")] GameObject player;//Ｙ軸を参照するため

    [Header("再配置するまでの最小時間と最大時間")] public float minRespawnTime;
    public float maxRespawnTime;

    //ランダム値を書くのするための値
    float rndRespawnTime;
    int rnd;

    bool isRespawnFlag;

    void Start()
    {
        rnd = Random.Range(0, 3);
        rndRespawnTime = Random.Range(minRespawnTime * 60, maxRespawnTime * 60);
        Instantiate(enemyBoxes[rnd], transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + 13f, transform.position.y, transform.position.z);
        //リスポーンして一定時間が経つと新しいEnemyBoxを生成するようにする
        if (!isRespawnFlag)
        {
            rndRespawnTime--;
            if (rndRespawnTime <= 0)
            {
                isRespawnFlag = true;
            }
        }
        else
        {
            //時間が経過したらまた時間を再設定し、Boxをリスポーンさせる
            rnd = Random.Range(0, 3);
            rndRespawnTime = Random.Range(minRespawnTime * 60, maxRespawnTime * 60);
            Instantiate(enemyBoxes[rnd], transform.position, Quaternion.identity);
            isRespawnFlag = false;
        }
    }
}
