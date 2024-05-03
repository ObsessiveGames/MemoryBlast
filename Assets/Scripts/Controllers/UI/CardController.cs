using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : BaseBehaviour {
    [SerializeField] private Button cardButton;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        cardButton.onClick.AddListener(OnCardButtonPressed);
    }

    private void OnDestroy() {
        cardButton.onClick.RemoveListener(OnCardButtonPressed);
    }

    private void OnCardButtonPressed() {
        Debug.Log("Boop");
    }
}