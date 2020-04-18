using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{

    [SerializeField, Header("敵を踏んだ時飛ぶ高さ")] public float boundHeight;

    //敵を踏んだ時のフラグ
    public bool playerjump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerjump==true)
        {
            Destroy(gameObject);
        }
    }
}
