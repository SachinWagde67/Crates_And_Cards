using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ColorData {

    public CardColor colorType;
    public Color color;
}

public class ColorManager : MonoBehaviour {

    public static ColorManager Instance { get; private set; }

    [SerializeField] private List<ColorData> colorMappings = new List<ColorData>();

    private Dictionary<CardColor, Color> colorMap = new Dictionary<CardColor, Color>();

    private void Awake() {

        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Initialize();
    }

    private void Initialize() {

        foreach(ColorData data in colorMappings) {

            colorMap[data.colorType] = data.color;
        }
    }

    public Color GetColor(CardColor colorType) {

        return colorMap[colorType];
    }
}