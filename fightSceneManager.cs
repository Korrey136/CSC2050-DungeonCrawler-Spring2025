using System;
using UnityEngine;

// Class to manage the fight scene
public class FightSceneManager: MonoBehaviour
{
    // Start is called once before the first execution of Update
    void Start()
    {
        Fight fight = new Fight();
        fight.StartFight();
    }

    // Update is called once per frame
    void Update()
    {
        // Currently no functionality here
    }
}

// Class representing a fight between two characters
public class Fight
{
    private Character attacker;
    private Character defender;

    public Fight()
    {
        // Initialize characters for the fight
        attacker = new Character("Attacker", 100, 15);
        defender = new Character("Defender", 120, 10);
    }

    public void StartFight()
    {
        Debug.Log("The fight begins!");

        // Fight loop until one character is defeated
        while (attacker.IsAlive && defender.IsAlive)
        {
            // Attacker attacks the defender
            attacker.Attack(defender);
            Debug.Log($"{defender.Name} has {defender.Health} health left.");

            if (!defender.IsAlive)
            {
                Debug.Log($"{attacker.Name} wins!");
                break;
            }

            // Defender retaliates
            defender.Attack(attacker);
            Debug.Log($"{attacker.Name} has {attacker.Health} health left.");

            if (!attacker.IsAlive)
            {
                Debug.Log($"{defender.Name} wins!");
                break;
            }
        }
    }
}

// Class representing a character in the fight
public class Character
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int AttackPower { get; private set; }

    public bool IsAlive => Health > 0;

    public Character(string name, int health, int attackPower)
    {
        Name = name;
        Health = health;
        AttackPower = attackPower;
    }

    public void Attack(Character target)
    {
        Debug.Log($"{Name} attacks {target.Name} for {AttackPower} damage!");
        target.TakeDamage(AttackPower);
    }

    private void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0; // Prevent negative health
    }
}
