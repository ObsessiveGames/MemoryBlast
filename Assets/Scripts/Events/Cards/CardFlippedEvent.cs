using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlippedEvent : BaseEvent {
    public CardController cardController {  get; private set; }

    public CardFlippedEvent(CardController cardController) {
        this.cardController = cardController;
    }
}