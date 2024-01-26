using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "RFC/PigeonDiscussion", order = 2)]
public class PigeonDiscussion : ScriptableObject
{
    public string message;
    public float messageTime;
    public List<PigeonReply> replies;
}

[System.Serializable]
public class PigeonReply
{
    public Sprite icon;
    public string message;
    public float replyTime;
}
