using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeChanger : MonoBehaviour
{
    public Animator animator;

    private int levelToLoad;

    public void FadeOut(){
        animator.SetBool("Darkness", true);        
        // animator.SetTrigger("Fade");
    }

    public void FadeIn(){
        animator.SetBool("Darkness", false);        
        // animator.SetTrigger("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0)){
        //     FadeOut();
        // } else if (Input.GetMouseButtonDown(1)){
        //     FadeIn();
        // }
    }

    public void OnFadeComplete(){
        // SceneManager.LoadScene(levelToLoad);
    }
}
