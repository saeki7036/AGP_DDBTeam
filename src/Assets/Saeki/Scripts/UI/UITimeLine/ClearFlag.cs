using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ClearFlag : MonoBehaviour
{
    //Enemy�o�^����list
    private List<EnemyBaseClass> Enemys = new List<EnemyBaseClass>();

    private bool clearCheck;

    [SerializeField] private PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        //TargetManeger�Ŏ擾����Enemy��list�ɓo�^
        Enemys = TargetManeger.EnemyList;
        clearCheck = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Enemy�̑S�ł𔻒�
        if (!clearCheck && DeadCheck(TargetManeger.EnemyList))
        {
            //timeScale���~
            Time.timeScale = 0f;
            //�N���A���o�Đ�
            PlayTimeline();
        }
    }

    //�S�Ŕ���
    bool DeadCheck(List<EnemyBaseClass> enemys)
    {
        //HP��0��Enemy�̎擾
        List<EnemyBaseClass> Dead = new List<EnemyBaseClass>();
        //�o�^���Ă���Enemy��T��
        foreach (EnemyBaseClass enemy in enemys)
        {
            //HP��0�Ȃ�List�ɓ����
            if (enemy.IsDead)
            {
                //���X�g�ɒǉ�
                Dead.Add(enemy);
            }
        }
        //HP��0��Enemy��List����O��
        for (int i = 0;i < Dead.Count; i++)
        {
            ////�o�^���Ă���Enemy�����X�g����O��
            Enemys.Remove(Dead[i]);
        }
        //�o�^���Ă���Enemy��List����Ȃ�S��
        return Enemys.Count == 0;
    }

    void PlayTimeline()
    {
        //TimeLine�N��
        playableDirector.Play();
        clearCheck = true;
    }
}
