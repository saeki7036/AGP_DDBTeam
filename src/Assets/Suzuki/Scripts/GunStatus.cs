using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunStatus : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;

    int remainBullets;
    [SerializeField] float firstShotIntarval = 2f, defaultShotIntarval = 0.5f;

    public float FirstIntarval => firstShotIntarval;
    public float DefaultIntarval => defaultShotIntarval;

    public int RemainBullets
    {
        get { return remainBullets; }
    }

    void Start()
    {
        FillBullet();
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

        if(remainBullets == 0 && weaponData.Role == WeaponData.WeaponRole.main)
        {
            ChangeWeapon();
        }
        return true;
    }

    void FillBullet()// ’eŠÛ‚Ì•â[
    {
        remainBullets = weaponData.MaxBullet;
    }

    void ChangeWeapon()// •Ší‚ÌØ‚è‘Ö‚¦
    {
        if (weaponData.SubWeapon != null)
        {
            Debug.Log("ƒTƒu•Ší‚ğg‚¤I");
            Transform parent = this.transform.parent;
            Instantiate(weaponData.SubWeapon, parent);// •Ší‚Ì¶¬
            PlayerMove playerMove = parent.GetComponentInChildren<PlayerMove>();

            this.transform.SetParent(null);// eq•t‚¯‚ğŠO‚·
            playerMove.SetGunObject();// ‚Á‚Ä‚¢‚ée‚Ìİ’è
            Destroy(this.gameObject);
        }
    }
}
