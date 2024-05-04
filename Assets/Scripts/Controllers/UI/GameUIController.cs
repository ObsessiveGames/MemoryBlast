using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : BaseBehaviour {
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float spacing;
    [SerializeField] private CardController cardControllerPrefab;
    [SerializeField] private TextMeshProUGUI matchesText;
    [SerializeField] private TextMeshProUGUI turnsText;
    [SerializeField] private Button homeButton;
    [SerializeField] private RectTransform containerRect;

    private CardDataSO availableCards;
    private SceneLoadingManager sceneLoadingManager => appManager.sceneLoadingManager;
    private DataManager dataManager => appManager.dataManager;
    private GameManager gameManager => appManager.gameManager;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        availableCards = dataManager.cards;
        matchesText.SetText($"Matches:\n{gameManager.matchScore}");
        turnsText.SetText($"Turns:\n{gameManager.matchTurn}/{gameManager.maxTurns}");
        homeButton.onClick.AddListener(OnHomeButtonPressed);
        if (dataManager.isPreviousGame) LoadPreviousCardLayout();
        else GenerateCardLayout();
        StartListeningToEvent<CardMatchedEvent>(OnCardMatchedEvent);
        StartListeningToEvent<CardMatchTurnEndedEvent>(OnCardMatchTurnEndedEvent);
    }

    private void OnDestroy() {
        homeButton.onClick.RemoveListener(OnHomeButtonPressed);
        StopListeningToEvent<CardMatchedEvent>(OnCardMatchedEvent);
        StopListeningToEvent<CardMatchTurnEndedEvent>(OnCardMatchTurnEndedEvent);
    }

    private void OnHomeButtonPressed() {
        sceneLoadingManager.LoadSceneSingle(Constants.mainMenuScene);
    }

    private void GenerateCardLayout() {
        // Calculate total width and height of the card layout
        float totalWidth = containerRect.rect.width;
        float totalHeight = containerRect.rect.height;

        // Calculate available width and height for each card after considering spacing
        float availableWidth = totalWidth - (columns - 1) * spacing;
        float availableHeight = totalHeight - (rows - 1) * spacing;

        // Calculate size of each card based on the available space and the number of rows and columns
        float cardWidth = availableWidth / columns;
        float cardHeight = availableHeight / rows;

        int totalPositions = rows * columns;
        int maxIndex = totalPositions / 2;

        List<int> availableCardIndices = new List<int>();
        List<int> duplicatedAvailableCardIndices = new List<int>();

        for (int i = 0; i < maxIndex; i++) {
            availableCardIndices.Add(i);
            duplicatedAvailableCardIndices.Add(i);
        }

        dataManager.ClearSaveAddIndex();

        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                int randomIndex = 0;
                if (duplicatedAvailableCardIndices.Count >= availableCardIndices.Count) {
                    int duplicateIndex = UnityEngine.Random.Range(0, duplicatedAvailableCardIndices.Count);
                    randomIndex = duplicatedAvailableCardIndices[duplicateIndex];
                    duplicatedAvailableCardIndices.RemoveAt(duplicateIndex);
                } else {
                    int availableIndex = UnityEngine.Random.Range(0, availableCardIndices.Count);
                    randomIndex = availableCardIndices[availableIndex];
                    availableCardIndices.RemoveAt(availableIndex);
                }

                CardDataSO.CardData selectedCard = availableCards.cardDataList[randomIndex];
                dataManager.GameSaveAddIndex(randomIndex);
                CardController card = Instantiate(cardControllerPrefab, Vector3.zero, Quaternion.identity);
                card.Setup(appManager, selectedCard);
                card.transform.SetParent(containerRect.transform, false); // Set as child of container
                RectTransform cardRect = card.GetComponent<RectTransform>();

                // Set size and position of the card
                cardRect.sizeDelta = new Vector2(cardWidth, cardHeight);
                float posX = col * (cardWidth + spacing);
                float posY = -row * (cardHeight + spacing); // Negative y to account for Unity's inverted UI coordinate system
                cardRect.anchoredPosition = new Vector2(posX, posY);
            }
        }
    }

    private void LoadPreviousCardLayout() {
        // Calculate total width and height of the card layout
        float totalWidth = containerRect.rect.width;
        float totalHeight = containerRect.rect.height;

        // Calculate available width and height for each card after considering spacing
        float availableWidth = totalWidth - (columns - 1) * spacing;
        float availableHeight = totalHeight - (rows - 1) * spacing;

        // Calculate size of each card based on the available space and the number of rows and columns
        float cardWidth = availableWidth / columns;
        float cardHeight = availableHeight / rows;

        int totalPositions = rows * columns;
        int maxIndex = totalPositions / 2;

        List<int> availableCardIndices = new List<int>();
        List<int> duplicatedAvailableCardIndices = new List<int>();

        for (int i = 0; i < maxIndex; i++) {
            availableCardIndices.Add(i);
            duplicatedAvailableCardIndices.Add(i);
        }

        int indexCounter = 0;
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                List<int> gameSaveIndex = dataManager.data.gameSaveIndex;
                int cardIndex = gameSaveIndex[indexCounter];
                CardDataSO.CardData selectedCard = availableCards.cardDataList[cardIndex];
                indexCounter++;
                CardController card = Instantiate(cardControllerPrefab, Vector3.zero, Quaternion.identity);
                if (dataManager.data.matchedIndex.Contains(cardIndex)) {
                    card.SetupClaimed(selectedCard);
                } else {
                    card.Setup(appManager, selectedCard);
                }
                
                card.transform.SetParent(containerRect.transform, false); // Set as child of container
                RectTransform cardRect = card.GetComponent<RectTransform>();

                // Set size and position of the card
                cardRect.sizeDelta = new Vector2(cardWidth, cardHeight);
                float posX = col * (cardWidth + spacing);
                float posY = -row * (cardHeight + spacing); // Negative y to account for Unity's inverted UI coordinate system
                cardRect.anchoredPosition = new Vector2(posX, posY);
            }
        }
    }

    private void OnCardMatchedEvent(object sender, EventArgs e) {
        matchesText.SetText($"Matches:\n{gameManager.matchScore}");
    }

    private void OnCardMatchTurnEndedEvent(object sender, EventArgs e) {
        turnsText.SetText($"Turns:\n{gameManager.matchTurn}/{gameManager.maxTurns}");
    }
}