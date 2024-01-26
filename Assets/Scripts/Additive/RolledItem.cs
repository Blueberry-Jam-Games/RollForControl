using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class RolledItem : MonoBehaviour
{
    [SerializeField]
    private SplineAnimate splineAnimation;

    // Something about the sprite

    private LootBoxItem ownItem;

    public float Durration
    {
        get => splineAnimation.Duration;
    }

    public void Initialize(LootBoxItem self)
    {
        this.ownItem = self;
        // TODO something about the sprite
    }

    public void Play(bool deactivateAfter = true)
    {
        splineAnimation.Play();
        if (deactivateAfter)
        {
            StartCoroutine(OnComplete());
        }
    }

    private IEnumerator OnComplete()
    {
        yield return new WaitForSeconds(Durration);
        gameObject.SetActive(false);
    }
}
