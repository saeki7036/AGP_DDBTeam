using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiscoveryController : EnemyBaseClass
{
    private bool discovery;
    /// <summary>
    /// �O������̔��������ݒ�
    /// </summary>
    public void IsDiscobery() { discovery = true; }
    protected override void SetUpOverride()
    {
        discovery = false;
    }

    protected override Vector3 GetTargetPos()
    {
        //�������Ă���Ȃ�Target�̈ʒu��Ԃ�
        if (discovery)
        {
            return TargetSetting.transform.position;
        }

        //���F���Ă���Ȃ甭������
        if (FindCheck())
        {
            discovery = true;
        }

        //������Ȃ��ꍇ�ҋ@
        return this.transform.position;
    }
}