using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadBlowScript : MonoBehaviour
{
    [SerializeField] float power;
    [SerializeField] Rigidbody rigidbody;

    public void BlowOff(Vector3 basePosition)
    {
        Vector3 direction = transform.position - basePosition;
        direction.Normalize();

        rigidbody.AddForce(direction * power, ForceMode.Impulse);
        StartCoroutine(DestroyAfterSeconds(1.5f));
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
