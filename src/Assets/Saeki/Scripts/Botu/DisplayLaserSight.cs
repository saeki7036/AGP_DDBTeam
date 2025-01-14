using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLaserSight : MonoBehaviour
{
    [Serializable]
    class LineWidth
    {
        [SerializeField] float min = 0.1f;
        [SerializeField] float max = 1f;
    }

    [SerializeField] LineWidth lineWidth;
    [SerializeField] AnimationCurve emitCurve;

    SearchColliderScript searchColliderScript;
    EnemyBaseClass enemyBaseClass;
    LineRenderer lineRenderer;
    float foundTimer = 0f;// �v���C���[�������Ă��鎞��
    float emitTime = 0.05f;// ���̓_�łɂ����鎞��
    bool firstFind = false;

    // Start is called before the first frame update
    void Start()
    {
        searchColliderScript = GetComponentInChildren<SearchColliderScript>();
        lineRenderer = GetComponent<LineRenderer>();
        enemyBaseClass = GetComponentInParent<EnemyBaseClass>();
        firstFind = false;
    }

    bool Createcheck() => enemyBaseClass.FindCheck() && enemyBaseClass.HealthCheck() && searchColliderScript.FoundPlayer != null;
        
    void FixedUpdate()
    {
        if (Createcheck())
        {
            foundTimer += Time.deltaTime;
            CreateLaser();
        }
        else if (firstFind)
        {
            OffLaser();
        }
    }

    void CreateLaser()
    {

        if (!firstFind)
        {
            firstFind = true;
            // �����Ƀ��[�U�[�̃V�F�[�_�[�ɍĐ��J�n������ǉ��\��
        }

        Vector3 direction = searchColliderScript.FoundPlayer.position - transform.position;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + direction);
    }

    void OffLaser()
    {
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

        firstFind = false;
    }

    IEnumerator Emit()
    {
        yield return null;
        // �}���Ȋg��A�k����������\��
    }
}
