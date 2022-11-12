using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingScreen : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameMgr.OnTimeForBlink += Blink;
    }

    private void OnDisable()
    {
        GameMgr.OnTimeForBlink -= Blink;
    }

    private void Blink()
    {
        animator.Play("Blink", 0, 0);
    }

    public void OnTimeForReshuffle()
    {
        GameMgr.instance.ReshuffleProps();
    }

    public void OnEndedBlinking()
    {
        GameMgr.instance.StartCoroutine(GameMgr.instance.CountdownToBlink());
    }
}
