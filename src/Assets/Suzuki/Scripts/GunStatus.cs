using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStatus : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;

    int remainBullets;

    void Start()
    {
        remainBullets = weaponData.MaxBullet;
    }
    /// <summary>
    /// e‚©‚ç”­Ë•ûŒü‚ğŒü‚¢‚½e’e‚ğ¶¬‚·‚é
    /// </summary>
    /// <param name="infiniteBullet">’e‚ğÁ”ï‚³‚¹‚é‚©‚Ç‚¤‚©</param>
    /// <returns>Œ‚‚Ä‚½‚©‚Ç‚¤‚©‚ğ•Ô‚·</returns>
    public bool Shoot(Vector3 position, Vector3 forward, string tag, bool infiniteBullet)
    {
        if(remainBullets <= 0 && !infiniteBullet) return false;
        if(!infiniteBullet) remainBullets--;
        
        GameObject bullet = Instantiate(weaponData.BulletPrefab, position, Quaternion.identity);
        bullet.tag = tag;
        bullet.transform.forward = forward;
        return true;
    }
}
