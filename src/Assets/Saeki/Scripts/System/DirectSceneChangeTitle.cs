using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectSceneChangeTitle : MonoBehaviour
{
    bool InputCheck = false;//���̓t���O

    void Start()
    {
        InputCheck = false;//Start�ŏ�����
    }
    void Update()
    {
        //�P�ƂQ�̓���������Title�ɖ߂�
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha2))
        {
            SceneChangeTitle();
        }
    }

    void SceneChangeTitle()
    {
        //��x������ĂȂ�������
        if (InputCheck == false)
            SceneManager.LoadSceneAsync("Title");
        //1�x������ĕύX
        InputCheck = true;
    }
}
