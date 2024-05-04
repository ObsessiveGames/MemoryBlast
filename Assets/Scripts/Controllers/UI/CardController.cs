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
        if (cardMatchedEvent.cardController.cardFront.sprite == cardFront.sprite) {
            StartCoroutine(PlayMatchAnimation());
            // This card matched.
            // Scale in and out and pop.
            cardButton.interactable = false;
        }
    }

    private IEnumerator PlayMatchAnimation() {
        // Define the initial scale and the target scale
        Vector3 initialScale = cardFront.transform.localScale;
        Vector3 targetScale = initialScale + new Vector3(0.5f, 0.5f, 0.5f);

        // Scale in
        float duration = 0.25f;
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            cardFront.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for a brief moment at the scaled size
        yield return new WaitForSeconds(0.1f);

        // Scale out
        duration = 0.25f;
        elapsedTime = 0f;
        while (elapsedTime < duration) {
            cardFront.transform.localScale = Vector3.Lerp(targetScale, initialScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the scale is exactly the initial scale at the end
        cardFront.transform.localScale = initialScale;
    }
}