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
        yield return new WaitForSeconds(1f);
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
        yield return new WaitForSeconds(discussion.messageTime);

        for (int i = 0; i < discussion.replies.Count; i++)
        {
            replyPool[i].gameObject.SetActive(true);
            // play voice
            yield return new WaitForSeconds(replyPool[i].Durration);
        }
    }
}
