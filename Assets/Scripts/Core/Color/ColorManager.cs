using UnityEngine;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour {

    public static ColorManager Instance { get; private set; }

    [System.Serializable]
    public struct ColorData {

        public CardColor colorType;
        public Color color;
    }

    [SerializeField] private ColorData[] colorMappings;

    private Dictionary<CardColor, Color> colorMap;

    private void Awake() {

        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Initialize();
    }

    private void Initialize() {

        colorMap = new Dictionary<CardColor, Color>();

        foreach(ColorData data in colorMappings) {

            colorMap[data.colorType] = data.color;
        }
    }

    public Color GetColor(CardColor colorType) {

        return colorMap[colorType];
    }
}