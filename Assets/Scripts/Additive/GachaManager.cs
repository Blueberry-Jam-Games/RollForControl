using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public List<RolledItem> gachaItemPool;
    public Animator lootBoxAnimation;
    public float lootBoxAnimationTime;
    public RolledItem rewardedItem;

    public TextMeshPro lootboxQty;

    public TextMeshPro lootboxType;

    private void Start()
    {
        // LootBoxRoll lbr = FlowManager.Instance.lootBoxMessage;
        // LootBoxItem foundItem = RolledItem(lbr);

        rewardedItem.gameObject.SetActive(false);
        List<LootBoxRoll> rolls = FlowManager.Instance.lootBoxMessage;
        StartCoroutine(PullLootBoxes(rolls));
    }

    private IEnumerator PullLootBoxes(List<LootBoxRoll> rolls)
    {
        yield return null;

        lootboxQty.text = System.Convert.ToString(rolls.Count);
        
        //Loop for multiple lootboxes
        for (int i = 0, count = rolls.Count; i < count; i++)
        {
            lootboxType.text = rolls[i].LootBoxName;
            //Start of animation for lootboxes
            yield return DoRoll(rolls[i], count - i);
        }
    }

    private IEnumerator DoRoll(LootBoxRoll roll, int remainingLootboxes)
    {
        rewardedItem.Reset();
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

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

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

        LootBoxItem reward = RolledItem(roll);
        rewardedItem.Initialize(reward);

        yield return new WaitForSeconds(lootBoxAnimationTime);

        rewardedItem.gameObject.SetActive(true);
        rewardedItem.Play(false);
        rewardedItem.displayObject.sprite = reward.image;

        yield return new WaitForSeconds(rewardedItem.Durration);

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        while (Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        // TODO idk what else?
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
