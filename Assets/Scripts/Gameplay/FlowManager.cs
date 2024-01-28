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

    [SerializeField]
    private string pinTailLevel;

    [SerializeField]
    private string thanksForPlaying;

    [SerializeField]
    private LootBoxRoll replayGacha;

    [SerializeField]
    private Animator levelAnimator;

    // Used to communicate data from 1 scene to the next, if you get to gacha or pigeon just assume it exists and keep going
    [HideInInspector]
    public List<LootBoxRoll> lootBoxMessage;

    private List<LootBoxRoll> gameplayLootboxes;

    [HideInInspector]
    public List<PigeonDiscussion> pigeonMessage;

    public Dictionary<string, bool> inventory = new Dictionary<string, bool>();

    [SerializeField]
    private BGMController localSoundtrack;

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
        Debug.Log("Title Screen Flow Trigger");
        HandleNextGameFlow();
    }

    public void AddativeSceneDone()
    {
        HandleNextGameFlow();
    }

    public bool CheckPermission(string check)
    {
        /*Debug.Log($"InventoryPrint {inventory.Count}");
        foreach (KeyValuePair<string, bool> kvp in inventory)
        {
            //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            Debug.Log($"Key = {kvp.Key}, Value = {kvp.Value}");
        }*/

        return inventory.ContainsKey(check);
    }

    public void GameplayWin(List<LootBoxRoll> gameplayGachas)
    {
        gameplayLootboxes = gameplayGachas;
        SoundManager.Instance.PlaySound("gameplaywin");
        StartCoroutine(NextFlowLater());
        // TODO gameplay gachas
    }

    private IEnumerator NextFlowLater()
    {
        yield return new WaitForSeconds(0.25f);
        HandleNextGameFlow();
    }

    public void GameplayLose()
    {
        localSoundtrack.GoToTheme(GameTheme.PINTAIL);
        currentLevel--; // go back and replay level
        StartCoroutine(LoadLevel(pinTailLevel));
    }

    public void PinTailComplete()
    {
        List<LootBoxRoll> replayGachas = new List<LootBoxRoll>{
            replayGacha
        };
        GoToGacha(replayGachas);
    }

    protected void HandleNextGameFlow()
    {
        currentLevel++;

        if (currentLevel >= gameFlow.Count)
        {
            GoToThanksForPlaying();
            return;
        }

        LevelAction next = gameFlow[currentLevel];

        Debug.Log($"Handling next action {currentLevel} called {next.name}");

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
            List<LootBoxRoll> gachas = next.lootBoxRoll;

            if (gameplayLootboxes != null && gameplayLootboxes.Count > 0)
            {
                gachas.AddRange(gameplayLootboxes);
            }

            if (gachas.Count == 0)
            {
                // skip
                HandleNextGameFlow();
                return;
            }
            GoToGacha(gachas);
            // Optional ADD GAMEPLAY ITEMS
        }
        else if (next.actionType == ActionType.PIGEON)
        {
            // Pigeon remove inventory
            for (int i = 0; i < next.pigeonDiscussion.Count; i++)
            {
                PigeonDiscussion pd = next.pigeonDiscussion[i];

                for (int j = 0; j < pd.removedInventory.Count; j++)
                {
                    if (inventory.ContainsKey(pd.removedInventory[j])) inventory.Remove(pd.removedInventory[j]);
                }
            }

            // Somehow run each pigeon discussion.
            List<PigeonDiscussion> cheeps = next.pigeonDiscussion;
            GoToPigeon(cheeps);
        }
    }

    protected void GoToThanksForPlaying()
    {
        StartCoroutine(LoadLevel(thanksForPlaying));
    }

    protected void GoToGacha(List<LootBoxRoll> lootBox)
    {
        localSoundtrack.GoToTheme(GameTheme.NONE);
        this.lootBoxMessage = lootBox;
        StartCoroutine(LoadLevel(gachalevel));
    }

    protected void GoToPigeon(List<PigeonDiscussion> pigeonDiscussion)
    {
        localSoundtrack.GoToTheme(GameTheme.PIGEON);
        this.pigeonMessage = pigeonDiscussion;
        StartCoroutine(LoadLevel(pigeonlevel));
    }

    protected void GoToGameplay(string level)
    {
        localSoundtrack.GoToTheme(GameTheme.GAMEPLAY);
        StartCoroutine(LoadLevel(level));
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
    // Not used by code but shows up in editor
    public string name;
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
