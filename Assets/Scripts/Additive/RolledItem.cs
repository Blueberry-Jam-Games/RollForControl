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

    public SpriteRenderer displayObject;

    public float Durration
    {
        get => splineAnimation.Duration;
    }

    public void Initialize(LootBoxItem self)
    {
        this.ownItem = self;
        displayObject.sprite = self.image;
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

    public void Reset()
    {
        splineAnimation.Restart(false);
    }

    private IEnumerator OnComplete()
    {
        yield return new WaitForSeconds(Durration);
        gameObject.SetActive(false);
    }

    public string GetName()
    {
        return ownItem.name;
    }
}
