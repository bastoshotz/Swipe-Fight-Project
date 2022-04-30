using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    Fighter self;
    Fighter playerOpponent;

    string[] possibleAttacks = new string[] { "north", "east", "west", "northEast", "northWest", "southEast", "southWest" };

    [SerializeField] private float baseCooldown = 1f;
    [Range(1,3)] public int difficulty = 1;
    private float cooldownIncrement = .75f;
    bool dodgeCooldown = false;
    bool attackCooldown = false;
    bool waitingStart = true;

    void Start()
    {
        self = GetComponent<Fighter>();
        playerOpponent = FindObjectsOfType<Fighter>().Where(f => f.tag == "Player").FirstOrDefault();

        StartCoroutine("WaitStartOfFight");
    }

    void Update()
    {
        if (waitingStart) return;

        if (playerOpponent.isAttacking && !dodgeCooldown) TryToDodge();
        else if (!attackCooldown) Attack();
    }

    private void Attack()
    {
        int idx = Random.Range(0, 6);
        self.ExecuteCommand(possibleAttacks[idx]);
        StartCoroutine("AttackCooldown");
    }

    private void TryToDodge()
    {
        int baseChance = 25 * difficulty;
        bool hasDodged = baseChance >= Random.Range(0, 100);

        if (hasDodged) self.ExecuteCommand("south"); // A.K.A. Dodge

        StartCoroutine("DodgeCooldown");
    }

    private IEnumerator DodgeCooldown()
    {
        dodgeCooldown = true;
        float timer = Time.time + baseCooldown;

        while (Time.time < timer) yield return new WaitForSeconds(1f);

        dodgeCooldown = false;
    }

    private IEnumerator AttackCooldown()
    {
        attackCooldown = true;
        float baseDelay = Time.time + baseCooldown + (cooldownIncrement / difficulty);
        float timer = Random.Range(1, baseDelay + .3f);

        while (Time.time < timer) yield return new WaitForSeconds(1f);

        attackCooldown = false;
    }

    private IEnumerator WaitStartOfFight()
    {
        float timer = Time.time + 2f;

        while (!playerOpponent.isAttacking || Time.time < timer) yield return new WaitForEndOfFrame();

        waitingStart = false;
    }
}
