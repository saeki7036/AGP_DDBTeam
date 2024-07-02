using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OriginalScriptableObjects/Data/WeaponData")]
public class WeaponData : BaseData
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float attackPower;// おそらく弾丸のデータベースを作りそちらに移行する
    [SerializeField] int maxBullet;

    // プロパティ
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
    }
    public float AttackPower
    {
        get { return attackPower; }
    }
    public int MaxBullet
    {
        get { return maxBullet; }
    }
}
