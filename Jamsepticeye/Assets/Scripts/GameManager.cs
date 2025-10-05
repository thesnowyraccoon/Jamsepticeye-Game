using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;

    [Header("Text")]
    public TMP_Text mainText;
    public TMP_Text standBtnText;
    public TMP_Text scoreText;
    public TMP_Text dealerText;
    public TMP_Text betsText;
    public TMP_Text cashText;

    private int standClicks = 0;

    [Header("Players")]
    public PlayerController player;
    public PlayerController dealer;

    [Header("Deck")]
    public DeckManager deck;

    [Header("Dealer Cards")]
    public GameObject hideCard;

    int pot = 0;

    void Start()
    {
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
    }

    public void OnDeal()
    {
        player.ResetHand();
        dealer.ResetHand();

        mainText.gameObject.SetActive(false);
        dealerText.gameObject.SetActive(false);

        deck.Shuffle();
        player.StartHand();
        dealer.StartHand();

        scoreText.text = "Hand: " + player.handValue.ToString();
        dealerText.text = "Hand: " + dealer.handValue.ToString();

        hideCard.GetComponent<Renderer>().enabled = true;

        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);

        standBtnText.text = "Stand";

        pot = 4;
        betsText.text = "Souls Bet - " + pot.ToString();

        player.AdjustMoney(-2);

        cashText.text = player.GetMoney().ToString() + " souls";
    }

    public void OnHit()
    {
        if (player.cardIndex <= 10)
        {
            player.GetCard();

            scoreText.text = "Hand: " + player.handValue.ToString();

            if (player.handValue > 20) RoundOver();
        }
    }

    public void OnStand()
    {
        standClicks++;

        if (standClicks > 1) RoundOver();

        HitDealer();

        standBtnText.text = "Call";
    }

    public void OnBet()
    {
        TMP_Text newBet = betBtn.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(1));

        player.AdjustMoney(-intBet);

        cashText.text = player.GetMoney().ToString() + " souls";

        pot += intBet * 2;

        betsText.text = "Souls Bet - " + pot.ToString();
    }

    private void HitDealer()
    {
        while (dealer.handValue < 16 && dealer.cardIndex < 10)
        {
            dealer.GetCard();

            dealerText.text = "Hand: " + dealer.handValue.ToString();

            if (dealer.handValue > 20) RoundOver();
        }
    }

    void RoundOver()
    {
        bool playerBust = player.handValue > 21;
        bool dealerBust = dealer.handValue > 21;
        bool player21 = player.handValue == 21;
        bool dealer21 = dealer.handValue == 21;

        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;

        bool roundOver = true;

        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust";

            player.AdjustMoney(pot / 2);
        }
        else if (playerBust || (!dealerBust && dealer.handValue > player.handValue))
        {
            mainText.text = "Dealer Wins!";
        }
        else if (dealerBust || player.handValue > dealer.handValue)
        {
            mainText.text = "You Win!";

            player.AdjustMoney(pot);
        }
        else if (player.handValue == dealer.handValue)
        {
            mainText.text = "Push: Bets Returned";

            player.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }

        if (roundOver)
        {
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerText.gameObject.SetActive(true);

            hideCard.GetComponent<Renderer>().enabled = false;

            cashText.text = player.GetMoney().ToString() + " souls";

            standClicks = 0;
        }
    }
}
