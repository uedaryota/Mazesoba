using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlockRespawnScript : MonoBehaviour
{

    [SerializeField, Header("ColorBlockを入れるリスト")] List<GameObject> ColorBlockBoxes;
    [SerializeField, Header("Player")] GameObject player;//Ｙ軸を参照するため

    [Header("再配置するまでの最小時間と最大時間")] public float minRespawnTime = 1.0f;
    public float maxRespawnTime = 4.0f;

    //ランダム値を書くのするための値
    float rndRespawnTime;
    //int rnd;

    bool isRespawnFlag;

    //Wave処理
    public WaveScript Wscript;
    private int Enemywave;
    int rnd;//保管用
    [Header("Wave毎のリスポーン頻度の上昇、減少の設定")]
    public float MaxTimeUp = 0.5f;
    public float MaxTimeDown = 0;
    public float MinTimeUp = 0.5f;
    public float MinTimeDown = 0;
    private float MaxTime;
    private float MinTime;


    void Start()
    {
        rnd = Random.Range(0, 1);
        rndRespawnTime = Random.Range(minRespawnTime * 60, maxRespawnTime * 60);
        Instantiate(ColorBlockBoxes[rnd], transform.position, Quaternion.identity);
        player = GameObject.Find("Player");
        Wscript = GameObject.Find("WaveText").GetComponent<WaveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemywave = Wscript.wave;

        transform.position = new Vector3(player.transform.position.x + 13f, transform.position.y, transform.position.z);

        switch (Enemywave)
        {
            case 1:
                MaxTime = maxRespawnTime + MaxTimeUp - MaxTimeDown;
                MinTime = minRespawnTime + MinTimeUp - MinTimeDown;
                rnd = Random.Range(0, 1);
                Respawn();
                break;

            case 2:
                MaxTime = maxRespawnTime + MaxTimeUp * Enemywave - MaxTimeDown * Enemywave;
                MinTime = minRespawnTime + MinTimeUp * Enemywave - MinTimeDown * Enemywave;
                rnd = Random.Range(1, 2);
                Respawn();
                break;

            case 3:
                MaxTime = maxRespawnTime + MaxTimeUp * Enemywave - MaxTimeDown * Enemywave;
                MinTime = minRespawnTime + MinTimeUp * Enemywave - MinTimeDown * Enemywave;
                rnd = Random.Range(2, 3);
                Respawn();
                break;

            case 4:
                MaxTime = maxRespawnTime + MaxTimeUp * Enemywave - MaxTimeDown * Enemywave;
                MinTime = minRespawnTime + MinTimeUp * Enemywave - MinTimeDown * Enemywave;
                rnd = Random.Range(0, 2);
                Respawn();
                break;

            case 5:
                MaxTime = maxRespawnTime + MaxTimeUp * Enemywave - MaxTimeDown * Enemywave;
                MinTime = minRespawnTime + MinTimeUp * Enemywave - MinTimeDown * Enemywave;
                rnd = Random.Range(0, 3);
                Respawn();
                break;

            default:
                MaxTime = maxRespawnTime + MaxTimeUp * 6 - MaxTimeDown * 6;
                MinTime = minRespawnTime + MinTimeUp * 6 - MinTimeDown * 6;
                rnd = Random.Range(1, 3);
                Respawn();
                break;
        }
    }

    void Respawn()
    {
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
            rndRespawnTime = Random.Range(MinTime * 60, MaxTime * 60);
            Instantiate(ColorBlockBoxes[rnd], transform.position, Quaternion.identity);
            isRespawnFlag = false;
        }
    }
}
