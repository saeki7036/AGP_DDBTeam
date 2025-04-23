using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TargetManeger : MonoBehaviour
{
    private static List<EnemyBaseClass> Enemy;
    private static GameObject playerObject;
    private static CharacterStatus playerStatus;

    private static float SlowTimeCount = 0;//Slow���̌v�Z�J�E���g

    [SerializeField] private static float Interval = 4.5f;// �X���[���Ԃ̃C���^�[�o��
    [SerializeField] private static float watchDistancs = 15f;//Enemy��Player�𔭌����锻�苗��

    [SerializeField] private static float slowTimeScaleValue = 0.5f;// �X���[���Ԃ�TimeScale

    /// <summary>
    /// Enemy��list�擾
    /// </summary>
    public static List<EnemyBaseClass> EnemyList => Enemy;

    /// <summary>
    /// static�Ő錾���ꂽGet���]�b�g
    /// </summary>
    /// <returns>�v���C���[�̃I�u�W�F�N�g</returns>
    public static GameObject getPlayerObj() { return playerObject; }

    /// <summary>
    /// Player�̃X�e�[�^�X�̎擾
    /// </summary>
    public static CharacterStatus PlayerStatus { get { return playerStatus; } }

    /// <summary>
    /// static�Ő錾���ꂽGet���]�b�g
    /// </summary>
    /// <returns>�c���Slow��Ԃ�(����)0~1(���)�܂łɕϊ������p�����[�^</returns>
    public static float GetSlowValue()
    {
        //0 <= value <=1�͈̔�
        float value = SlowTimeCount / Interval;
        //��������
        if (value < 0f) value = 0f;
        //�������
        else if (value > 1f) value = 1f;
        //�v�Z���ʕԋp
        return value; 
    }
    
    void Start()
    {
        //Player�̃I�u�W�F�N�g�̎擾
        playerObject = GameObject.FindWithTag("Player");
        //Player�̃X�e�[�^�X�擾
        playerStatus = playerObject.GetComponent<CharacterStatus>();
        //Enemy�����ׂĎ擾
        GameObject[] onFieldEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        //������
        SlowTimeCount = 0;
        //Enemy��list��������
        Enemy = new List<EnemyBaseClass>();
        //Enemy��list�ɒǉ�
        foreach (GameObject g in onFieldEnemy)
            AddEnemyBaseClass(g);
    }

    private void Update()
    {
        //timeScale���Ǘ�����
        TimeScaleManagement();
    }

    /// <summary>
    /// timeScale���Ǘ�
    /// </summary>
    private void TimeScaleManagement()
    {
        //timeScale��Slow��ԂłȂ���Έȉ��̏����͑���Ȃ�
        if (Time.timeScale != slowTimeScaleValue)
            return;
        //TimeCount��DeltaTime�ł͂Ȃ�unscaled�ŉ��Z
        SlowTimeCount += Time.unscaledDeltaTime;
        //TimeCount��Interval�𒴂����猳��timeScale�ɖ߂�
        if (SlowTimeCount > Interval)
            Time.timeScale = 1f;
    }

    /// <summary>
    /// ���ڂ�̓��𓊂����ԂɈꎞ�I��timeScale��߂�
    /// </summary>
    public static void StartHeadChange()
    {
        SlowTimeCount = 0;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// �G�ΑΏۂ̈����̊m��
    /// </summary>
    public static void AddEnemyBaseClass(GameObject enemy)
    {
        Enemy.Add(enemy.GetComponentInParent<EnemyBaseClass>());
    }
    /// <summary>
    /// �G�΂���Ώۂ�ύX����
    /// </summary>
    public static void SetTarget(GameObject player)
    {
        //�ΏۃI�u�W�F�N�g�̍X�V
        playerObject = player;
        //�ΏۃX�e�[�^�X�̍X�V
        playerStatus = playerObject.GetComponent<CharacterStatus>();
        //���ԃJ�E���g������
        SlowTimeCount = 0;
        //timeScale��Slow��Ԃ�
        Time.timeScale = slowTimeScaleValue;
        //Enemy�̍U���ΏەύX
        ChangeTarget();
    }

    /// <summary>
    /// Enemy�̃^�[�Q�b�g���X�V������
    /// </summary>
    private static void ChangeTarget()
    {
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            //Enemy�̍U���Ώۂ�Set����
            baseClass.ChangeTarget(playerObject);
        }
    }

    /// <summary>
    /// ���̋����ɂ���Enemy�̃^�[�Q�b�g����������
    /// </summary>
    public static void WatchTarget()
    {
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            //�������v�Z����watchDistancs�ȉ��Ȃ甭������
            //����������2��v�Z�̕����R�X�g�y��
            if (distance_Square(playerObject.transform.position,baseClass.transform.position) < watchDistancs)    
                baseClass.Watch();
        }
    }
    /// <summary>
    /// ����ʒu���W������̋����ɂ���Enemy���擾
    /// </summary>
    /// <param name="position">�ʒu���W</param>
    /// <param name="radius">�ʒu���W����̋���</param>
    /// <returns>���̋������ɂ���Enemy��List</returns>
    public static List<EnemyBaseClass> TakeTarget(Vector3 position, float radius)
    {
        List<EnemyBaseClass> list = new();
        //�G�̈ʒu�𒲂ׂ�
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            //���̋����ȉ��Ȃ�list�ɒǉ�
            //����������2��v�Z�̕����R�X�g�y��
            if (distance_Square(position ,baseClass.transform.position) < radius * radius)
                list.Add(baseClass);
        }
        //list�ŕԋp
        return list;
    }

    /// <summary>
    /// �w�肳�ꂽ2�̈ʒu�iVector3�j�Ԃ̋����̓����v�Z
    /// </summary>
    /// <param name="Target">�^�[�Q�b�g�̈ʒu</param>
    /// <param name="enemy">�G�̈ʒu</param>
    /// <returns>2�_�Ԃ�3�����̋����i�����������Ȃ��j</returns>
    private static float distance_Square(Vector3 Target ,Vector3 enemy) 
    { 
        return (Target - enemy).sqrMagnitude; 
    }
}