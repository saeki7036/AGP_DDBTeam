using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectSceneChangeTitle : MonoBehaviour
{
    bool InputCheck = false;//入力フラグ

    void Start()
    {
        InputCheck = false;//Startで初期化
    }
    void Update()
    {
        //１と２の同時押しでTitleに戻る
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha2))
        {
            SceneChangeTitle();
        }
    }

    void SceneChangeTitle()
    {
        //二度押されてないか判定
        if (InputCheck == false)
            SceneManager.LoadSceneAsync("Title");
        //1度押されて変更
        InputCheck = true;
    }
}
