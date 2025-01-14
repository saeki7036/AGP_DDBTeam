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
    /// HP‚ª‰æ–ÊŠO‚ÉˆÚ“®‚·‚éUI‘JˆÚ
    /// </summary>
    public void AnimatorHPBarRemove()
    {
        animator.SetBool("BarDirected", false);
    }

    /// <summary>
    /// HP‚ª‰æ–Ê“à‚ÉˆÚ“®‚·‚éUI‘JˆÚ
    /// </summary>
    public void AnimatorHPBarEnter()
    {
        animator.SetBool("BarDirected", true);
    }
}
