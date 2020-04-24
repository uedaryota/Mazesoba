using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }

    public void ButtonClickedTitle()
    {
        SceneManager.LoadScene("SampleTitle");
    }

    public void ButtonClickedGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void ButtonClickedEnd()
    {
        Quit();
    }
}
