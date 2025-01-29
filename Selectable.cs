using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selectable : MonoBehaviour
{
    public bool faceUp = false;
    private Links links;

    private bool isTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        links = FindObjectOfType<Links>();
    }

    void Update()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (!isTouching)  // Ensure single touch detection
            {
                isTouching = true;

                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

                if (hit)
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        if (IsTopCard())
                        {
                            links.PlayCard(this.gameObject);
                        }
                    }
                }
            }
        }
        else
        {
            isTouching = false; // Reset touch detection
        }
    }

    bool IsTopCard()
    {
        foreach (var playerCards in links.PlayerCards)
        {
            if (playerCards.Contains(gameObject.name) && playerCards[playerCards.Count - 1] == gameObject.name)
            {
                return true;
            }
        }
        return false;
    }
}
