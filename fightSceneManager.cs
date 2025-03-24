using System;
using UnityEngine;

public class Fight
{
    private Character attacker;
    private Character defender;
    private float attackCooldown = 1f; // Time between attacks (in seconds)
    private float timeSinceLastAttack = 0f; // Time tracker

    public Fight(Character attacker, Character defender)
    {
        this.attacker = attacker;
        this.defender = defender;
    }

    public void UpdateFight(float deltaTime)
    {
        timeSinceLastAttack += deltaTime;

        if (timeSinceLastAttack >= attackCooldown)
        {
            PerformAttack();
            timeSinceLastAttack = 0f;
        }
    }

    private void Perform Attack()
    {
        defender.CurrentHealth -= attacker.AttackPower;

        if (defender.CurrentHealth <= 0)
        {
            Debug.Log($"{defender.Name} has been defeated!");
        }
    }

    public bool IsFightOver()
    {
        return attacker.CurrentHealth <= 0 || defender.CurrentHealth <= 0;
    }
}
