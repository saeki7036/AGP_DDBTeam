using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameHPBar: MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// HP����ʊO�Ɉړ�����UI�J��
    /// </summary>
    public void AnimatorHPBarRemove()
    {
        animator.SetBool("BarDirected", false);
    }

    /// <summary>
    /// HP����ʓ��Ɉړ�����UI�J��
    /// </summary>
    public void AnimatorHPBarEnter()
    {
        animator.SetBool("BarDirected", true);
    }
}