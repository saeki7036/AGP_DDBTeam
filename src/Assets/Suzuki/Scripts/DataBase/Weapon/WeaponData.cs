using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OriginalScriptableObjects/Data/WeaponData")]
public class WeaponData : BaseData
{
    public enum WeaponRole
    {
        main,
        sub
    }

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int maxBullet;
    [SerializeField] WeaponRole weaponRole;
    [SerializeField] GameObject subWeapon;

    // プロパティ
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
    }
    public int MaxBullet
    {
        get { return maxBullet; }
    }
    public WeaponRole Role
    {
        get { return weaponRole; }
    }
    public GameObject SubWeapon
    {
        get { return subWeapon; }
    }
}
