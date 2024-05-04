using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "GameData/New Card Datas")]
public class CardDataSO : ScriptableObject {
    [field: SerializeField] public List<CardData> cardDataList { get; private set; }

    [Serializable]
    public class CardData {
        public Sprite frontSprite;
    }
}