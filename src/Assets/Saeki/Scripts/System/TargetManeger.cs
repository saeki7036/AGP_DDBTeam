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

    private static float SlowTimeCount = 0;//Slow中の計算カウント

    [SerializeField] private static float Interval = 4.5f;// スロー時間のインターバル
    [SerializeField] private static float watchDistancs = 15f;//EnemyがPlayerを発見する判定距離

    [SerializeField] private static float slowTimeScaleValue = 0.5f;// スロー時間のTimeScale

    /// <summary>
    /// Enemyのlist取得
    /// </summary>
    public static List<EnemyBaseClass> EnemyList => Enemy;

    /// <summary>
    /// staticで宣言されたGetメゾット
    /// </summary>
    /// <returns>プレイヤーのオブジェクト</returns>
    public static GameObject getPlayerObj() { return playerObject; }

    /// <summary>
    /// Playerのステータスの取得
    /// </summary>
    public static CharacterStatus PlayerStatus { get { return playerStatus; } }

    /// <summary>
    /// staticで宣言されたGetメゾット
    /// </summary>
    /// <returns>残りのSlow状態を(下限)0~1(上限)までに変換したパラメータ</returns>
    public static float GetSlowValue()
    {
        //0 <= value <=1の範囲
        float value = SlowTimeCount / Interval;
        //下限処理
        if (value < 0f) value = 0f;
        //上限処理
        else if (value > 1f) value = 1f;
        //計算結果返却
        return value; 
    }
    
    void Start()
    {
        //Playerのオブジェクトの取得
        playerObject = GameObject.FindWithTag("Player");
        //Playerのステータス取得
        playerStatus = playerObject.GetComponent<CharacterStatus>();
        //Enemyをすべて取得
        GameObject[] onFieldEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        //初期化
        SlowTimeCount = 0;
        //Enemyのlistを初期化
        Enemy = new List<EnemyBaseClass>();
        //Enemyのlistに追加
        foreach (GameObject g in onFieldEnemy)
            AddEnemyBaseClass(g);
    }

    private void Update()
    {
        //timeScaleを管理する
        TimeScaleManagement();
    }

    /// <summary>
    /// timeScaleを管理
    /// </summary>
    private void TimeScaleManagement()
    {
        //timeScaleがSlow状態でなければ以下の処理は走らない
        if (Time.timeScale != slowTimeScaleValue)
            return;
        //TimeCountにDeltaTimeではなくunscaledで加算
        SlowTimeCount += Time.unscaledDeltaTime;
        //TimeCountがIntervalを超えたら元のtimeScaleに戻す
        if (SlowTimeCount > Interval)
            Time.timeScale = 1f;
    }

    /// <summary>
    /// 乗り移りの頭を投げる状態に一時的にtimeScaleを戻す
    /// </summary>
    public static void StartHeadChange()
    {
        SlowTimeCount = 0;
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
        //対象オブジェクトの更新
        playerObject = player;
        //対象ステータスの更新
        playerStatus = playerObject.GetComponent<CharacterStatus>();
        //時間カウント初期化
        SlowTimeCount = 0;
        //timeScaleをSlow状態に
        Time.timeScale = slowTimeScaleValue;
        //Enemyの攻撃対象変更
        ChangeTarget();
    }

    /// <summary>
    /// Enemyのターゲットを更新させる
    /// </summary>
    private static void ChangeTarget()
    {
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            //Enemyの攻撃対象をSetする
            baseClass.ChangeTarget(playerObject);
        }
    }

    /// <summary>
    /// 一定の距離にあるEnemyのターゲット発見させる
    /// </summary>
    public static void WatchTarget()
    {
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            //距離を計算してwatchDistancs以下なら発見処理
            //平方根よりも2乗計算の方がコスト軽い
            if (distance_Square(playerObject.transform.position,baseClass.transform.position) < watchDistancs)    
                baseClass.Watch();
        }
    }
    /// <summary>
    /// ある位置座標から一定の距離にあるEnemyを取得
    /// </summary>
    /// <param name="position">位置座標</param>
    /// <param name="radius">位置座標からの距離</param>
    /// <returns>一定の距離内にあるEnemyのList</returns>
    public static List<EnemyBaseClass> TakeTarget(Vector3 position, float radius)
    {
        List<EnemyBaseClass> list = new();
        //敵の位置を調べる
        foreach (EnemyBaseClass baseClass in Enemy)
        {
            //一定の距離以下ならlistに追加
            //平方根よりも2乗計算の方がコスト軽い
            if (distance_Square(position ,baseClass.transform.position) < radius * radius)
                list.Add(baseClass);
        }
        //listで返却
        return list;
    }

    /// <summary>
    /// 指定された2つの位置（Vector3）間の距離の二乗を計算
    /// </summary>
    /// <param name="Target">ターゲットの位置</param>
    /// <param name="enemy">敵の位置</param>
    /// <returns>2点間の3次元の距離（平方根を取らない）</returns>
    private static float distance_Square(Vector3 Target ,Vector3 enemy) 
    { 
        return (Target - enemy).sqrMagnitude; 
    }
}