using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    /// <summary>
    /// e‚©‚ç”­Ë•ûŒü‚ğŒü‚¢‚½e’e‚ğ¶¬‚·‚é
    /// </summary>
    /// <returns>Œ‚‚Ä‚½‚©‚Ç‚¤‚©‚ğ•Ô‚·</returns>
    public bool Shoot(GameObject bulletPrefab, Vector3 position, Vector3 direction, int remainBullets)
    {
        if(remainBullets <= 0) return false;
        remainBullets--;
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.transform.LookAt(direction);
        return true;
    }
}
