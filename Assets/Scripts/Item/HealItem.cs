using UnityEngine;

public class HealItem : IItem
{
    private readonly float healAmount;

    public HealItem(float healAmount = 0.1f)
    {
        this.healAmount = healAmount;
    }

    public void hitPlayer()
    {
        Debug.Log("HealItem strategy: apply heal to player");

        // Try find GameDirector by tag
        GameObject gdObj = GameObject.FindWithTag("director");
        if (gdObj != null)
        {
            GameDirector gd = gdObj.GetComponent<GameDirector>();
            if (gd != null)
            {
                gd.PauseScore();
                gd.IncreaseHp(healAmount);
                return;
            }
        }

        Debug.LogWarning("GameDirector not found. Heal not applied.");
    }
}
