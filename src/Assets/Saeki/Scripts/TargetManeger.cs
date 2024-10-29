using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManeger : MonoBehaviour
{
    private static List<EnemyBaseClass> Enemy = new List<EnemyBaseClass>();
    private static GameObject playerObject;
    /// <summary>
    /// staticで宣言されたGetメゾット
    /// </summary>
    /// <returns>プレイヤーのオブジェクト</returns>
    public static GameObject getPlayerObj() { return playerObject; }
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        GameObject[] onFieldEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject g in onFieldEnemy)
            AddEnemyBaseClass(g);
    }

    /// <summary>
    /// 敵対する対象を変更する
    /// </summary>
    public static void SetTarget(GameObject player)
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
    public static void AddEnemyBaseClass(GameObject enemy)
    {
        Enemy.Add(enemy.GetComponentInParent<EnemyBaseClass>());
    }
}
