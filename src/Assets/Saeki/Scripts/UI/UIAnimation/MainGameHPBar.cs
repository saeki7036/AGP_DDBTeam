using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameHPBar: MonoBehaviour
{
    [SerializeField] Animator animator;//HPBarのAnimator

    /// <summary>
    /// HPが画面外に移動するUI遷移
    /// </summary>
    public void AnimatorHPBarRemove()
    {
        animator.SetBool("BarDirected", false);
    }

    /// <summary>
    /// HPが画面内に移動するUI遷移
    /// </summary>
    public void AnimatorHPBarEnter()
    {
        animator.SetBool("BarDirected", true);
    }
}
