using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class HandManager : MonoBehaviour
{
    public List<GameObject> cardsDisplay = new List<GameObject>();
    public List<CardData> cardsInHand = new List<CardData>();
    public GameObject returnToDeck;
    public AnimationCurve cardSpacingCurve;
    public int cardCount;
    bool onHandHud = false;
    public float selectedRatio = 0f;
    public Vector2 bounds = Vector2.zero;
    public bool cardInSelection;
    public int selectedCard;
    public Transform[] anchors;
    public Vector3 offset;
    public static HandManager instance;
    public Room currentRoomEdit;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!cardInSelection)
        {
            float minX = Camera.main.WorldToScreenPoint(anchors[0].position).x;
            float maxX = Camera.main.WorldToScreenPoint(anchors[1].position).x;

            selectedRatio = (Input.mousePosition.x - minX) / (maxX - minX);
            selectedCard = Mathf.FloorToInt(cardCount * selectedRatio);
            selectedCard = Mathf.Clamp(selectedCard, 0, cardCount - 1);
            for (int i = 0; i < cardsDisplay.Count; i++)
            {
                if (i == selectedCard)
                {
                    cardsDisplay[i].GetComponent<Renderer>().material.color = Color.white;
                }
                else
                {
                    cardsDisplay[i].GetComponent<Renderer>().material.color = Color.gray;
                }
                if (i >= cardCount)
                {
                    cardsDisplay[i].SetActive(false);

                }
                else
                {
                    cardsDisplay[i].SetActive(true);
                }
            }
            UpdateCardPlacement();
        }
        else
        {
            Vector3 slerpOffset = Camera.main.transform.position - Camera.main.transform.forward * 1f;
            float maxSpace = cardsDisplay[0].transform.localScale.x * 10f * 6f / 2;
            float minSpace = cardsDisplay[0].transform.localScale.x * 10f * 2f / 2;
            float currentSpace = Mathf.Lerp(minSpace, maxSpace, cardSpacingCurve.Evaluate(cardCount / 15f));
            anchors[0].position = new Vector3(currentSpace * -1, anchors[0].position.y, anchors[0].position.z);
            anchors[1].position = new Vector3(currentSpace, anchors[0].position.y, anchors[0].position.z);
            for (int i = 0; i < cardCount; i++)
            {
                Vector3 targetPos = Vector3.zero;
                if (i != selectedCard)
                {
                    targetPos = Vector3.Slerp(anchors[0].position + new Vector3(0, -0.5f, -0.1f) - slerpOffset, anchors[1].position + new Vector3(0, -0.5f, -0.1f) - slerpOffset, (i + 1) / (cardCount + 1f)) + slerpOffset;
                    cardsDisplay[i].transform.position = Vector3.Lerp(cardsDisplay[i].transform.position, targetPos, Time.deltaTime * 10f);
                }

            }
        }
        
        if (Input.GetMouseButtonDown(0) && !cardInSelection && cardCount != 0)
        {
            cardInSelection = true;
            StartCoroutine(FocusOnCard(cardsDisplay[selectedCard]));

        }
        if (cardInSelection && Input.GetKeyDown(KeyCode.W))
        {
            AddCardToRoom();
        }
        UpdateHand();
    }

    public void AddCardToRoom()
    {
        currentRoomEdit.AddToRoom(cardsInHand[selectedCard]);
        StartCoroutine(PoseCard(cardsDisplay[selectedCard]));
    }
    IEnumerator PoseCard(GameObject card)
    {
        bool inAnim = true;
        while (inAnim)
        {
            if (Vector3.Distance(card.transform.position, anchors[3].position) < 0.01f)
            {
                inAnim = false;
            }
            card.transform.position = Vector3.Lerp(card.transform.position, anchors[3].position, Time.deltaTime * 10f);
            yield return null;



        }
        GameObject ghost = Instantiate(card, card.transform.position, Quaternion.identity);
        ghost.GetComponent<CardDisplay>().onTable = true;
        cardsDisplay.RemoveAt(selectedCard);
        cardsInHand.RemoveAt(selectedCard);
        cardsDisplay.Add(card);
        cardInSelection = false;
        returnToDeck.SetActive(false);
    }

    public void DrawCard(CardData card)
    {
        cardsInHand.Add(card);
    }
    public void UpdateHand()
    {
        cardCount = cardsInHand.Count;
        for (int i = 0; i < cardCount; i++)
        {


            cardsDisplay[i].GetComponent<CardDisplay>().UpdateData(cardsInHand[i]);
        }
    }

    IEnumerator FocusOnCard(GameObject card)
    {
        bool inAnim = true;
        while(inAnim)
        {
            if(Vector3.Distance(card.transform.position, anchors[2].position) < 0.01f){
                inAnim = false;
            }
            card.transform.position = Vector3.Lerp(card.transform.position, anchors[2].position, Time.deltaTime*10f);
            yield return null;



        }
        returnToDeck.SetActive(true);
    }
    public void UnfocusCard()
    {
        if (cardInSelection)
        {
            returnToDeck.SetActive(false);
            cardInSelection = false;
        }

    }

    private void UpdateCardPlacement()
    {
        cardCount = Mathf.Clamp(cardCount, 0, 8);
        Vector3 slerpOffset = Camera.main.transform.position - Camera.main.transform.forward * 1f;
        float maxSpace = cardsDisplay[0].transform.localScale.x * 10f * 6f / 2;
        float minSpace = cardsDisplay[0].transform.localScale.x * 10f * 2f / 2;
        float currentSpace = Mathf.Lerp(minSpace, maxSpace, cardSpacingCurve.Evaluate(cardCount / 15f));
        anchors[0].position= new Vector3(currentSpace * -1, anchors[0].position.y, anchors[0].position.z);
        anchors[1].position= new Vector3(currentSpace, anchors[0].position.y, anchors[0].position.z);
        for (int i = 0; i < cardCount; i++)
        {
            Vector3 targetPos = Vector3.zero;
            if(i != selectedCard)
            {
                 targetPos = Vector3.Slerp(anchors[0].position - slerpOffset, anchors[1].position - slerpOffset, (i + 1) / (cardCount + 1f))+ slerpOffset;
            }
            else
            {
                 targetPos = Vector3.Slerp(anchors[0].position + new Vector3(0, 0.1f, -0.1f)- slerpOffset, anchors[1].position + new Vector3(0, 0.1f, -0.1f)- slerpOffset, (i + 1) / (cardCount + 1f))+ slerpOffset;
            }
            cardsDisplay[i].transform.position = Vector3.Lerp(cardsDisplay[i].transform.position, targetPos, Time.deltaTime * 10f);
        }
    }
    public void ChangeState(bool newState)
    {
        onHandHud = newState;
    }
}
