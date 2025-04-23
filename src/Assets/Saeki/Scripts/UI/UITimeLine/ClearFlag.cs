using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearFlag : MonoBehaviour
{
    //Enemy“o˜^‚·‚élist
    private List<EnemyBaseClass> Enemys = new List<EnemyBaseClass>();

    private bool clearCheck;

    [SerializeField] private PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        //TargetManeger‚Åæ“¾‚µ‚½Enemy‚ğlist‚É“o˜^
        Enemys = TargetManeger.EnemyList;
        clearCheck = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Enemy‚Ì‘S–Å‚ğ”»’è
        if (!clearCheck && DeadCheck(TargetManeger.EnemyList))
        {
            //timeScale‚ğ’â~
            Time.timeScale = 0f;
            //ƒNƒŠƒA‰‰oÄ¶
            PlayTimeline();
        }
    }

    //‘S–Å”»’è
    bool DeadCheck(List<EnemyBaseClass> enemys)
    {
        //HP‚ª0‚ÌEnemy‚Ìæ“¾
        List<EnemyBaseClass> Dead = new List<EnemyBaseClass>();
        //“o˜^‚µ‚Ä‚¢‚éEnemy‚ğ’Tõ
        foreach (EnemyBaseClass enemy in enemys)
        {
            //HP‚ª0‚È‚çList‚É“ü‚ê‚é
            if (enemy.IsDead)
            {
                //ƒŠƒXƒg‚É’Ç‰Á
                Dead.Add(enemy);
            }
        }
        //HP‚ª0‚ÌEnemy‚ğList‚©‚çŠO‚·
        for (int i = 0;i < Dead.Count; i++)
        {
            ////“o˜^‚µ‚Ä‚¢‚éEnemy‚ğƒŠƒXƒg‚©‚çŠO‚·
            Enemys.Remove(Dead[i]);
        }
        //“o˜^‚µ‚Ä‚¢‚éEnemy‚ÌList‚ª‹ó‚È‚ç‘S–Å
        return Enemys.Count == 0;
    }

    void PlayTimeline()
    {
        //TimeLine‹N“®
        playableDirector.Play();
        clearCheck = true;
    }
}
