using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PigeonManager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI originalCheep;

    [SerializeField]
    public List<CheepReply> replyPool;

    [Header("Sounds")]
    [SerializeField]
    private AudioSource tweetSound;

    [SerializeField]
    private AudioSource tweetDialogue;

    [SerializeField]
    private AudioSource replySound;

    private void Start()
    {
        for (int i = 0; i < replyPool.Count; i++)
        {
            replyPool[i].gameObject.SetActive(false);
        }
        List<PigeonDiscussion> cheeps = FlowManager.Instance.pigeonMessage;

        originalCheep.text = cheeps[0].message;
        StartCoroutine(PlayAllDiscussions(cheeps));
        // Do what you need
    }

    private IEnumerator PlayAllDiscussions(List<PigeonDiscussion> cheeps)
    {
        // scene transition delay
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < cheeps.Count; i++)
        {
            yield return PlayDiscussion(cheeps[i]);

            if (i + 1 < cheeps.Count)
            {
                FlowManager.Instance.RequestFadeToBlack();
                yield return new WaitForSeconds(1.5f);
                FlowManager.Instance.RequestVisable();
            }
        }

        FlowManager.Instance.AddativeSceneDone();
    }

    private IEnumerator PlayDiscussion(PigeonDiscussion discussion)
    {
        originalCheep.text = discussion.message;

        for (int i = 0; i < replyPool.Count; i++)
        {
            if (i < discussion.replies.Count)
            {
                replyPool[i].Initialize(discussion.replies[i]);
            }
            replyPool[i].gameObject.SetActive(false);
        }

        Canvas.ForceUpdateCanvases();
        
        // trigger original message sound
        tweetSound.Play();
        yield return new WaitForSeconds(tweetSound.clip.length);

        if (discussion.voiceOver == null)
        {
            Debug.LogError($"Forgot to configure dialogue {tweetDialogue.name}");
        }

        tweetDialogue.clip = discussion.voiceOver;
        tweetDialogue.Play();

        yield return new WaitForSeconds(tweetDialogue.clip.length);

        for (int i = 0; i < discussion.replies.Count; i++)
        {
            replySound.Play();
            yield return new WaitForSeconds(replyPool[i].Durration / 2);
            replyPool[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(replyPool[i].Durration / 2);
        }

        yield return new WaitForSeconds(0.25f);
    }
}
