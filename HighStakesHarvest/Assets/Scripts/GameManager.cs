using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Button deal;
    public Button hit;
    public Button stand;

    public PlayerScript playerscript;
    public PlayerScript dealerscript;

    public Text CashText;
    public Text HandText;
    public Text BetText;
    public Text DealerText;
    public GameObject hideCard;

    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject DrawScreen;

    private int pot = 0;
    private bool playerHasStood = false;
    private bool roundEnded = false;

    private void Start()
    {
        // Remove all previous listeners
        deal.onClick.RemoveAllListeners();
        hit.onClick.RemoveAllListeners();
        stand.onClick.RemoveAllListeners();

        // Add listeners
        deal.onClick.AddListener(DealClicked);
        hit.onClick.AddListener(HitClicked);
        stand.onClick.AddListener(StandClicked);

        ResetUI();
    }

    // Start a new round
    public void DealClicked()
    {
        ResetUI();

        deal.gameObject.SetActive(false); // hide Deal when round starts
        hit.gameObject.SetActive(true);
        stand.gameObject.SetActive(true);

        roundEnded = false;
        playerHasStood = false;

        DealerText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();

        playerscript.DealInitialHand();
        dealerscript.DealInitialHand();

        if (hideCard != null)
            hideCard.SetActive(true);

        pot = 40;
        playerscript.AdjustMoney(-20);

        UpdateUI();

        CheckBlackjack(); // handle instant 21
    }

    public void HitClicked()
    {
        if (roundEnded) return;

        playerscript.HitOneCard();
        UpdateUI();

        if (playerscript.handValue >= 21)
        {
            playerHasStood = true; // auto-stand if bust or 21
            StartCoroutine(DealerTurn());
        }
    }

    public void StandClicked()
    {
        if (roundEnded) return;

        playerHasStood = true;
        hit.gameObject.SetActive(false);
        stand.gameObject.SetActive(false);

        if (hideCard != null)
            hideCard.SetActive(false);

        StartCoroutine(DealerTurn());
    }

    // Dealer plays automatically
    private IEnumerator DealerTurn()
    {
        DealerText.gameObject.SetActive(true);

        while (dealerscript.handValue < 16 && dealerscript.cardIndex < dealerscript.hand.Length)
        {
            dealerscript.HitOneCard();
            UpdateUI();
            yield return new WaitForSeconds(1.5f); // small delay to see cards
        }

        roundEnded = true;
        DetermineRoundOutcome();
    }

    private void CheckBlackjack()
    {
        // If anyone has 21 at the start
        if (playerscript.handValue == 21 || dealerscript.handValue == 21)
        {
            playerHasStood = true;
            StartCoroutine(DealerTurn());
        }
    }

    private void DetermineRoundOutcome()
    {
        bool playerBust = playerscript.handValue > 21;
        bool dealerBust = dealerscript.handValue > 21;

        if ((playerBust && dealerBust) || (playerscript.handValue == dealerscript.handValue))
        {
            DrawScreen.SetActive(true);
            playerscript.AdjustMoney(pot / 2);
        }
        else if (playerBust || (!dealerBust && dealerscript.handValue > playerscript.handValue))
        {
            LoseScreen.SetActive(true);
        }
        else
        {
            WinScreen.SetActive(true);
            playerscript.AdjustMoney(pot);
        }

        hit.gameObject.SetActive(false);
        stand.gameObject.SetActive(false);
        deal.gameObject.SetActive(true);

        roundEnded = true;
    }

    private void UpdateUI()
    {
        HandText.text = "Hand: " + playerscript.handValue;
        DealerText.text = "Hand: " + dealerscript.handValue;
        CashText.text = "Cash: " + playerscript.GetMoney().ToString();
        BetText.text = "Bet: " + pot.ToString();
    }

    private void ResetUI()
    {
        hit.gameObject.SetActive(false);
        stand.gameObject.SetActive(false);

        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        DrawScreen.SetActive(false);

        if (hideCard != null)
            hideCard.GetComponent<Renderer>().enabled = false;
    }
}
