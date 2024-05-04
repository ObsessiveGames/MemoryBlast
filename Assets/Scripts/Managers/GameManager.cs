using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager {
    public int matchScore { get; private set; }
    public int matchTurn { get; private set; }
    
    private CardController previousCardController;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        StartListeningToEvent<CardFlippedEvent>(OnCardFlippedEvent);
    }

    private void OnDestroy() {
        StartListeningToEvent<CardFlippedEvent>(OnCardFlippedEvent);
        // TODO:
        // Save data to dataManager.
    }

    private void OnCardFlippedEvent(object sender, EventArgs e) {
        CardFlippedEvent cardFlippedEvent = e as CardFlippedEvent;
        CardController flippedCardController = cardFlippedEvent.cardController;
        if (previousCardController == null) previousCardController = flippedCardController;
        else {
            if (previousCardController.cardFront.sprite == cardFlippedEvent.cardController.cardFront.sprite) {
                matchScore++;
                TriggerEvent<CardMatchedEvent>(new CardMatchedEvent(cardFlippedEvent.cardController));
            } else {
                previousCardController.StartFlip(true);
                flippedCardController.StartFlip(true);
            }

            matchTurn++;
            TriggerEvent<CardMatchTurnEndedEvent>(new CardMatchTurnEndedEvent());
            previousCardController = null;
        }
    }
}