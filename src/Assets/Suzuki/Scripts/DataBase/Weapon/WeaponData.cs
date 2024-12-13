using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class BulletSettings
{
    [SerializeField] Vector2 diffusion;
    [Range(0,1),SerializeField] float randomNess = 0f;

    public Vector2 Diffusion
    {
        get { return diffusion; }
    }
    public float RandomNess
    {
        get { return randomNess;}
    }
}

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
    [SerializeField] List<BulletSettings> bulletSettings;
    [SerializeField] WeaponRole weaponRole;
    [SerializeField] Sprite weaponImage;
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
    public List<BulletSettings> BulletSettings
    {
        get { return bulletSettings; }
    }
    public Sprite WeaponImage
    {
        get { return weaponImage; }
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
