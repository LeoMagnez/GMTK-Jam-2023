using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Troops : MonoBehaviour
{
    public float health = 100f;
    public float baseHealth = 100f;
    public float cooldown = 1f;
    public float attack = 50f;
    public float defense = 10f;
    public bool melee;

    public float statusStr;

    public int attackIndex = 0;

    public ParticleSystem attackParticle;
    public ParticleSystem deadParticle;

    public enum troopType
    {
        Gobelin,
        Troll,
        Mortier,
        ElementaireFeu,
        ElementaireEau,
        ElementaireSlime,
        ElementaireAlcohol,
        FireMage,
        Paladin,
    }

    public troopType type;

    public int chanceToInflictStatus;
    public float healingAmount;

    public bool isAlive;

    //public TextMeshPro HPtext;

    // Start is called before the first frame update
    void Start()
    {
        //HPtext.SetText(health.ToString());
        isAlive = true;
        health = baseHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {
        
        //Debug.Log(gameObject.name + " is attacking");

        //if(gameObject.name == "Paladin")
        //{
            
        //    bool temp = true;

        //    while (temp && attackIndex < FightManager.instance.allies.Count)
        //    {
        //        if(FightManager.instance.allies[attackIndex] != null)
        //        {
        //            if (FightManager.instance.allies[attackIndex].GetComponent<Troops>().health > 0)
        //            {
        //                VisualAttackEffects();
        //                FightManager.instance.allies[attackIndex].GetComponent<Troops>().TakeDamage(attack);
                        
        //                temp = false;
        //            }
        //            else
        //            {
                        
        //                Debug.Log(FightManager.instance.allies[attackIndex].gameObject.name + " is dead");
        //                attackIndex++;
        //            }
                    
        //        }
        //    }

        //    if (attackIndex >= FightManager.instance.allies.Count)
        //    {
        //        attackIndex++;
        //        FightManager.instance.CheckForEndOfCombat();
                
        //    }




        //    //if (FightManager.instance.allies[0] != null)
        //    //{
        //    //    Debug.Log(FightManager.instance.allies[0].gameObject.name + " is under attack");

        //    //    FightManager.instance.allies[0].GetComponent<Troops>().TakeDamage(attack);
        //    //}
        //}

        if (health > 0)
        {
            VisualAttackEffects();
            

            SelectAttack();

        }

        StartCoroutine(AttackCR());
    }

    public IEnumerator AttackCR()
    {

        yield return new WaitForSeconds(1f);

        FightManager.instance.attacking = false;
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

            //HPtext.SetText(health.ToString());
        }

    }

    public void VisualAttackEffects()
    {
        attackParticle.Play();
    }

    public void VisualDeadEffect()
    {
        deadParticle.Play();
    }

    public void SelectAttack()
    {
        switch(type)
        {
            case troopType.Mortier:
                AttackAllMelee();
                break;

            case troopType.Gobelin:
                MonoTarget();
                break;

            case troopType.FireMage:
                AttackEveryone();
                break;

            case troopType.Troll:
                MonoTarget();
                break;

            case troopType.ElementaireFeu:
                MonoTarget();
                if (InflictStatus())
                {
                    if(Gamemanager.instance.paladin.GetComponent<Paladin>().breakArmorRatio >= statusStr)
                    {
                        Gamemanager.instance.paladin.GetComponent<Paladin>().breakArmor = 2;
                        Gamemanager.instance.paladin.GetComponent<Paladin>().breakArmorRatio = statusStr;
                    }
                    
                }
                break;

            case troopType.ElementaireSlime:
                MonoTarget();
                if (InflictStatus())
                {
                    if(Gamemanager.instance.paladin.GetComponent<Paladin>().slowEffectRatio <= statusStr)
                    {
                        Gamemanager.instance.paladin.GetComponent<Paladin>().slowEffect = 2;
                        Gamemanager.instance.paladin.GetComponent<Paladin>().slowEffectRatio = statusStr;
                    }
                    
                }
                break;

            case troopType.ElementaireAlcohol:
                MonoTarget();
                if (InflictStatus())
                {
                    Gamemanager.instance.paladin.GetComponent<Paladin>().precisionDebuff = 2;
                }
                break;
            case troopType.ElementaireEau:
                for (int i = 0; i < FightManager.instance.allies.Count; i++)
                {
                    FightManager.instance.allies[i].GetComponent<Troops>().Heal(healingAmount);
                }
                break;



        }
    }

    public void MonoTarget()
    {
        Gamemanager.instance.paladin.GetComponent<Paladin>().TakeDamage(attack);
        //Debug.Log(gameObject.name + " attaque en cible unique");
    }

    public void AttackAllMelee()
    {
        for (int i = 0; i < FightManager.instance.allies.Count; i++)
        {
            if (FightManager.instance.allies[i].GetComponent<Troops>().melee)
            {
                FightManager.instance.allies[i].GetComponent<Troops>().TakeDamage(attack);
            }
        }

        Gamemanager.instance.paladin.GetComponent<Paladin>().TakeDamage(attack);
        //Debug.Log(gameObject.name + " attaque tous les melee");
    }

    public void AttackEveryone()
    {
        for (int i = 0; i < FightManager.instance.allies.Count; i++)
        {
            FightManager.instance.allies[i].GetComponent<Troops>().TakeDamage(attack);
        }

        Gamemanager.instance.paladin.GetComponent<Paladin>().TakeDamage(attack);
        //Debug.Log(gameObject.name + " attaque tout le monde");
    }

    public bool InflictStatus()
    {
        return (Random.Range(0, 100) < chanceToInflictStatus);  
    }

    public void Heal(float ratio)
    {
        health += health * ratio;
        if(health > baseHealth)
        {
            health = baseHealth;
        }
        //HPtext.SetText(health.ToString());
    }
}
