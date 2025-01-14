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
    private static float TimeCount = 0;

    [SerializeField] private static float Interval = 4.5f;// �X���[����
    [SerializeField] private static float watchDistancs = 15f;



    public static List<EnemyBaseClass > EnemyList => Enemy;

    /// <summary>
    /// static�Ő錾���ꂽGet���]�b�g
    /// </summary>
    /// <returns>�v���C���[�̃I�u�W�F�N�g</returns>
    public static GameObject getPlayerObj() { return playerObject; }

    public static CharacterStatus PlayerStatus { get { return playerStatus; } }

    /// <summary>
    /// static�Ő錾���ꂽGet���]�b�g
    /// </summary>
    /// <returns>�c���Slow��Ԃ�(����)0~1(���)�܂łɕϊ������p�����[�^</returns>
    public static float GetSlowValue()
    {
        float value = TimeCount / Interval;
        if (value < 0f) value = 0f;
        else if (value > 1f) value = 1f;
        return value; 
    }
    
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        playerStatus = playerObject.GetComponent<CharacterStatus>();
        GameObject[] onFieldEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        TimeCount = 0;

        Enemy = new List<EnemyBaseClass>();
        foreach (GameObject g in onFieldEnemy)
            AddEnemyBaseClass(g);
    }

    private void Update()
    {
        TimeScaleManagement();
    }

    private void TimeScaleManagement()
    {
        if (Time.timeScale == 1f || Time.timeScale == 0f || Time.timeScale == 0.1f)
            return;

        TimeCount += Time.unscaledDeltaTime;

        if (TimeCount > Interval)
            Time.timeScale = 1f;
    }

    /// <summary>
    /// ���ڂ�̓��𓊂����ԂɈꎞ�I��timeScale��߂�
    /// </summary>
    public static void StartHeadChange()
    {
        TimeCount = 0;
        Time.timeScale = 0.1f;
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
        playerObject = player;
        playerStatus = playerObject.GetComponent<CharacterStatus>();
        TimeCount = 0;
        Time.timeScale = 0.5f;
        ChangeTarget();
    }
   
    private static void ChangeTarget()
    {
        foreach (EnemyBaseClass baseClass in Enemy)
        {
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
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            if (distance_Square(position ,baseClass.transform.position) < radius * radius)
                list.Add(baseClass);
        }
        return list;
    }


    private static float distance_Square(Vector3 Target ,Vector3 enemy) 
    { 
        return (Target - enemy).sqrMagnitude; 
    }
}