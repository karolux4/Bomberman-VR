using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class You_Win_Animation : MonoBehaviour {
    private Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = new Animator();
        animator = this.gameObject.GetComponent<Animator>();
        animator.SetTrigger("Victory");
    }
}
