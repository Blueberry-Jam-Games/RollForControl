using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class BGMController : MonoBehaviour
{
    [SerializeField]
    private AudioSource gameplayTheme;
    public float gameplayVol = 0.7f;

    [SerializeField]
    private AudioSource twitterTheme;
    public float twitterVol = 0.5f;

    [SerializeField]
    private AudioSource gachaTheme;
    public float gachaVol = 0.0f;

    [SerializeField]
    private AudioSource pinTailTheme;
    public float pinTailVol = 0.7f;

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
            target.volume = (1.0f - diff) * GetTargetVolume(currentPlaying);
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
        yield return new WaitForSeconds(0.5f);
        currentPlaying = theme;
        AudioSource target = GetPlayingTrack(currentPlaying);

        target.volume = GetTargetVolume(currentPlaying);
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

    private float GetTargetVolume(GameTheme themeCheck)
    {
        switch(themeCheck)
        {
            case GameTheme.GAMEPLAY: return gameplayVol;
            case GameTheme.PIGEON: return twitterVol;
            case GameTheme.GACHA: return gachaVol;
            case GameTheme.PINTAIL: return pinTailVol;
            default: return 0.0f;
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
