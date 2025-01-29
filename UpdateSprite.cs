using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private Links links;

    // Start is called before the first frame update
    void Start()
    {
        links = FindObjectOfType<Links>();
        if (links == null)
        {
            Debug.LogError("Links component not found.");
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found.");
            return;
        }

        selectable = GetComponent<Selectable>();
        if (selectable == null)
        {
            Debug.LogError("Selectable component not found.");
            return;
        }

        // Skip setting cardFace if this is the prefab named "Card"
        if (this.gameObject.name != "card")
        {
            cardFace = GetCardFaceByName(this.gameObject.name);
            if (cardFace == null)
            {
                Debug.LogError($"Card face not found for card name: {this.gameObject.name}");
                cardFace = cardBack; // Assign card back as a fallback
            }
        }
        else
        {
            Debug.Log("Skipping prefab named 'card'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer != null && selectable != null)
        {
            spriteRenderer.sprite = selectable.faceUp ? cardFace : cardBack;
        }
        else
        {
            Debug.LogError("SpriteRenderer or Selectable is null in Update.");
        }
    }

    Sprite GetCardFaceByName(string cardName)
    {
        if (links == null || links.cardFaces == null)
        {
            Debug.LogError("Links or cardFaces is null.");
            return cardBack;
        }

        // Example of how you might normalize names if needed
        string normalizedCardName = cardName.Replace("_", "").ToLower();

        foreach (Sprite face in links.cardFaces)
        {
            string normalizedFaceName = face.name.Replace("_", "").ToLower();
            if (normalizedFaceName == normalizedCardName)
            {
                return face;
            }
        }

        Debug.LogError($"Card face not found for card name: {cardName}");
        return cardBack; // Return a default sprite or handle the error as needed
    }
}


/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private Links links;

    // Start is called before the first frame update
    void Start()
    {
        links = FindObjectOfType<Links>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();

        // Find the correct face sprite based on the card name
        cardFace = GetCardFaceByName(this.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = selectable.faceUp ? cardFace : cardBack;
    }

    Sprite GetCardFaceByName(string cardName)
    {
        // Debug log to ensure cardName is correct
        // Debug.Log($"Looking for card face: {cardName}");

        // Example of how you might normalize names if needed
        string normalizedCardName = cardName.Replace("_", "").ToLower();

        foreach (Sprite face in links.cardFaces)
        {
            string normalizedFaceName = face.name.Replace("_", "").ToLower();
            if (normalizedFaceName == normalizedCardName)
            {
                // Debug.Log($"Found card face: {face.name}");
                return face;
            }
        }

        // Debug.LogError($"Card face not found for card name: {cardName}");
        return cardBack; // Return a default sprite or handle the error as needed
    }
}*/
