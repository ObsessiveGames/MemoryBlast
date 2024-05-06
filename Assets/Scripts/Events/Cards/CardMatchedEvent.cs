using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchedEvent : BaseEvent {
    public CardController cardController { get; private set; }

    public CardMatchedEvent(CardController cardController) {
        this.cardController = cardController;
    }
}