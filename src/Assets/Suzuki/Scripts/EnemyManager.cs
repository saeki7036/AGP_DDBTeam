using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class EnemyProperties
{
    [SerializeField] EnemyBaseClass enemyClass;
    [SerializeField] SearchColliderScript searchColliderScript;

    public EnemyBaseClass EnemyClass
    {
        get { return enemyClass; }
    }

    public SearchColliderScript SearchColliderScript
    {
        get { return searchColliderScript; }
        set { searchColliderScript = value; }
    }

    //public void SetSearchColliderScript(SearchColliderScript searchColliderScript)
    //{
    //    this.searchColliderScript = searchColliderScript;
    //}
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<EnemyProperties> enemyList;
    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// プレイヤーを発見している敵の発見情報をリセットする
    /// </summary>
    public void ResetSearch()
    {
        // Changeスクリプトから乗り移り時に呼び出す予定
        foreach(EnemyProperties enemy in enemyList)
        {
            if(enemy.SearchColliderScript.FoundPlayer != null)
            {
                enemy.SearchColliderScript.OnPlayerChange();
            }
        }
    }

#if UNITY_EDITOR
    void OnValidate()// インスペクター上の変更時
    {
        if(enemyList != null)
        {
            foreach(var enemy in enemyList)
            {
                if(enemy != null)
                {
                    enemy.SearchColliderScript = enemy.EnemyClass.GetComponentInChildren<SearchColliderScript>();
                }
            }
        }
    }
#endif
}
