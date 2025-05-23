using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearFlag : MonoBehaviour
{
    //Enemy登録するlist
    private List<EnemyBaseClass> Enemys = new List<EnemyBaseClass>();

    private bool clearCheck;

    [SerializeField] private PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        //TargetManegerで取得したEnemyをlistに登録
        Enemys = TargetManeger.EnemyList;
        clearCheck = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Enemyの全滅を判定
        if (!clearCheck && DeadCheck(TargetManeger.EnemyList))
        {
            //timeScaleを停止
            Time.timeScale = 0f;
            //クリア演出再生
            PlayTimeline();
        }
    }

    //全滅判定
    bool DeadCheck(List<EnemyBaseClass> enemys)
    {
        //HPが0のEnemyの取得
        List<EnemyBaseClass> Dead = new List<EnemyBaseClass>();
        //登録しているEnemyを探索
        foreach (EnemyBaseClass enemy in enemys)
        {
            //HPが0ならListに入れる
            if (enemy.IsDead)
            {
                //リストに追加
                Dead.Add(enemy);
            }
        }
        //HPが0のEnemyをListから外す
        for (int i = 0;i < Dead.Count; i++)
        {
            ////登録しているEnemyをリストから外す
            Enemys.Remove(Dead[i]);
        }
        //登録しているEnemyのListが空なら全滅
        return Enemys.Count == 0;
    }

    void PlayTimeline()
    {
        //TimeLine起動
        playableDirector.Play();
        clearCheck = true;
    }
}
