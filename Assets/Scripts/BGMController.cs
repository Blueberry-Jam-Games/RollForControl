using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class BGMController : MonoBehaviour
{
    [SerializeField]
    private AudioSource gameplayTheme;

    [SerializeField]
    private AudioSource twitterTheme;

    [SerializeField]
    private AudioSource gachaTheme;

    [SerializeField]
    private AudioSource pinTailTheme;

    private GameTheme currentPlaying = GameTheme.NONE;
    private bool isTransitioning = false;

    private void Start()
    {
        // TODO title screen
    }

    public void GoToTheme(GameTheme next)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("Trying to change themes while change in progress, failing");
            return;
        }

        isTransitioning = true;
        if (currentPlaying != GameTheme.NONE)
        {
            StartCoroutine(TransitionTheme(next));
        }
        else
        {
            StartCoroutine(FadeInTheme(next));
        }
    }

    private IEnumerator TransitionTheme(GameTheme next)
    {
        float start = Time.time;
        AudioSource target = GetPlayingTrack(currentPlaying);

        float diff;
        while ((diff = Time.time - start) < 1.0f)
        {
            target.volume = 1.0f - diff;
            yield return null;
        }

        target.volume = 0;
        target.Stop();

        if (next != GameTheme.NONE)
        {
            yield return FadeInTheme(next);
        }
        else
        {
            isTransitioning = false;
        }
    }

    private IEnumerator FadeInTheme(GameTheme theme)
    {
        yield return new WaitForSeconds(0.75f);
        currentPlaying = theme;
        AudioSource target = GetPlayingTrack(currentPlaying);

        target.volume = 1.0f;
        target.Play();

        isTransitioning = false;
    }

    private AudioSource GetPlayingTrack(GameTheme themeCheck)
    {
        switch(themeCheck)
        {
            case GameTheme.GAMEPLAY: return gameplayTheme;
            case GameTheme.PIGEON: return twitterTheme;
            case GameTheme.GACHA: return gachaTheme;
            case GameTheme.PINTAIL: return pinTailTheme;
            default: return gameplayTheme;
        }
    }
}

public enum GameTheme
{
    NONE = -1,
    GAMEPLAY = 0,
    PIGEON = 1,
    GACHA = 2,
    PINTAIL = 3
}
