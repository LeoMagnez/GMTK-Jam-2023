using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Paladin : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth;
    public float cooldown = 1f;
    public float baseCooldown = 1f;
    public float attack = 50f;
    public float baseAttack = 50f;
    public float defense = 10f;
    public float baseDefense = 10f;
    public bool melee;

    public bool isAlive;

    public float exp;

    public int attackIndex = 0;

    public ParticleSystem attackParticle;
    public ParticleSystem deadParticle;
    public TextMeshPro HPtext;

    public int breakArmor;
    [HideInInspector]
    public float breakArmorRatio;
    public int slowEffect;
    [HideInInspector]
    public float slowEffectRatio;
    public int precisionDebuff;

    public bool backtoRoom;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        HPtext.SetText(health.ToString());

        breakArmor = 0;
        slowEffect = 0;

        health = maxHealth;
        defense = baseDefense;
        cooldown = baseCooldown;
        attack = baseAttack;

        breakArmorRatio = 1f;
        slowEffectRatio = 1f;

        backtoRoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyStatus();

        if(backtoRoom)
        {
            mort();
        }

        if(health <= 0)
        {
            mort();
        }
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
        attack = baseAttack;
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

                if (precisionDebuff > 0)
                {
                    if(Random.Range(0, 100) > 75)
                    {
                        attack = 0;
                    }
                    
                }

                FightManager.instance.attacking = true;

                Attack();
                
                timer = cooldown;
            }
            
            yield return null;
        }
    }

    public void ApplyStatus()
    {
        if (breakArmor > 0)
        {
            defense = baseDefense * breakArmorRatio;
        }
        else
        {
            defense = baseDefense;
        }

        if (slowEffect > 0)
        {
            cooldown = baseCooldown * slowEffectRatio;

        }
        else
        {
            cooldown = baseCooldown;
        }

        //if(precisionDebuff > 0)
        //{
        //    attack = baseAttack * Random.Range(0, 1);
        //}
        //else
        //{
        //    attack = baseAttack;
        //}
    }

    public void mort()
    {
        backtoRoom = false;
        health = maxHealth;

        //if (FightManager.instance.allies != null)
        //{
        //    FightManager.instance.allies.Clear();
        //}


        Gamemanager.instance.Paladead = true;
        
        FightManager.instance.CheckForEndOfCombat();
        //RoomManager.instance.ChangeCurrentRoom(MapManager.instance.startRoom);
    }
}
