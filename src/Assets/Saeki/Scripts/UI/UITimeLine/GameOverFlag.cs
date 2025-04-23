using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameOverFlag : MonoBehaviour
{
    [SerializeField] private Change change;
    [SerializeField] private PlayableDirector playableDirector;

    private bool gameOverCheck;
   
    float PlayerHP => change.CharacterStatusHp;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        gameOverCheck = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ゲームオーバーしているか判定
        //頭をチェンジ中か判定
        //PlayerのHPが全損しているか判定
        if (!gameOverCheck && !change.Changing && PlayerHP <= 0)
        {
            //timeScaleを停止
            Time.timeScale = 0f;
            //演出再生
            PlayTimeline();
        }
    }
 
    void PlayTimeline()
    {
        //TimeLine起動
        playableDirector.Play();
        gameOverCheck = true;
    }
}
