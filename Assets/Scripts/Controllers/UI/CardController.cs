using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : BaseBehaviour {
    [field: SerializeField] public Image cardFront { get; private set; }

    [SerializeField] private bool cardBackIsActive;
    [SerializeField] private float flipDuration;
    [SerializeField] private Button cardButton;
    [SerializeField] private GameObject cardBack;

    private bool isFlipping;

    public void Setup(AppManager appManager, CardDataSO.CardData cardData) {
        base.Setup(appManager);
        cardFront.sprite = cardData.frontSprite;
        cardBackIsActive = true;
        cardButton.onClick.AddListener(OnCardButtonPressed);
        StartListeningToEvent<CardMatchedEvent>(OnCardMatchedEvent);
    }

    private void OnDestroy() {
        cardButton.onClick.RemoveListener(OnCardButtonPressed);
        StopListeningToEvent<CardMatchedEvent>(OnCardMatchedEvent);
    }

    public void StartFlip(bool isReset = false) {
        if (!isFlipping) {
            isFlipping = true;
            StartCoroutine(FlipCard(isReset));
        }
    }

    private IEnumerator FlipCard(bool isReset) {
        float elapsedTime = 0f;
        float halfDuration = flipDuration / 2f;
        Quaternion startRotation = cardBackIsActive ? cardBack.transform.rotation : cardFront.transform.rotation;
        Quaternion targetRotation = cardBackIsActive && cardBack.transform.rotation.y == 0 || cardFront.gameObject.activeSelf && cardFront.transform.rotation.y == 0 ? Quaternion.Euler(0f, 90f, 0f) : Quaternion.Euler(0f, 0f, 0f); // Rotate to 90 or 0 degrees

        while (elapsedTime < halfDuration) {
            yield return null;
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / halfDuration);
            if (cardBackIsActive) cardBack.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            else cardFront.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
        }

        Flip();
        
        // Reset the elapsed time for the second half of the flip
        elapsedTime = 0f;
        startRotation = cardBackIsActive ? cardBack.transform.rotation : cardFront.transform.rotation;
        targetRotation = cardBackIsActive && cardBack.transform.rotation.y == 0 || cardFront.gameObject.activeSelf && cardFront.transform.rotation.y == 0 ? Quaternion.Euler(0f, 90f, 0f) : Quaternion.Euler(0f, 0f, 0f);

        while (elapsedTime < halfDuration) {
            yield return null;
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / halfDuration);
            if (cardBackIsActive) cardBack.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            else cardFront.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
        }

        isFlipping = false;
        if (!isReset) TriggerEvent<CardFlippedEvent>(new CardFlippedEvent(this));
    }

    private void Flip() {
        GlobalAudioPlayer.PlaySFX(Constants.sfxFlip);
        cardBackIsActive = !cardBackIsActive;
        cardBack.SetActive(cardBackIsActive);
        cardFront.gameObject.SetActive(!cardBackIsActive);
    }

    private void OnCardButtonPressed() {
        if (!cardFront.gameObject.activeSelf) StartFlip();
    }

    private void OnCardMatchedEvent(object sender, EventArgs e) {
        CardMatchedEvent cardMatchedEvent = e as CardMatchedEvent;
        if (cardMatchedEvent.cardController == this) {
            // This card matched.
            // Scale in and out and pop.
            cardButton.interactable = false;
        }
    }
}