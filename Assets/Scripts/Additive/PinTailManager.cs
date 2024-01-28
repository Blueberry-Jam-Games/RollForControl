using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinTailManager : MonoBehaviour
{
    public TailMovement tail;

    public Animator uiAnimator;

    [SerializeField]
    private AudioSource tailPinned;

    void Start()
    {
        StartCoroutine(RunPinTailGame());
    }

    private bool done = false;
    private IEnumerator RunPinTailGame()
    {
        // screen fade delay
        yield return new WaitForSeconds(0.75f);
        // play animator
        uiAnimator.Play("PinTailInstructions");
        yield return new WaitForSeconds(1.91f);
        tail.doMovement = true;
        yield return new WaitForSeconds(0.6f);

        while (!tail.pinned)
        {
            yield return null;
        }

        tailPinned.Play();

        uiAnimator.Play("PinTailCongradulations");
        yield return new WaitForSeconds(1.84f);

        if (!done)
        {
            FlowManager.Instance.PinTailComplete();
            done = true;
        }
    }
}
