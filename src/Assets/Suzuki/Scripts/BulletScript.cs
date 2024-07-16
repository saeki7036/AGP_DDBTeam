using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] BulletData bulletData;
    void Start()
    {
     
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterStatus>(out CharacterStatus character))// キャラクターに当たったとき
        {
            character.TakeDamage(bulletData.AttackPower);
            Destroy(this.gameObject);
        }
    }
}
