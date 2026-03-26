using UnityEngine;

public class HealItem : IItem
{
    private readonly float healAmount;

    public HealItem(float healAmount = 0.1f)
    {
        this.healAmount = healAmount;
    }

    public void Apply(GameDirector director)
    {
        if (director == null)
        {
            Debug.LogWarning("GameDirector is null. Heal not applied.");
            return;
        }

        director.PauseScore();
        director.HealPlayer(healAmount);
    }
}