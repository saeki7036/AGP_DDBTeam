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
        //Enemy�̑S�ł𔻒�
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
            //HP��0�Ȃ�List�ɓ����
            if (enemy.IsDead)
            {
                Dead.Add(enemy);
            }
        }
        //HP��0��Enemy��List����O��
        for (int i = 0;i < Dead.Count; i++)
        {
            Enemys.Remove(Dead[i]);
        }

        //List����Ȃ�S��
        return Enemys.Count == 0;
    }

    void PlayTimeline()
    {
        //TimeLine�N��
        playableDirector.Play();
        clearCheck = true;
    }
}
