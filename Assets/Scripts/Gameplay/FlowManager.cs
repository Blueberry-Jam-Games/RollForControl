using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    private static FlowManager _instance;
    public static FlowManager Instance { get => _instance; }

    [SerializeField]
    private string gachalevel;

    [SerializeField]
    private string pigeonlevel;

    // TODO gameplay levels

    [SerializeField]
    private Animator levelAnimator;

    // Used to communicate data from 1 scene to the next, if you get to gacha or pigeon just assume it exists and keep going
    [HideInInspector]
    public LootBoxRoll lootBoxMessage;

    [HideInInspector]
    public PigeonDiscussion pigeonMessage;

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
    }

    public void TitleScreenEnter()
    {
        LootBoxRoll lbr = new LootBoxRoll();
        lbr.rolls = new List<LootBoxItem>();
        GoToGacha(lbr);
    }

    public void GoToGacha(LootBoxRoll lootBox)
    {
        this.lootBoxMessage = lootBox;
        StartCoroutine(LoadLevel(gachalevel));
    }

    public void GoToPigeon(PigeonDiscussion pigeonDiscussion)
    {
        this.pigeonMessage = pigeonDiscussion;
        StartCoroutine(LoadLevel(pigeonlevel));
    }

    public void GoToGameplay(int level)
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
}
