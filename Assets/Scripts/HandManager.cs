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
    public GameObject returnToMap;
    public GameObject poseCard;
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
    bool inFocus;


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
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        inFocus = (Input.mousePosition.y < Screen.height * 0.4f);

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
                        //cardsDisplay[i].GetComponent<Renderer>().material.color = Color.white;
                    }
                    else
                    {
                        //cardsDisplay[i].GetComponent<Renderer>().material.color = Color.light;
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

            UpdateCardPlacement(inFocus);

            
            
        }
        else
        {
            Vector3 slerpOffset = Camera.main.transform.position - Camera.main.transform.forward * 0.8f;
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
        
        if (Input.GetMouseButtonDown(0) && !cardInSelection && cardCount != 0 && inFocus)
        {
            cardInSelection = true;
            StartCoroutine(FocusOnCard(cardsDisplay[selectedCard]));

        }
        UpdateHand();
    }

    public void PlaceButton()
    {
        if (cardInSelection)
        {
            AddCardToRoom();
        }
    }

    public void Init()
    {
        returnToMap.SetActive(true);
        foreach (GameObject item in cardsDisplay)
        {
            item.transform.position = Camera.main.transform.position - new Vector3(0, 3, 1);
        }
    }
    public void Close()
    {
        returnToMap.SetActive(false);

    }

    public void AddCardToRoom()
    {
        MapManager.instance.currentRoomInEdit.AddToRoom(cardsInHand[selectedCard]);
        StartCoroutine(PoseCard(cardsDisplay[selectedCard]));
    }
    IEnumerator PoseCard(GameObject card)
    {
        bool inAnim = true;
        returnToDeck.SetActive(false);
        poseCard.SetActive(false);
        Quaternion randRot = Quaternion.Euler(0,Random.Range(0,360),0);
        Vector3 randOffset = new Vector3(Random.Range(-1.2f,1.2f), 0, Random.Range(-0.5f, 0.5f));
        while (inAnim)
        {
            if (Vector3.Distance(card.transform.position, anchors[3].position + randOffset) < 0.01f && Quaternion.Angle(card.transform.rotation, randRot)<3f)
            {
                inAnim = false;
            }
            card.transform.position = Vector3.Lerp(card.transform.position, anchors[3].position + randOffset, Time.deltaTime * 10f);
            card.transform.rotation = Quaternion.Lerp(card.transform.rotation, randRot,Time.deltaTime * 10f);
            Debug.Log(Quaternion.Angle(card.transform.rotation, randRot) < 3f);
            yield return null;



        }
        GameObject ghost = Instantiate(card, card.transform.position, card.transform.rotation);
        ghost.transform.SetParent(MapManager.instance.currentRoomInEdit.cardHolder.transform,true);
        ghost.GetComponent<CardDisplay>().onTable = true;
        card.transform.position = Camera.main.transform.position - new Vector3(0, 3, 1);
        card.transform.localRotation = Quaternion.Euler(-90f, 0, 0);
        cardsDisplay.RemoveAt(selectedCard);
        cardsInHand.RemoveAt(selectedCard);
        cardsDisplay.Add(card);
        cardInSelection = false;

        returnToMap.SetActive(true);
        
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
    public int GetNumberOfCard()
    {
        return cardsInHand.Count;
    }
    IEnumerator FocusOnCard(GameObject card)
    {
        returnToMap.SetActive(false);
        bool inAnim = true;
        while(inAnim)
        {
            if(Vector3.Distance(card.transform.position, anchors[2].position) < 0.01f){
                inAnim = false;
            }
            card.transform.position = Vector3.Lerp(card.transform.position, anchors[2].position, Time.deltaTime*10f);
            yield return null;



        }
        poseCard.SetActive(true);
        //anim de bouton
        returnToDeck.SetActive(true);
    }
    public void UnfocusCard()
    {
        
        if (cardInSelection)
        {
            returnToMap.SetActive(true);
            returnToDeck.SetActive(false);
            cardInSelection = false;
            poseCard.SetActive(false);
        }
        
    }

    private void UpdateCardPlacement(bool focus)
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
                if (focus)
                {
                    targetPos = Vector3.Slerp(anchors[0].position + new Vector3(0, 0.1f, -0.15f) - slerpOffset, anchors[1].position + new Vector3(0, 0.1f, -0.15f) - slerpOffset, (i + 1) / (cardCount + 1f)) + slerpOffset;
                }
                else
                {
                    targetPos = Vector3.Slerp(anchors[0].position - slerpOffset, anchors[1].position - slerpOffset, (i + 1) / (cardCount + 1f)) + slerpOffset;
                }
            }
            cardsDisplay[i].transform.position = Vector3.Lerp(cardsDisplay[i].transform.position, targetPos, Time.deltaTime * 10f);
        }
    }
    public void ChangeState(bool newState)
    {
        onHandHud = newState;
    }
}
