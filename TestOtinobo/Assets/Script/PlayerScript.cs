using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //playerの画像を指定するところ
    [SerializeField, Header("上方向のプレイヤー")] Sprite UpPlayer;
    [SerializeField, Header("下方向のプレイヤー")] Sprite DownPlayer;

    //メインカメラの当たり判定
    [SerializeField, Header("プレイヤーを追跡するカメラ")] Camera Maincamera;


    #region//インスペクターで設定する
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("ジャンプする高さ")] public float jumpHeight;
    [Header("ジャンプする長さ")] public float jumpLimitTime;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    [Header("パラソルのバランス5.0～9.6の範囲で設定すること")] public float Parasol;
    #endregion



    //プレイヤーのHPとそれを表示するテキストの指定
    [SerializeField] Text HPtext;
    public int Hp;
    //プレイヤーの画像変更に必要なもの
    SpriteRenderer MainSpriteRenderer;

    //取得した蒸気の数
    public float SteamPoint;
    //蒸気を表示するテキスト
    [SerializeField, Header("蒸気テキスト")] Text SteamText;
    //テキスト用SteamPoint
    int TextSteampoint;
    //大雨降っているかどうか
    private bool steamFallflag;
    public int score = 0;//スコアの追加(4/17)
    public int AddPoint = 100;//普通のスコア加算
    public int HighPoint = 200;//スコア加算の高いポイント
    //大雨が降っている時間
    //private int steamTime = 120;


    //潤いゲージとそれを表示するテキストの指定
    [SerializeField] Text Utext;
    public int Rain;
    private float AddRain;
    //潤いゲージ上昇速度
    public float Rainup = 0.1f;
    Slider _RainSlider;

    private Rigidbody2D rig2D;
    private Vector2 gravity;

    private bool parasolFlag;　//傘が開いているか閉じているか判定
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

    private void Awake()
    {
        rig2D = GetComponent<Rigidbody2D>();
        //潤いゲージの取得
        _RainSlider = GameObject.Find("RainSlider").GetComponent<Slider>();
    }

    void Start()
    {
        MainSpriteRenderer = GetComponent<SpriteRenderer>();

        HPtext.text = string.Format("HP: {0}", Hp);
        Utext.text = string.Format("潤い: {0}", Rain);
        SteamText.text = string.Format("\n{0}/3",TextSteampoint);
        capcol = GetComponent<BoxCollider2D>();
        FadeManager.FadeIn();
    }

    private void FixedUpdate()
    {
        //傘が閉じていたら重力を下に、開いていたら重力を上にする
        if (!parasolFlag)
        {
            gravity = new Vector2(0.0f, -9.81f + Parasol);
            MainSpriteRenderer.sprite = DownPlayer;
        }
        else
        {
            gravity = new Vector2(0.0f, 9.81f * 1.5f);
            MainSpriteRenderer.sprite = UpPlayer;
        }
        rig2D.AddForce(gravity);
    }



    void Update()
    {
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0.0f, transform.rotation.w);
        float axis = Input.GetAxis("Horizontal");
        Vector2 velocity = rig2D.velocity;
        float ySpeed = GetYSpeed();
        //プレイヤーが動いていたらaxisの値に5かけて動かす
        if (axis != 0)
        {
            velocity.x = axis * 5;
        }
        rig2D.velocity = new Vector2(velocity.x,ySpeed);
        
        //ジャンプ(Spaceキー)が押されたら傘のフラグを変える
        //if (Input.GetButtonDown("Jump"))

        //{
        //    if (parasolFlag)
        //    {
        //        parasolFlag = false;
        //    }
        //    else if (!parasolFlag)
        //    {
        //        parasolFlag = true;
        //    }
        //}

        //ダメージを受けたら一定時間無敵にして点滅させる
        if (damageFlag)
        {
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            MainSpriteRenderer.color = new Color(1f, 1f, 1f, level);
        }

        //死亡時のフラグ(エフェクトなど追々追加)
        if(isDeadFlag)
        {
            SceneManager.LoadScene(0);
        }

        //HPがゼロになったらやり直し
        if (Hp <= 0)
        {
            SceneManager.LoadScene(0);
        }


        //大雨発生条件
        if (SteamPoint >= 60)
        {
            steamFallflag = true;
        }

        //蒸気が最大まで溜まったら一定時間大雨を降らす
        if (steamFallflag)
        {
            SteamText.text = string.Format("FEVER!!");
            Maincamera.GetComponent<Collider2D>().enabled = true;
            SteamPoint -= 0.5f;
            //ゼロになったらお終い
            if (SteamPoint <= 0)
            {
                SteamPoint = 0;
                TextSteampoint = 0;
                SteamText.text = string.Format("\n{0}/3", TextSteampoint);
                steamFallflag = false;
            }
        }
        else
        {
            Maincamera.GetComponent<Collider2D>().enabled = false;
        }


        //敵を倒せる条件を満たしたらタグを変更する
        if(Rain >= 15)
        {
            tag = "RPlayer";
        }
        else
        {
            tag = "Player";
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            //プレイヤー死亡
            isDeadFlag = true;
        }

        if(other.gameObject.tag == "DGround")
        {
            OnDamegeEffect();
            Hp--;
            HPtext.text = string.Format("HP: {0}", Hp);
        }
        
        if (other.collider.tag == "Enemy" || other.collider.tag == "HighEnemy")
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));
            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;
            Debug.Log("接触したよ");
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
                        Debug.Log("ジャンプしたよ");
                        Camera.main.gameObject.GetComponent<CameraScritpt>().Shake();
                        if(other.collider.tag == "Enemy")
                        {
                            score += AddPoint / 2;//スコアを足す(4/17)
                        }
                        else
                        {
                            score += HighPoint / 2;
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //当たった相手のタグがEnemyかつダメージを受けていない状態なら、無敵にしてHpを減らす
        if (other.tag == "Enemy" && !damageFlag)
        {
            Debug.Log("HIr");
            //潤いが一定以上ならタグを変更して敵を倒せるようにする
            if (tag == "RPlayer")
            {
                Rain -= 20;
                AddRain = Rain;
                //端数を無くすため
                if (Rain < 0)
                {
                    Rain = 0;
                }
                Utext.text = string.Format("潤い: {0}", Rain);
                _RainSlider.value = Rain;
            }
            else
            {
                OnDamegeEffect();
                Hp--;
                HPtext.text = string.Format("HP: {0}", Hp);
            }
        }


        //ゴールと接触したら
        if (other.tag == "Goal")
        {
            Invoke("Next", 0f);
        }
        //蒸気に当たったら蒸気ポイントに加算する
        if (other.tag == "item" && !steamFallflag)
        {
            SteamPoint += 20;
            if (SteamPoint > 60)
            {
                SteamPoint = 60;
            }

            TextSteampoint = (int)SteamPoint / 20;
            SteamText.text = string.Format("\n{0}/3", TextSteampoint);
        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //雨の中に留まっていたら潤いが溜まっていく
        if (other.tag == "Rain" && Rain < 100)
        {
            AddRain += Rainup;
            Rain = (int)AddRain;
            Utext.text = string.Format("潤い: {0}", Rain);
            _RainSlider.value = Rain;
        }
    }

    /// <summary>
    /// ダメージを受けた時に呼び出す処理
    /// </summary>
    void OnDamegeEffect()
    {
        damageFlag = true;

        StartCoroutine(WaitForit());//WaitForitで指定した秒分待ってから呼び出す
    }


    IEnumerator WaitForit()
    {
        yield return new WaitForSeconds(1);//1秒間処理を止める

        //ダメージを受けてないと判断し、点滅をやめる
        damageFlag = false;
        MainSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

    }

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
