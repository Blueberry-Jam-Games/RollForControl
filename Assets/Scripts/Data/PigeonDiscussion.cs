using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "RFC/PigeonDiscussion", order = 2)]
public class PigeonDiscussion : ScriptableObject
{
    [Multiline(5)]
    public string message;
    // public float messageTime;
    public List<PigeonReply> replies;
    public AudioClip voiceOver;
}

[System.Serializable]
public class PigeonReply
{
    public Sprite icon;
    [Multiline(2)]
    public string message;
    public float replyTime;
}
