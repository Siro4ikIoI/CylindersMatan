using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMaxObj : MonoBehaviour
{
    private Animator animCanvos;
    public bool isActive = false;

    private void Start()
    {
        animCanvos = gameObject.GetComponent<Animator>();
    }

    public void Toggle()
    {
        isActive = !isActive;
        animCanvos.SetBool("MAX", isActive);
        Debug.Log(isActive);
    }
}
