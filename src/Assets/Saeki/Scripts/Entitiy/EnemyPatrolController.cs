using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolController : EnemyBaseClass
{ 
    [SerializeField] private float nextPosDistance = 4f;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private bool isRoop = true;
   
    private int nextPoint = 0;
    protected override Vector3 GetTargetPos() { return NextPointUpdate(); }

    private void GoNextPoint()
    {
        // �z����̎��̈ʒu��ڕW�n�_�ɐݒ�
        nextPoint++;

        // �ꏄ������ŏ��̒n�_�Ɉړ�
        if (isRoop && nextPoint == wayPoints.Length)
        {
            nextPoint = 0;
        }
    }
    private Vector3 NextPointUpdate()
    {
        
        // �n�_���Ȃɂ��ݒ肳��Ă��Ȃ��Ƃ��ɕԂ�
        if (wayPoints.Length == 0)
            return this.transform.position;

        // �n�_���P�ӏ������ݒ肳��Ă���Ƃ��ɕԂ�
        else if (wayPoints.Length == 1)
        {
            return wayPoints[0].position;
        }

        //�����𔻒�
        else if (GetDistanseForNavmesh() < nextPosDistance)
            GoNextPoint();
        
        // �G�[�W�F���g�����ݐݒ肳�ꂽ�ڕW�n�_�ɍs���悤�ɐݒ�
        return wayPoints[nextPoint].position;
    }
}
