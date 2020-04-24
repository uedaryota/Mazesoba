using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJump : MonoBehaviour
{
    [SerializeField, Header("アイテムを入手したときの飛ぶ高さ")] public float boundHeight;
    [SerializeField, Header("アイテムを入手したときの飛ぶ回数")] public float boundCount;

    //敵を踏んだ時のフラグ
    public bool playerjump;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerjump == true)
        {
            Destroy(gameObject);
        }
    }
}
