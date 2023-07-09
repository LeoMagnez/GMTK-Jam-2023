using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Cards", order = 1)]
public class CardData : ScriptableObject
{
    // Start is called before the first frame update
    public GameObject generatedTroup;
    public Sprite art;
    public int tier;
    public int rarity;
    public int price;
    public CardData evolvedCard;

    public CardDescriptor[] tokens;


}
