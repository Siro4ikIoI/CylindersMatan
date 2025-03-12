using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMaxObj : MonoBehaviour
{
    private Animator animCanvos;
    public GameObject Layer;
    public bool isActive = true;

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

    private void Update()
    {
        if (isActive) Layer.SetActive(true);
        else Layer.SetActive(false);
    }
}
