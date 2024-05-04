using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager {
    public int matchScore { get; private set; }
    public int matchTurn { get; private set; }

    [field: SerializeField] public int maxTurns { get; private set; } = 1;
    
    private CardController previousCardController;
    private SceneLoadingManager sceneLoadingManager => appManager.sceneLoadingManager;
    private DataManager dataManager => appManager.dataManager;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        StartListeningToEvent<CardFlippedEvent>(OnCardFlippedEvent);
    }

    private void OnDestroy() {
        StopListeningToEvent<CardFlippedEvent>(OnCardFlippedEvent);
        dataManager.SaveMatchScore(matchScore);
    }

    private void OnCardFlippedEvent(object sender, EventArgs e) {
        CardFlippedEvent cardFlippedEvent = e as CardFlippedEvent;
        CardController flippedCardController = cardFlippedEvent.cardController;
        if (previousCardController == null) previousCardController = flippedCardController;
        else {
            if (previousCardController.cardFront.sprite == cardFlippedEvent.cardController.cardFront.sprite) {
                matchScore++;
                TriggerEvent<CardMatchedEvent>(new CardMatchedEvent(cardFlippedEvent.cardController));
                GlobalAudioPlayer.PlaySFX(Constants.sfxMatched);
            } else {
                GlobalAudioPlayer.PlaySFX(Constants.sfxMismatched);
                previousCardController.StartFlip(true);
                flippedCardController.StartFlip(true);
            }

            matchTurn++;
            previousCardController = null;
            if (matchTurn > maxTurns) {
                GlobalAudioPlayer.PlaySFX(Constants.sfxGameOver);
                sceneLoadingManager.LoadSceneSingle(Constants.mainMenuScene);
            } else {
                TriggerEvent<CardMatchTurnEndedEvent>(new CardMatchTurnEndedEvent());
            }
        }
    }
}