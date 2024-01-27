using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinTailManager : MonoBehaviour
{
    public TailMovement tail;

    public Animator uiAnimator;

    void Start()
    {
        StartCoroutine(RunPinTailGame());
    }

    private IEnumerator RunPinTailGame()
    {
        // screen fade delay
        yield return new WaitForSeconds(0.75f);
        // play animator
        uiAnimator.Play("PinTailInstructions");
        yield return new WaitForSeconds(1.666f);
        tail.doMovement = true;
        yield return new WaitForSeconds(0.6f);

        while (!tail.pinned)
        {
            yield return null;
        }

        uiAnimator.Play("PinTailCongradulations");
        yield return new WaitForSeconds(1.84f);

        FlowManager.Instance.PinTailComplete();
    }
}
