using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheepReply : MonoBehaviour
{
    public Image profilePicture;
    public TextMeshProUGUI message;

    [HideInInspector]
    public float Durration;

    public void Initialize(PigeonReply config)
    {
        profilePicture.sprite = config.icon;
        message.text = config.message;
        Durration = config.replyTime;
    }
}
