using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayStub : MonoBehaviour
{
    public Button win;
    public Button lose;

    private void Start()
    {
        win.onClick.AddListener(Win);
        lose.onClick.AddListener(Lose);
    }

    private void Win()
    {
        FlowManager.Instance.GameplayWin(new List<LootBoxRoll>(0));
    }

    private void Lose()
    {
        FlowManager.Instance.GameplayLose();
    }
}
