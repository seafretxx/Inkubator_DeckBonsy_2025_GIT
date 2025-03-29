using System.Collections;
using UnityEngine;

public class Card
{
    public int id { get; private set; }
    public string cardName { get; private set; }
    public int effectId { get; private set; }
    public int power { get; private set; }
    public CardType cardType { get; private set; }
    public int points { get; private set; }

    public Card(int _id, string _cardName, int _effectId, int _power, CardType _cardType, int _points) /// id, name, effectId, type, points
    {
        id = _id;
        cardName = _cardName;
        effectId = _effectId;
        power = _power;
        cardType = _cardType;
        points = _points;
    }

    public int GetPoints()
    {
        return points;
    }


}
