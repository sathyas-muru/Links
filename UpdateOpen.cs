using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateOpen : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private SelectableOpen selectable;
    private Links links;

    // Start is called before the first frame update
    void Start()
    {
        links = FindObjectOfType<Links>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<SelectableOpen>();

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
}
