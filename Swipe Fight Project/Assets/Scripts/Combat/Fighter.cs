using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    Animator animator;
    Health health;

    [HideInInspector] public bool isAttacking = false;
    bool isTakingDamage = false;
    bool isImmune = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    public void ExecuteCommand(string direction)
    {
        if (isTakingDamage) return;
        ResetAllTrigger();
        animator.SetTrigger(direction);
    }

    public void TakeDamage(DamageType type)
    {
        if (isImmune) return; // Could set a "Dodge" floating text

        isTakingDamage = true;
        health.TakeDamage(10);

        string trigger = type == DamageType.HEAD ? "headDamage" : "bodyDamage";
        animator.SetTrigger(trigger);
    }

    public void RecoverFromDamage() { isTakingDamage = false; }

    public void ToggleAttack(AnimationEvent evt)
    {
        bool.TryParse(evt.stringParameter, out bool immune);
        isAttacking = immune;
    }

    public void ToggleImmunity(AnimationEvent evt)
    {
        bool.TryParse(evt.stringParameter, out bool immune);
        isImmune = immune;
    }

    private void ResetAllTrigger()
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger) animator.ResetTrigger(param.name);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == gameObject || other.gameObject.tag == "Terrain") return;

        if (other.gameObject.tag == "Head")
        {
            Fighter fighter = other.transform.root.gameObject.GetComponent<Fighter>();
            if (!fighter.isTakingDamage) fighter.TakeDamage(DamageType.HEAD);
        }
        else if (other.gameObject.tag == "Body")
        {
            Fighter fighter = other.transform.root.gameObject.GetComponent<Fighter>();
            if (!fighter.isTakingDamage) fighter.TakeDamage(DamageType.BODY);
        }
    }

    public enum DamageType { HEAD, BODY }
}
