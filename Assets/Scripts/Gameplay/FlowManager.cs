using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    private static FlowManager _instance;
    public static FlowManager Instance { get => _instance; }

    [Header("Gameplay Config")]
    public List<LevelAction> gameFlow;
    public int currentLevel = 0;

    [Header("Prefab Config")]
    [SerializeField]
    private string gachalevel;

    [SerializeField]
    private string pigeonlevel;

    // TODO gameplay levels

    [SerializeField]
    private Animator levelAnimator;

    // Used to communicate data from 1 scene to the next, if you get to gacha or pigeon just assume it exists and keep going
    [HideInInspector]
    public List<LootBoxRoll> lootBoxMessage;

    //[HideInInspector]
    public List<PigeonDiscussion> pigeonMessage;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        currentLevel = -1;
    }

    public void TitleScreenEnter()
    {
        // LootBoxRoll lbr = new LootBoxRoll();
        // lbr.rolls = new List<LootBoxItem>();
        // GoToGacha(lbr);
        HandleNextGameFlow();
    }

    protected void HandleNextGameFlow()
    {
        currentLevel++;
        LevelAction next = gameFlow[currentLevel];

        if (next.actionType == ActionType.CRASH)
        {
            Debug.LogError($"Recieved a crash instruction on action {currentLevel}, have you tried configuring your stuff properly.");
            return;
        }
        else if (next.actionType == ActionType.GAMEPLAY)
        {
            GoToGameplay(next.gameplayLevel);
        }
        else if (next.actionType == ActionType.GACHA)
        {
            // Somehow run each of the gacha rolls.
        }
        else if (next.actionType == ActionType.PIGEON)
        {
            // Somehow run each pigeon discussion.
        }
    }

    protected void GoToGacha(LootBoxRoll lootBox)
    {
        this.lootBoxMessage = new List<LootBoxRoll>
        {
            lootBox
        };
        StartCoroutine(LoadLevel(gachalevel));
    }

    protected void GoToPigeon(PigeonDiscussion pigeonDiscussion)
    {
        this.pigeonMessage = new List<PigeonDiscussion>
        {
            pigeonDiscussion
        };
        StartCoroutine(LoadLevel(pigeonlevel));
    }

    protected void GoToGameplay(string level)
    {
        // TODO
    }

    private IEnumerator LoadLevel(string levelName)
    {
        PauseControl.Instance.PauseGlobal();
        levelAnimator.Play("FadeToBlack");
        yield return new WaitForSeconds(1f);
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        while (!ao.isDone)
        {
            yield return null;
        }
        levelAnimator.Play("FadeIntoScene");
        yield return new WaitForSeconds(1f);
        PauseControl.Instance.UnpauseGlobal();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    // Added due to pigeon manager to fade to black and reset
    // it will take 1 second to do that, you deal with it.
    public void RequestFadeToBlack()
    {
        levelAnimator.Play("FadeToBlack");
    }

    public void RequestVisable()
    {
        levelAnimator.Play("Idle");
    }
}

[System.Serializable]
public class LevelAction
{
    public ActionType actionType;

    /**Only used if it is a gameplay section*/
    [Header("Gameplay Configuration")]
    public string gameplayLevel;

    [Header("GACHA Configuration")]
    public List<LootBoxRoll> lootBoxRoll;

    [Header("Pigeon Configuration")]
    public List<PigeonDiscussion> pigeonDiscussion;
}

public enum ActionType
{
    CRASH,
    GAMEPLAY,
    GACHA,
    PIGEON,
}
