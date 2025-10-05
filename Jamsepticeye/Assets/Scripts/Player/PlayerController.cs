using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Cards")]
    public Card card;
    public DeckManager deck;
    public int cardIndex = 0;

    List<Card> aceList = new List<Card>();

    [Header("Hand")]
    public int handValue = 0;
    public GameObject[] hand;

    private int money = 10;

    public void StartHand()
    {
        GetCard();
        GetCard();
    }

    public int GetCard()
    {
        int cardValue = deck.Deal(hand[cardIndex].GetComponent<Card>());

        hand[cardIndex].GetComponent<Renderer>().enabled = true;

        handValue += cardValue;

        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<Card>());
        }

        AceCheck();

        cardIndex++;

        return handValue;
    }

    public void AceCheck()
    {
        foreach (Card ace in aceList)
        {
            if (handValue + 10 < 22 && ace.GetRank() == 1)
            {
                ace.SetRank(11);
                handValue += 10;
            }
            else if (handValue > 21 && ace.GetRank() == 11)
            {
                ace.SetRank(1);
                handValue -= 10;
            }
        }
    }

    public void AdjustMoney(int amount)
    {
        money += amount;
    }

    public int GetMoney()
    {
        return money;
    }

    public void ResetHand()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<Card>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }

        cardIndex = 0;
        handValue = 0;

        aceList = new List<Card>();
    }
}