using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Paladin : MonoBehaviour
{
    public float health = 100f;
    public float cooldown = 1f;
    public float attack = 50f;
    public float defense = 10f;
    public bool melee;

    public bool isAlive;

    public float exp;

    public int attackIndex = 0;

    public ParticleSystem attackParticle;
    public ParticleSystem deadParticle;
    public TextMeshPro HPtext;
    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        HPtext.SetText(health.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (gameObject.name == "Paladin")
        {

            bool temp = true;

            while (temp && attackIndex < FightManager.instance.allies.Count)
            {
                if (FightManager.instance.allies[attackIndex] != null)
                {
                    if (FightManager.instance.allies[attackIndex].GetComponent<Troops>().health > 0)
                    {
                        VisualAttackEffects();
                        FightManager.instance.allies[attackIndex].GetComponent<Troops>().TakeDamage(attack);

                        temp = false;
                    }
                    else
                    {

                        Debug.Log(FightManager.instance.allies[attackIndex].gameObject.name + " is dead");
                        attackIndex++;
                    }

                }
            }

            if (attackIndex >= FightManager.instance.allies.Count)
            {
                attackIndex++;
                FightManager.instance.CheckForEndOfCombat();

            }




            //if (FightManager.instance.allies[0] != null)
            //{
            //    Debug.Log(FightManager.instance.allies[0].gameObject.name + " is under attack");

            //    FightManager.instance.allies[0].GetComponent<Troops>().TakeDamage(attack);
            //}
        }

        StartCoroutine(AttackCR());
    }

    public IEnumerator AttackCR()
    {

        yield return new WaitForSeconds(1f);

        FightManager.instance.attacking = false;
    }

    public void VisualAttackEffects()
    {
        attackParticle.Play();
    }

    public void VisualDeadEffect()
    {
        deadParticle.Play();
    }

    public void TakeDamage(float damage)
    {
        if (isAlive)
        {
            float tempDamage = damage * (1 - (defense / 100f));

            if (health > 0)
            {
                health -= tempDamage;
            }

            if (health <= 0)
            {
                VisualDeadEffect();
                health = 0;
                isAlive = false;
            }

            HPtext.SetText(health.ToString());
        }

    }

    public IEnumerator PaladinLogic()
    {
        float timer = cooldown;
        while (health > 0)
        {

            while (timer > 0)
            {
                if (!FightManager.instance.attacking)
                {
                    timer -= Time.deltaTime;
                }

                yield return null;
            }

            if (health > 0 && attackIndex <= FightManager.instance.allies.Count)
            {
                //Debug.Log(entity.GetComponent<Troops>().attackIndex);
                FightManager.instance.attacking = true;

                Attack();

                timer = cooldown;
            }

            yield return null;
        }
    }
}
