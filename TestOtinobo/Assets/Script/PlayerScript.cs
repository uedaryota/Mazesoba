using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    ////playerの画像を指定するところ
    //[SerializeField, Header("上方向のプレイヤー")] Sprite UpPlayer;
    //[SerializeField, Header("下方向のプレイヤー")] Sprite DownPlayer;

    //メインカメラの当たり判定
    [SerializeField, Header("プレイヤーを追跡するカメラ")] Camera Maincamera;
    [SerializeField, Header("プレイヤーが死んだときに出すパーティクルオブジェクト")] GameObject playerDeathObj;

    #region//インスペクターで設定する
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("ジャンプする高さ")] public float jumpHeight;
    [Header("ジャンプする長さ")] public float jumpLimitTime;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    [Header("パラソルのバランス5.0～9.6の範囲で設定すること")] public float Parasol;
    #endregion
    public Text jumpText;


    //プレイヤーの画像変更に必要なもの
    SpriteRenderer MainSpriteRenderer;

    //取得した蒸気の数
    public float SteamPoint;

    public int score = 0;//スコアの追加(4/17)
    private int numScore = 0;//ジャンプのご褒美を与えるための500区切りのスコア
    public int AddPoint = 100;//普通のスコア加算
    public int HighPoint = 200;//スコア加算の高いポイント
    

    private Rigidbody2D rig2D;
    private Vector2 gravity;

    private bool damageFlag; //ダメージを受けているか判定
    private bool isDeadFlag; //死亡フラグ

    private bool isJump = false;
    private float jumpPos = 0.0f;
    private bool isOtherJump=false;
    private float jumpTime;
    private float otherJumpHeight = 0.0f;
    private bool isDown = false;
    private BoxCollider2D capcol = null;
    private bool isGround = false;
    private bool isHead = false;

    private bool IJump = false;
    private float IJumpC = 0;
    private float IJumpH = 0;


    void Start()
    {
        //MainSpriteRenderer = GetComponent<SpriteRenderer>();
        rig2D = GetComponent<Rigidbody2D>();
        capcol = GetComponent<BoxCollider2D>();
        FadeManager.FadeIn();
        jumpText.text = string.Format("ジャンプ残り {0} 回", IJumpC);
    }


    void Update()
    {
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0.0f, transform.rotation.w);
        float axis = Input.GetAxis("Horizontal");
        Vector2 velocity = rig2D.velocity;
        gravity = new Vector2(0.0f, -9.81f + Parasol);
        float ySpeed = GetYSpeed();
        //プレイヤーが動いていたらaxisの値に5かけて動かす
        if (axis != 0)
        {
            velocity.x = axis * 5;
        }
        rig2D.velocity = new Vector2(velocity.x,ySpeed);

        //ジャンプ(Spaceキー)が押されたらアイテムジャンプを使用する
        if (IJump && Input.GetButtonDown("Jump"))
        {

            otherJumpHeight = IJumpH;    //踏んづけたものから跳ねる高さを取得する          
            jumpPos = transform.position.y; //ジャンプした位置を記録する 
            isOtherJump = true;
            isJump = false;
            jumpTime = 0.0f;
            //Debug.Log("ジャンプしたよ");
            if (IJumpC > 1)
            {
                IJumpC--;
                jumpText.text = string.Format("ジャンプ残り {0} 回", IJumpC);
                return;
            }
            else
            {
                IJump = false;
            }
        }


        ////ダメージを受けたら一定時間無敵にして点滅させる(ダメージ関連を追加することは無いと思うけど念のため残してます)
        //if (damageFlag)
        //{
        //    float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
        //    MainSpriteRenderer.color = new Color(1f, 1f, 1f, level);
        //}

        //死亡時のフラグ(エフェクトなど追々追加)//ごめん追加してしまいました。期限に間に合うなら手直し全然オケです0422佐藤
        if(isDeadFlag)
        {
            gameObject.SetActive(false);
            rig2D.velocity = new Vector2(0, 0);
            //Destroy(gameObject);
            //SceneManager.LoadScene(0);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            Camera.main.gameObject.GetComponent<CameraScritpt>().Shake();
            Instantiate(playerDeathObj, transform.position,Quaternion.identity);
            //プレイヤー死亡
            isDeadFlag = true;
        }

        if (other.collider.tag == "item")
        {
            ItemJump o = other.gameObject.GetComponent<ItemJump>();
            if (o != null)
            {
                IJumpH = o.boundHeight;    //踏んづけたものから跳ねる高さを取得する
                IJumpC += o.boundCount;
                IJump = true;
                o.playerjump = true;        //踏んづけたものに対して踏んづけた事を通知する
                jumpText.text = string.Format("ジャンプ残り {0} 回", IJumpC);
            }
            else
            {
                Debug.Log("ObjectCollisionが付いてないよ!");
            }
        }
    　　
        if (other.collider.tag == "Enemy" || other.collider.tag == "HighEnemy")
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));
            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;
            //Debug.Log("接触したよ");
            foreach (ContactPoint2D p in other.contacts)
            {
                if (p.point.y < judgePos)
                {
                    EnemyJump o = other.gameObject.GetComponent<EnemyJump>();
                    if (o != null)
                    {
                        otherJumpHeight = o.boundHeight;    //踏んづけたものから跳ねる高さを取得する
                        o.playerjump = true;        //踏んづけたものに対して踏んづけた事を通知する
                        jumpPos = transform.position.y; //ジャンプした位置を記録する 
                        isOtherJump = true;
                        isJump = false;
                        jumpTime = 0.0f;
                        //Debug.Log("ジャンプしたよ");
                        Camera.main.gameObject.GetComponent<CameraScritpt>().Shake();
                        if (other.collider.tag == "Enemy")
                        {
                            score += AddPoint / 2;//スコアを足す(4/17)
                            numScore += AddPoint / 2;
                        }
                        else
                        {
                            score += HighPoint / 2;
                            numScore += HighPoint / 2;
                        }
                    }
                    else
                    {
                        Debug.Log("ObjectCollisionが付いてないよ!");
                    }
                }
                else
                {
                    isDown = true;
                    break;
                }
            }
           
        }
        //300点取る度にジャンプを一回増やす
        if(numScore >= 300)
        {
            IJumpC += 1;
            numScore = 0;
            jumpText.text = string.Format("ジャンプ残り {0} 回", IJumpC);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //OnDamegeEffect();
        
        ////ゴールと接触したら
        //if (other.tag == "Goal")
        //{
        //    Invoke("Next", 0f);
        //}

    }

    #region　ダメージの時に点滅させる処理(今は使ってない)
    ///// <summary>
    ///// ダメージを受けた時に呼び出す処理
    ///// </summary>
    //void OnDamegeEffect()
    //{
    //    damageFlag = true;

    //    StartCoroutine(WaitForit());//WaitForitで指定した秒分待ってから呼び出す
    //}


    //IEnumerator WaitForit()
    //{
    //    yield return new WaitForSeconds(1);//1秒間処理を止める

    //    //ダメージを受けてないと判断し、点滅をやめる
    //    damageFlag = false;
    //    MainSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

    //}
    #endregion

    /// <summary>
    /// シーン変更時にフェードをかける処理
    /// </summary>
    void Next()
    {
        FadeManager.FadeOut(1);
    }
    private float GetYSpeed()
    {

        float ySpeed = gravity.y;

        //何かを踏んだ際のジャンプ
        if (isOtherJump)
        {
            if (jumpPos + otherJumpHeight > transform.position.y && jumpTime < jumpLimitTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;

            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
     

       
        return ySpeed;
    }
}
