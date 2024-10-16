using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetManeger : MonoBehaviour
{
    private List<EnemyBaseClass> Enemy = new List<EnemyBaseClass>();
    private GameObject playerObject;
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        GameObject[] onFieldEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject g in onFieldEnemy)       
            Enemy.Add(g.GetComponentInParent<EnemyBaseClass>());
        
    }

    /// <summary>
    /// 敵対する対象を変更する
    /// </summary>
    public void SetTarget(GameObject player)
    {
        playerObject = player;
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            baseClass.ChengeTarget(playerObject);
        }         
    }
    /// <summary>
    /// 敵対対象の引数の確保
    /// </summary>
    public void AddEnemyBaseClass(GameObject enemy)
    {
        Enemy.Add(enemy.GetComponentInParent<EnemyBaseClass>());
    }
}
