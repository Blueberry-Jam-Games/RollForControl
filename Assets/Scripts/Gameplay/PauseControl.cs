using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    private static PauseControl _instance;
    public static PauseControl Instance { get => _instance; }

    private bool pauseGlobal;
    private List<Pausable> pausables;
    private HashSet<byte> pausedChannels;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        pauseGlobal = false;
        pausables = new List<Pausable>();
    }

    public void RegisterPausable(Pausable item)
    {
        pausables.Add(item);
    }

    public void RemovePausable(Pausable item)
    {
        pausables.Remove(item);
    }

    public void PauseLayer(byte layer)
    {
        if (!pausedChannels.Contains(layer)) pausedChannels.Add(layer);
    }

    public void UnpauseLayer(byte layer)
    {
        if (pausedChannels.Contains(layer)) pausedChannels.Remove(layer);
    }

    public void PauseGlobal()
    {
        pauseGlobal = true;
    }

    public void UnpauseGlobal()
    {
        pauseGlobal = false;
    }

    private void Update()
    {
        if (!pauseGlobal)
        {
            for (int i = 0, count = pausables.Count; i < count; i++)
            {
                Pausable current = pausables[i];
                if (!pausedChannels.Contains(current.Layer))
                {
                    current.PausableUpdate();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!pauseGlobal)
        {
            for (int i = 0, count = pausables.Count; i < count; i++)
            {
                Pausable current = pausables[i];
                if (!pausedChannels.Contains(current.Layer))
                {
                    current.PausableFixedUpdate();
                }
            }
        }
    }
}

public interface Pausable
{
    public byte Layer {get;}
    public void PausableUpdate();
    public void PausableFixedUpdate();
}
