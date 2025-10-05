using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;
    public Button decreaseBetBtn;

    [Header("Text")]
    public TMP_Text mainText;
    public TMP_Text standBtnText;
    public TMP_Text scoreText;
    public TMP_Text dealerText;
    public TMP_Text betsText;
    public TMP_Text cashText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dealSFX;
    public AudioClip hitSFX;
    public AudioClip standSFX;
    public AudioClip increaseBetSFX;
    public AudioClip decreaseBetSFX;
    public AudioClip winSFX;
    public AudioClip loseSFX;

    private int standClicks = 0;
    private int pot = 0;
    private int currentBet = 2;
    private bool roundActive = false;

    [Header("Players")]
    public PlayerController player;
    public PlayerController dealer;

    [Header("Deck")]
    public DeckManager deck;

    [Header("Dealer Cards")]
    public GameObject hideCard;

    private readonly string[] winLines = {
        "You’ve cheated death… temporarily.",
        "Even I must fold sometimes.",
        "You’ve earned a brief extension on existence.",
        "Well played, soul survivor.",
        "You really took my breath away—shame I have none."
    };

    private readonly string[] loseLines = {
        "Looks like your luck flatlined.",
        "Another soul in the discard pile.",
        "You bust, I collect—such a lively arrangement.",
        "Death always deals the final hand.",
        "You’re all out of time… and chips."
    };

    private readonly string[] bustLines = {
        "Too greedy, mortal. Now you’re toast.",
        "You went over—guess you’re under now.",
        "Your ambition has expired.",
        "That hand’s as dead as you’ll soon be.",
        "Over twenty-one? Under six feet."
    };

    private readonly string[] pushLines = {
        "A draw? Even the Reaper needs a rest.",
        "We’re even. How boring.",
        "No souls change hands—what a deadlock.",
        "A tie? How lifeless.",
        "Death yawns: neither of us wins."
    };

    private readonly string notEnoughMoney = "You can’t bet what you don’t have, mortal.";
    private readonly string notEnoughToRaise = "You reach for souls you no longer own.";
    private readonly string midRoundBet = "Changing bets mid-death? Unheard of.";
    private readonly string placeBetPrompt = "Place your wager, little mortal.";
    private readonly string winGameLine = "A hundred souls? Consider yourself undeadly lucky.";
    private readonly string loseGameLine = "No souls left. Welcome to my domain.";

    void Start()
    {
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        mainText.text = placeBetPrompt;
        UpdateUI();
    }

    public void OnDeal()
    {
        if (roundActive)
            return;

        if (player.GetMoney() < currentBet)
        {
            mainText.text = notEnoughMoney;
            mainText.gameObject.SetActive(true);
            return;
        }

        audioSource.PlayOneShot(dealSFX);

        player.ResetHand();
        dealer.ResetHand();

        mainText.gameObject.SetActive(false);
        dealerText.gameObject.SetActive(false);

        deck.Shuffle();
        player.StartHand();
        dealer.StartHand();

        hideCard.GetComponent<Renderer>().enabled = true;

        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);

        standBtnText.text = "Stand";
        standClicks = 0;

        pot = currentBet * 2;
        player.AdjustMoney(-currentBet);
        roundActive = true;

        betBtn.interactable = false;
        decreaseBetBtn.interactable = false;

        UpdateUI();
    }

    public void OnHit()
    {
        if (!roundActive || player.cardIndex >= player.hand.Length)
            return;

        audioSource.PlayOneShot(hitSFX);

        player.GetCard();
        scoreText.text = "Hand: " + player.handValue.ToString();

        if (player.handValue >= 21)
            RoundOver();
    }

    public void OnStand()
    {
        if (!roundActive)
            return;

        audioSource.PlayOneShot(standSFX);

        standClicks++;

        if (standClicks == 1)
        {
            HitDealer();
            standBtnText.text = "Call";
        }
        else
        {
            RoundOver();
        }
    }

    public void OnBet()
    {
        if (roundActive)
        {
            mainText.text = midRoundBet;
            mainText.gameObject.SetActive(true);
            return;
        }

        int newBet = currentBet + 2;

        if (newBet <= player.GetMoney())
        {
            audioSource.PlayOneShot(increaseBetSFX);
            currentBet = newBet;
            UpdateUI();
        }
        else
        {
            mainText.text = notEnoughToRaise;
            mainText.gameObject.SetActive(true);
        }
    }

    public void OnDecreaseBet()
    {
        if (roundActive)
        {
            mainText.text = midRoundBet;
            mainText.gameObject.SetActive(true);
            return;
        }

        int newBet = Mathf.Max(2, currentBet - 2);

        if (newBet != currentBet)
        {
            audioSource.PlayOneShot(decreaseBetSFX);
            currentBet = newBet;
            UpdateUI();
        }
    }

    private void HitDealer()
    {
        while (dealer.handValue < 17 && dealer.cardIndex < dealer.hand.Length)
        {
            dealer.GetCard();
            dealerText.text = "Hand: " + dealer.handValue.ToString();
        }

        RoundOver();
    }

    void RoundOver()
    {
        bool playerBust = player.handValue > 21;
        bool dealerBust = dealer.handValue > 21;
        string result = "";

        if (playerBust && dealerBust)
        {
            result = pushLines[UnityEngine.Random.Range(0, pushLines.Length)];
            player.AdjustMoney(pot / 2);
        }
        else if (playerBust)
        {
            result = bustLines[UnityEngine.Random.Range(0, bustLines.Length)];
            audioSource.PlayOneShot(loseSFX);
        }
        else if (dealerBust || player.handValue > dealer.handValue)
        {
            result = winLines[UnityEngine.Random.Range(0, winLines.Length)];
            player.AdjustMoney(pot);
            audioSource.PlayOneShot(winSFX);
        }
        else if (player.handValue == dealer.handValue)
        {
            result = pushLines[UnityEngine.Random.Range(0, pushLines.Length)];
        }
        else
        {
            result = loseLines[UnityEngine.Random.Range(0, loseLines.Length)];
            audioSource.PlayOneShot(loseSFX);
        }

        mainText.text = result;
        mainText.gameObject.SetActive(true);
        dealerText.text = "Hand: " + dealer.handValue.ToString();
        dealerText.gameObject.SetActive(true);

        hideCard.GetComponent<Renderer>().enabled = false;

        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        dealBtn.gameObject.SetActive(true);

        betBtn.interactable = true;
        decreaseBetBtn.interactable = true;

        roundActive = false;

        UpdateUI();

        if (player.GetMoney() <= 0)
        {
            mainText.text = "No souls left. Welcome to my domain.";
            StartCoroutine(GameOver());
        }
        else if (player.GetMoney() >= 100)
        {
            mainText.text = "A hundred souls? Consider yourself undeadly lucky.";
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UpdateUI()
    {
        scoreText.text = "Hand: " + player.handValue.ToString();
        cashText.text = player.GetMoney() + " souls";
        betsText.text = "Bet: " + currentBet + " souls | Pot: " + pot;
    }
}