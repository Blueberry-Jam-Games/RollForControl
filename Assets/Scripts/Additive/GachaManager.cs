using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random=UnityEngine.Random;


public class GachaManager : MonoBehaviour
{
    public List<RolledItem> gachaItemPool;
    public Animator lootBoxAnimation;
    public float lootBoxAnimationTime;
    public RolledItem rewardedItem;

    public TextMeshPro lootboxQty;

    public TextMeshPro lootboxType;

    private FlowManager fManager;

    public TextMeshPro lootBoxItemDisplayName;

    public LootBoxRoll waifuLootbox;

    [SerializeField]
    private AudioSource preLootbox;

    [SerializeField]
    private AudioSource rollSound;

    [SerializeField]
    private GameObject instructions;

    private void Start()
    {
        // fManager = FindObjectOfType<FlowManager>();
        fManager = FlowManager.Instance;
        rewardedItem.gameObject.SetActive(false);
        lootBoxItemDisplayName.gameObject.SetActive(false);
        List<LootBoxRoll> rolls = FlowManager.Instance.lootBoxMessage;
        StartCoroutine(PullLootBoxes(rolls));
    }

    private IEnumerator PullLootBoxes(List<LootBoxRoll> rolls)
    {
        preLootbox.Play();
        yield return null;

        lootboxQty.text = System.Convert.ToString(rolls.Count);
        
        //Loop for multiple lootboxes
        for (int i = 0; i < rolls.Count; i++)
        {
            lootboxType.text = rolls[i].LootBoxName;
            //Start of animation for lootboxes
            yield return DoRoll(rolls[i], rolls.Count - i, rolls, i == 0);
        }

        // end
        FlowManager.Instance.AddativeSceneDone();
    }

    private IEnumerator DoRoll(LootBoxRoll roll, int remainingLootboxes, List<LootBoxRoll> rolls, bool requireInput)
    {
        rewardedItem.Reset();
        lootBoxItemDisplayName.gameObject.SetActive(false);
        lootBoxAnimation.Rebind();
        lootBoxAnimation.Update(0);
        int rollables = roll.rolls.Count;
        // initialize
        for (int i = 0; i < gachaItemPool.Count; i++)
        {
            if (i < rollables)
            {
                gachaItemPool[i].gameObject.SetActive(true);
                gachaItemPool[i].Initialize(roll.rolls[i]);
                gachaItemPool[i].Reset();
            }
            else
            {
                gachaItemPool[i].gameObject.SetActive(false);
            }
        }

        //yield return new WaitForSeconds(1);

        instructions.SetActive(true);
        while (requireInput && !Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        instructions.SetActive(false);

        preLootbox.Stop();
        rollSound.Play();

        lootboxQty.text = System.Convert.ToString(remainingLootboxes - 1);

        // play animations
        for (int i = 0; i < gachaItemPool.Count; i++)
        {
            if (gachaItemPool[i].gameObject.activeInHierarchy)
            {
                gachaItemPool[i].Play();
            }
        }
        
        yield return new WaitForSeconds(gachaItemPool[0].Durration);

        lootBoxAnimation.Play("LootBoxSpin");

        //Calculate won item
        LootBoxItem reward = RolledItem(roll);
        rewardedItem.Initialize(reward);

        yield return new WaitForSeconds(lootBoxAnimationTime);

        rewardedItem.gameObject.SetActive(true);
        rewardedItem.Play(false);
        rewardedItem.displayObject.sprite = reward.image;

        yield return new WaitForSeconds(rewardedItem.Durration);

        lootBoxItemDisplayName.gameObject.SetActive(true);
        lootBoxItemDisplayName.text = rewardedItem.GetName();

        if(rewardedItem.GetName() == "Waifu Lootbox")
        {
            rolls.Add(waifuLootbox);
            lootboxQty.text = System.Convert.ToString(remainingLootboxes);
        }
        else
        {
            AddToInventory(rewardedItem.GetName());
            Debug.Log("Added Item \n" + rewardedItem.GetName() + "  " + fManager.inventory[rewardedItem.GetName()]);
        }

        preLootbox.Play();

        instructions.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        instructions.SetActive(false);
    }

    private void AddToInventory(string name)
    {
        if(fManager.inventory.ContainsKey(name))
        {
            if(fManager.inventory[name] == false)
            {
                fManager.inventory.Add(name, true);
            }
        }
        else
        {
            fManager.inventory.Add(name, true);
        }
    }

    private LootBoxItem RolledItem(LootBoxRoll lbr)
    {
        List<LootBoxItem> items = lbr.rolls;

        float sum = 0;
        for (int i = 0, count = items.Count; i < count; i++)
        {
            sum += items[i].percentage;
        }

        float select = Random.Range(0.0001f, sum);

        for (int i = 0, count = items.Count; i < count; i++)
        {
            LootBoxItem item = items[i];
            select -= item.percentage;
            if (select <= 0)
            {
                return item;
            }
        }

        // last item
        return items[^1];
    }

    private void Update()
    {
        
    }
}
