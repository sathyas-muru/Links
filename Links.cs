using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Links : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject OpenCardPrefab;
    public GameObject[] cardPos;
    public GameObject[] OpenCardPos;
    public int penalty;
    // public GameObject PlayerScore;

    public static string[] colors = new string[] { "b", "g", "p" };
    public static string[] shapes = new string[] { "c", "d", "h", "t" };
    public static string[] value = new string[] { "1", "2", "3", "4", "5", "6" };

    public List<string> PlayerDeck;
    public List<string> OpenDeck;
    public List<string>[] PlayerCards;

    private List<string> card0 = new List<string>();
    private List<string> card1 = new List<string>();
    private List<string> card2 = new List<string>();
    private List<string> card3 = new List<string>();
    private List<string> card4 = new List<string>();

    private GameObject openCardsParent;
    private Coroutine updateOpenCardCoroutine;
    public float elapsedTime = 0f;
    private bool gameRunning = false;
    public float OpenCardDelay = 1.0f;

    SoundEffects soundEffects;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText;

    LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        penalty = 0;
        levelManager = FindObjectOfType<LevelManager>();
        soundEffects = FindObjectOfType<SoundEffects>();
        PlayerCards = new List<string>[] { card0, card1, card2, card3, card4 };
        openCardsParent = GameObject.Find("OpenCards");


        PlayCards();

    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            elapsedTime += Time.deltaTime;

            // Display time
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void StartGameTimer()
    {
        gameRunning = true;
        elapsedTime = 0f;
    }

    public void PlayCards()
    {
        // Create Player deck
        PlayerDeck = GenerateDeck();
        Shuffle(PlayerDeck);
        Shuffle(PlayerDeck);

        // Create Open Card deck
        OpenDeck = GenerateDeck();
        Shuffle(OpenDeck);
        Shuffle(OpenDeck);

        Sort();
        StartCoroutine(StartGame());
        // StartCoroutine(Deal());
        // OpenCardDeal();
        // StartGameTimer();
        // StartCoroutine(StartGameWithDelay(2f));



    }

    public static List<string> GenerateDeck()
    {
        List<string> newPlayerDeck = new List<string>();
        foreach (string c in colors)
        {
            foreach (string s in shapes)
            {
                foreach (string v in value)
                {
                    newPlayerDeck.Add(s + c + v);
                }
            }
        }
        return newPlayerDeck;
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    IEnumerator StartGame()
    {
        soundEffects.StartDealingSound();
        yield return StartCoroutine(Deal());
        OpenCardDeal();
        soundEffects.StopDealingSound();
        StartGameTimer();

    }

    IEnumerator Deal()
    {
        for (int i = 0; i < 5; i++)
        {


            float yOffset = 0;
            float zOffset = 0.01f;

            foreach (string card in PlayerCards[i])
            {
                yield return new WaitForSeconds(0f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(cardPos[i].transform.position.x, cardPos[i].transform.position.y - yOffset, cardPos[i].transform.position.z - zOffset), Quaternion.identity, cardPos[i].transform);
                newCard.name = card;

                if (card == PlayerCards[i][PlayerCards[i].Count - 1])
                {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }

                yOffset = yOffset + 0.025f;
                zOffset = zOffset + 0.03f;
            }
        }
    }



    void OpenCardDeal()
    {
        if (OpenDeck.Count > 0)
        {
            // Position for the new face-down card
            Vector3 dealPosition = new Vector3(-1.02f, 1.68f, 0f);

            // Create the new open card
            string newOpenCardName = OpenDeck[OpenDeck.Count - 1];
            OpenDeck.RemoveAt(OpenDeck.Count - 1);

            GameObject newCard = Instantiate(OpenCardPrefab, dealPosition, Quaternion.identity);
            newCard.name = newOpenCardName;
            newCard.GetComponent<SelectableOpen>().faceUp = false;
            newCard.transform.SetParent(openCardsParent.transform);

            SpriteRenderer newCardRenderer = newCard.GetComponent<SpriteRenderer>();
            newCardRenderer.sortingOrder = openCardsParent.transform.childCount;

            // Move the new card to the face-up position
            NewOpen();

            if (updateOpenCardCoroutine != null)
            {
                StopCoroutine(updateOpenCardCoroutine);
            }
            updateOpenCardCoroutine = StartCoroutine(UpdateOpenCard(OpenCardDelay));
        }
    }


    void NewOpen()
    {
        // Position for the face-up top card
        Vector3 faceUpPosition = new Vector3(1.08f, 1.68f, 0f);

        GameObject openCardsParent = GameObject.Find("OpenCards");

        if (openCardsParent.transform.childCount > 0)
        {
            // Get the top card (the last child of the openCardsParent)
            Transform topCardTransform = openCardsParent.transform.GetChild(openCardsParent.transform.childCount - 1);
            topCardTransform.position = faceUpPosition;
            topCardTransform.GetComponent<SelectableOpen>().faceUp = true;

            SpriteRenderer topCardRenderer = topCardTransform.GetComponent<SpriteRenderer>();
            topCardRenderer.sortingOrder = openCardsParent.transform.childCount;
        }
    }

    IEnumerator StartGameWithDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Start the game timer
        StartGameTimer();
    }


    IEnumerator UpdateOpenCard(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            OpenCardDeal();
        }
    }

    void Sort()
    {
        int currentPlayer = 0;
        int totalCards = PlayerDeck.Count;

        for (int i = 0; i < totalCards; i++)
        {
            PlayerCards[currentPlayer].Add(PlayerDeck.Last<string>());
            PlayerDeck.RemoveAt(PlayerDeck.Count - 1);
            currentPlayer = (currentPlayer + 1) % 5;
        }

    }


    public void PlayCard(GameObject selectedCard)
    {


        string selectedCardName = selectedCard.name;
        Transform openCardsTransform = openCardsParent.transform;

        // Check if the selected card can be played
        string topOpenCardName = openCardsTransform.GetChild(openCardsTransform.childCount - 1).name;
        Debug.Log($"Selected Card: {selectedCardName}, Top Open Card: {topOpenCardName}");

        if (ValidPlay(selectedCardName, topOpenCardName))
        {
            // Update the position and state of the selected card
            selectedCard.transform.position = new Vector3(1.08f, 1.68f, 0f);
            selectedCard.transform.SetParent(openCardsTransform);
            selectedCard.GetComponent<Selectable>().faceUp = true;


            SpriteRenderer spriteRenderer = selectedCard.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;

            // Remove the card from the player's hand
            foreach (var playerCards in PlayerCards)
            {
                if (playerCards.Remove(selectedCard.name))
                {
                    Debug.Log("Removed card from player's hand: " + selectedCard.name);
                    break;
                }
            }

            if (openCardsTransform.childCount > 1)
            {
                GameObject topCard = openCardsTransform.GetChild(openCardsTransform.childCount - 2).gameObject;
                OpenDeck.Insert(0, topCard.name);
            }

            // Flip over the next card in the player hand pile
            /*foreach (var playerCards in PlayerCards)
            {
                if (playerCards.Count > 0)
                {
                    // Get the next card to be revealed
                    string nextCardName = playerCards.Last();
                    GameObject nextCard = GameObject.Find(nextCardName);
                    nextCard.GetComponent<Selectable>().faceUp = true;

                }
            }*/
            foreach (var playerCards in PlayerCards)
            {
                if (playerCards.Count > 0)
                {
                    // Get the next card to be revealed
                    string nextCardName = playerCards.Last();
                    try
                    {
                        GameObject nextCard = GameObject.Find(nextCardName);

                        if (nextCard == null)
                        {
                            Debug.LogError("GameObject not found for card: " + nextCardName);
                            continue; // Skip to the next card in the loop
                        }

                        Selectable selectableComponent = nextCard.GetComponent<Selectable>();
                        if (selectableComponent == null)
                        {
                            Debug.LogError("Selectable component missing on card: " + nextCardName);
                            continue; // Skip to the next card in the loop
                        }

                        selectableComponent.faceUp = true;
                        // Debug.Log("Flipped next card: " + nextCardName);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError("Exception occurred while flipping card: " + nextCardName + " - " + ex.Message);
                    }
                }
            }



            if (openCardsTransform.childCount > 0)
            {
                GameObject topCard = openCardsTransform.GetChild(openCardsTransform.childCount - 1).gameObject;
                if (topCard != null)
                {
                    Destroy(topCard);
                }
            }

            CheckGameOver();

            // GameManager.Instance.CheckGameOver();

            // Deal the new open card
            // OpenCardDeal();

            // Start the process to update the open card
            // StartCoroutine(ReplaceOpenCard());
        }
        else
        {
            soundEffects.PlayWrongCard();
            penalty += 1;
            print($"Penalty: {penalty}");

            // Debug.Log($"Invalid play: {selectedCardName} cannot be played on {topOpenCardName}");
        }

    }

    IEnumerator ReplaceOpenCard()
    {
        // Wait for 2 seconds before replacing the open card
        yield return new WaitForSeconds(OpenCardDelay);

        // Remove the current top open card
        if (openCardsParent.transform.childCount > 0)
        {
            GameObject topCard = openCardsParent.transform.GetChild(openCardsParent.transform.childCount - 1).gameObject;
            Destroy(topCard);
        }

        // Deal the new open card
        OpenCardDeal();
        FlipNextCard();
    }

    void CheckGameOver()
    {
        bool allHandsEmpty = PlayerCards.All(cards => cards.Count == 0);

        if (allHandsEmpty)
        {
            Debug.Log("Game Over! All hands are empty.");

            // Stop card dealing
            StopCoroutine(updateOpenCardCoroutine);

            // Stop game timer
            gameRunning = false;

            // Display time
            Debug.Log("Time Taken: " + elapsedTime.ToString("F2") + " seconds");

            PlayerScore.Instance.SetFinalScore(GetScore());
            PlayerScore.Instance.SetPenalty(penalty);

            levelManager.LoadGameOver();
        }
    }

    public string GetScore()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    void FlipNextCard()
    {
        // Iterate over all player's card lists to find the next card
        foreach (var playerCards in PlayerCards)
        {
            if (playerCards.Count > 0)
            {
                // The next card to flip should be the new top card of the player’s stack
                string nextCardName = playerCards.Last();
                GameObject nextCard = GameObject.Find(nextCardName);

                nextCard.GetComponent<Selectable>().faceUp = true;

                if (nextCard != null)
                {
                    nextCard.GetComponent<Selectable>().faceUp = true;
                }
                else
                {
                    Debug.LogError("Next card not found: " + nextCardName);
                }
                break; // Assuming you only want to flip one card
            }
        }
    }


    bool ValidPlay(string selectedCard, string topOpenCard)
    {
        return selectedCard[1] == topOpenCard[1] || selectedCard[0] == topOpenCard[0] || selectedCard[2] == topOpenCard[2];
    }
}