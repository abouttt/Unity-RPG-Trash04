using System.Collections.Generic;
using UnityEngine;

public class CooldownManager
{
    private readonly HashSet<Cooldown> _cooldowns = new();
    private readonly Queue<Cooldown> _completedCooldownQueue = new();

    public void UpdateCooldowns()
    {
        foreach (var cooldown in _cooldowns)
        {
            cooldown.RemainingTime -= Time.deltaTime;
            if (cooldown.RemainingTime <= 0f)
            {
                cooldown.RemainingTime = 0f;
                _completedCooldownQueue.Enqueue(cooldown);
            }
        }

        while (_completedCooldownQueue.Count > 0)
        {
            var completedCooldown = _completedCooldownQueue.Dequeue();
            _cooldowns.Remove(completedCooldown);
        }
    }

    public void AddCooldown(Cooldown cooldown)
    {
        if (cooldown == null)
        {
            return;
        }

        if (_cooldowns.Add(cooldown))
        {
            cooldown.Start();
        }
    }

    public void Clear()
    {
        foreach (var cooldown in _cooldowns)
        {
            cooldown.Clear();
        }

        _cooldowns.Clear();
        _completedCooldownQueue.Clear();
    }
}
