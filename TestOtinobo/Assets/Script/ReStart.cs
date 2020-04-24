using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FadeManager.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    /// ボタンをクリックした時の処理
    public void OnClick()
    {
        FadeManager.FadeOut(0);
    }


    public void Retry()
    {
        SceneManager.LoadScene("SampleEnd");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
}
