using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int rank = 0;

    public DeckManager deckManager;

    public int GetRank()
    {
        return rank;
    }

    public void SetRank(int newRank)
    {
        rank = newRank;
    }

    public string GetSpriteName()
    {
        return GetComponent<SpriteRenderer>().sprite.name;
    }

    public void SetSprite(Sprite newSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public void ResetCard()
    {
        Sprite back = deckManager.GetCardBack();
        gameObject.GetComponent<SpriteRenderer>().sprite = back;

        rank = 0;
    }
}
