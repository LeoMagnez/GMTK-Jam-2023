using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troops : MonoBehaviour
{
    public float health = 100f;
    public float cooldown = 1f;
    public float attack = 50f;
    public float defense = 10f;
    public bool melee;

    public int attackIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        Debug.Log(gameObject.name + " is attacking");

        if(gameObject.name == "Paladin")
        {
            
            bool temp = true;

            while (temp && attackIndex < FightManager.instance.allies.Length)
            {
                if(FightManager.instance.allies[attackIndex] != null)
                {
                    if (FightManager.instance.allies[attackIndex].GetComponent<Troops>().health > 0)
                    {
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

            if (attackIndex >= FightManager.instance.allies.Length)
            {
                Debug.Log("No more enemies");
                attackIndex++;
            }




            //if (FightManager.instance.allies[0] != null)
            //{
            //    Debug.Log(FightManager.instance.allies[0].gameObject.name + " is under attack");

            //    FightManager.instance.allies[0].GetComponent<Troops>().TakeDamage(attack);
            //}
        }

        else
        {
            FightManager.instance.paladin.GetComponent<Troops>().TakeDamage(attack);
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
        float tempDamage = damage * (1 - (defense / 100f));

        health -= tempDamage;

        Debug.Log(gameObject.name + "'s health =" + health);
    }
}
