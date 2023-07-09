using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] CardData currentData;
    [SerializeField] Sprite[] raritySprites;
    [SerializeField] Sprite[] tierSprites;
    [SerializeField] Sprite[] priceSprites;
    public Image[] tokens; 
    public Image artwork;
    public Image tier;
    public Image rarity;
    public Image price;
    public bool onTable;
    //public TextMeshProUGUI nameText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateData(CardData newData)
    {
        currentData = newData;
        for (int i = 0; i < tokens.Length; i++)
        {
            if(currentData.tokens.Length > i)
            {
                tokens[i].sprite = currentData.tokens[i].cardImage;
            }
            else
            {
                tokens[i].sprite = null;
            }

        }
        tier.sprite = tierSprites[currentData.tier];
        rarity.sprite = raritySprites[currentData.rarity];
        price.sprite = priceSprites[currentData.price-1];
        
    }
    private void Update()
    {
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PickUp();
            // Whatever you want it to do.
        }
    }
    public void PickUp()
    {
        if(onTable && !HandManager.instance.cardInSelection)
        {
            HandManager.instance.DrawCard(currentData);
            MapManager.instance.currentRoomInEdit.RemoveFromRoom(currentData);
            Destroy(gameObject);
        }

    }
}
