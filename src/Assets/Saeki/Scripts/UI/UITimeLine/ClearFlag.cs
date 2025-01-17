using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearFlag : MonoBehaviour
{
    private List<EnemyBaseClass> Enemys = new List<EnemyBaseClass>();
    private bool clearCheck;
    [SerializeField] private PlayableDirector playableDirector;
    public bool IsClearFlag => clearCheck;
    // Start is called before the first frame update
    void Start()
    {
        Enemys = TargetManeger.EnemyList;
        clearCheck = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Enemyの全滅を判定
        if (!clearCheck && Check(TargetManeger.EnemyList))
        {
            Time.timeScale = 0f;
            PlayTimeline();
        }
    }
    bool Check(List<EnemyBaseClass> enemys)
    {
        List<EnemyBaseClass> Dead = new List<EnemyBaseClass>();
        foreach (EnemyBaseClass enemy in enemys)
        {
            //HPが0ならListに入れる
            if (enemy.IsDead)
            {
                Dead.Add(enemy);
            }
        }
        //HPが0のEnemyをListから外す
        for (int i = 0;i < Dead.Count; i++)
        {
            Enemys.Remove(Dead[i]);
        }

        //Listが空なら全滅
        return Enemys.Count == 0;
    }

    void PlayTimeline()
    {
        //TimeLine起動
        playableDirector.Play();
        clearCheck = true;
    }
}
