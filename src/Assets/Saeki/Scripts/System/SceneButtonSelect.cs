using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonSelect : MonoBehaviour
{
    //�{�^�������������̉���
    [SerializeField] private AudioSource audioSE;
    /// <summary>
    /// �^�C�g���V�[���ɑJ��
    /// </summary>
    public void SceneChangeTitle()
    {
        audioSE.PlayOneShot(audioSE.clip);
        SceneManager.LoadSceneAsync("Title");
    }
    /// <summary>
    /// �w��̃V�[���ɑJ��
    /// </summary>
    public void SceneChangeMainGame(string SceneName)
    {
        audioSE.PlayOneShot(audioSE.clip);
        SceneManager.LoadSceneAsync(SceneName);
    }
    /// <summary>
    /// �w��̃V�[���ɑJ��(SE��炳�Ȃ�)
    /// </summary>
    public void SceneChangeMainGameNoSE(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    /// <summary>
    /// �����V�[���ɍēx�J�ڂ�����B
    /// </summary>
    public void SceneRerode()
    {
        audioSE.PlayOneShot(audioSE.clip);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// �Q�[�����I������
    /// </summary>
    public void FinishGame()
    {
        audioSE.PlayOneShot(audioSE.clip);
        Application.Quit();
    }
}
