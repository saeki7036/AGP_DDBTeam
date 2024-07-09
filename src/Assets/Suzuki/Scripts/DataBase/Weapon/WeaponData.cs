using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OriginalScriptableObjects/Data/WeaponData")]
public class WeaponData : BaseData
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int maxBullet;

    // プロパティ
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
    }
    public int MaxBullet
    {
        get { return maxBullet; }
    }
}
