using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    void Start()
    {
        LootBoxRoll lbr = FlowManager.Instance.lootBoxMessage;
        LootBoxItem foundItem = RolledItem(lbr);

        // Do what you will.
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

    void Update()
    {
        
    }
}
