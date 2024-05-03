using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : BaseBehaviour {
    [SerializeField] private bool cardBackIsActive;
    [SerializeField] private float flipDuration;
    [SerializeField] private Button cardButton;
    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;

    private bool isFlipping;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        cardBackIsActive = true;
        cardButton.onClick.AddListener(OnCardButtonPressed);
    }

    private void OnDestroy() {
        cardButton.onClick.RemoveListener(OnCardButtonPressed);
    }

    private void StartFlip() {
        if (!isFlipping) {
            isFlipping = true;
            StartCoroutine(FlipCard());
        }
    }

    private IEnumerator FlipCard() {
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
    }

    private void Flip() {
        cardBackIsActive = !cardBackIsActive;
        cardBack.SetActive(cardBackIsActive);
        cardFront.SetActive(!cardBackIsActive);
    }

    private void OnCardButtonPressed() {
        StartFlip();
    }
}