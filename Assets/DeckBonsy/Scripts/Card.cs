using System.Collections;
using UnityEngine;

public class Card
{
    public int id { get; private set; }
    public string cardName { get; private set; }
    public int effectId { get; private set; }
    public int power { get; private set; }
    public CardType cardType { get; private set; }

    public void SetValues(int _id, string _cardName, int _effectId, int _power, CardType _cardType) /// id, name, effectId, type
    {
        id = _id;
        cardName = _cardName;
        effectId = _effectId;
        power = _power;
        cardType = _cardType;
    }

    public void SetPower(int _power)
    {
        power = _power;
    }
}
