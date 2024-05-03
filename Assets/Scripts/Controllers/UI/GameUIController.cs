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

    private SceneLoadingManager sceneLoadingManager => appManager.sceneLoadingManager;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        matchesText.SetText($"Matches:\n0");
        turnsText.SetText($"Turns:\n0");
        homeButton.onClick.AddListener(OnHomeButtonPressed);
        GenerateCardLayout();
    }

    private void OnDestroy() {
        homeButton.onClick.RemoveListener(OnHomeButtonPressed);
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

        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                // Instantiate card object
                CardController card = Instantiate(cardControllerPrefab, Vector3.zero, Quaternion.identity);
                card.Setup(appManager);
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
}