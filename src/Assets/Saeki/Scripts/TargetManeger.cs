using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TargetManeger : MonoBehaviour
{
    private static List<EnemyBaseClass> Enemy = new List<EnemyBaseClass>();
    private static GameObject playerObject;

    private float Interval = 3f;
    private static float TimeCount = 0;

    [SerializeField] private static float watchDistancs = 15f;

    /// <summary>
    /// staticで宣言されたGetメゾット
    /// </summary>
    /// <returns>プレイヤーのオブジェクト</returns>
    public static GameObject getPlayerObj() { return playerObject; }
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        GameObject[] onFieldEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        TimeCount = 0;
        foreach (GameObject g in onFieldEnemy)
            AddEnemyBaseClass(g);
    }

    private void Update()
    {
        if (Time.timeScale == 1f)
            return;

        TimeCount += Time.unscaledDeltaTime;

        if(TimeCount > Interval)
            Time.timeScale = 1f;
         
    }
    /// <summary>
    /// 敵対対象の引数の確保
    /// </summary>
    public static void AddEnemyBaseClass(GameObject enemy)
    {
        Enemy.Add(enemy.GetComponentInParent<EnemyBaseClass>());
    }
    /// <summary>
    /// 敵対する対象を変更する
    /// </summary>
    public static void SetTarget(GameObject player)
    {
        playerObject = player;
        TimeCount = 0;
        Time.timeScale = 0.2f;
        ChangeTarget();
    }
   
    private static void ChangeTarget()
    {
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            baseClass.ChangeTarget(playerObject);
        }
    }

    public static void WatchTarget()
    {
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            if (distance_Square(baseClass.transform.position) < watchDistancs)    
                baseClass.Watch();
        }
    }

    private static float distance_Square(Vector3 enemy) 
    { 
        return (playerObject.transform.position - enemy).sqrMagnitude; 
    }
}