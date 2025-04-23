using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonSelect : MonoBehaviour
{
    //ボタンを押した時の音声
    [SerializeField] private AudioSource audioSE;
    /// <summary>
    /// タイトルシーンに遷移
    /// </summary>
    public void SceneChangeTitle()
    {
        audioSE.PlayOneShot(audioSE.clip);
        SceneManager.LoadSceneAsync("Title");
    }
    /// <summary>
    /// 指定のシーンに遷移
    /// </summary>
    public void SceneChangeMainGame(string SceneName)
    {
        audioSE.PlayOneShot(audioSE.clip);
        SceneManager.LoadSceneAsync(SceneName);
    }
    /// <summary>
    /// 指定のシーンに遷移(SEを鳴らさない)
    /// </summary>
    public void SceneChangeMainGameNoSE(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    /// <summary>
    /// 同じシーンに再度遷移させる。
    /// </summary>
    public void SceneRerode()
    {
        audioSE.PlayOneShot(audioSE.clip);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void FinishGame()
    {
        audioSE.PlayOneShot(audioSE.clip);
        Application.Quit();
    }
}
