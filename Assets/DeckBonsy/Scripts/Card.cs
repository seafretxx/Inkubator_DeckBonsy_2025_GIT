using System.Collections;
using UnityEngine;

public class Card
{
    public int id { get; private set; }
    public string cardName { get; private set; }
    public int effectId { get; private set; }
    public int points { get; private set; }
    public CardType cardType { get; private set; }

    public void SetValues(int _id, string _cardName, int _effectId, int _points, CardType _cardType)
    {
        id = _id;
        cardName = _cardName;
        effectId = _effectId;
        points = _points;
        cardType = _cardType;
    }

    public void SetValues((int _id, string _cardName, int _effectId, int _points, CardType _cardType) _cardValue)
    {
        id = _cardValue._id;
        cardName = _cardValue._cardName;
        effectId = _cardValue._effectId;
        points = _cardValue._points;
        cardType = _cardValue._cardType;
    }

    public void SetPoints(int _points)
    {
        points = _points;
    }
}
