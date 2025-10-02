using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Sprite[] cardSprites;

    private int[] cardRanks = new int[53];
    private int currentIndex = 0;

    void GetCardRank()
    {
        int num = 0;

        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            num %= 13;

            if (num > 10 || num == 0)
            {
                num = 10;
            }

            cardRanks[i] = num++;
        }

        currentIndex = 1;
    }

    public void Shuffle()
    {
        for (int i = cardSprites.Length - 1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length - 1) + 1;

            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int rank = cardRanks[i];
            cardRanks[i] = cardRanks[j];
            cardRanks[j] = rank;
        }
    }

    public int Deal(Card card)
    {
        card.SetSprite(cardSprites[currentIndex]);
        card.SetRank(cardRanks[currentIndex++]);

        currentIndex++;

        return card.GetRank();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
