using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryArea : MonoBehaviour
{
    [SerializeField]
    EnemyDiscoveryController[] enemy;
    /// <summary>
    /// �v���C���[�����͈͓��ɓ���������Enemy�̈ړ���ݒ�
    /// </summary>
    /// <param name="other">�R���C�_</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //�ēx�N�����Ȃ��悤�ɔ�\��
            this.gameObject.SetActive(false);

            foreach (EnemyDiscoveryController enemies in enemy)
            {
                //��Ɍ��j����Ă���ꍇ�����O
                if (enemies != null)
                    enemies.IsDiscobery();
            }
        }
    }
}
