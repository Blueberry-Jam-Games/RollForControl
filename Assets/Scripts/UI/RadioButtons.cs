using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioButtons : MonoBehaviour
{
    private List<Toggle> controlled;

    private void Start()
    {
        controlled = new List<Toggle>();

        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.TryGetComponent<Toggle>(out Toggle got))
            {
                controlled.Add(got);
            }
        }
    }

    public int GetSelected()
    {
        for (int i = 0; i < controlled.Count; i++)
        {
            if (controlled[i].isOn)
            {
                return i;
            }
        }

        Debug.LogError($"Radio Button Set {gameObject.name} has nothing active, this is a break");
        return 0;
    }

    public void SelectValue(int toSelect)
    {
        if (toSelect >= controlled.Count)
        {
            Debug.LogError($"Radio Button Set {gameObject.name} told to set slot {toSelect} with only {controlled.Count} slots, this is a break");
            return;
        }

        controlled[toSelect].isOn = true;
    }
}
