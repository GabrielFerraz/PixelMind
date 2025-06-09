using UnityEngine;
using System.Collections.Generic;

public class Duck : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [System.Serializable]
    public struct ColorSpritePair
    {
        public string colorName;
        public Sprite aliveSprite;
        public Sprite deadSprite;
    }

    [Header("Sprites por Cor")]
    public ColorSpritePair[] colorSpritePairs;

    private Dictionary<string, Sprite> aliveSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> deadSprites = new Dictionary<string, Sprite>();

    public string CurrentColor { get; private set; }
    public bool wasHit { get; private set; }

    private bool isClickable = false;

    void Start()
    {
        foreach (var pair in colorSpritePairs)
        {
            aliveSprites[pair.colorName] = pair.aliveSprite;
            deadSprites[pair.colorName] = pair.deadSprite;
        }

        GameManagerDuck.Instance.RegisterDuck(this);
    }

    public void SetRandomColor(string[] colorNames)
    {
        string selected = colorNames[Random.Range(0, colorNames.Length)];
        CurrentColor = selected;

        if (aliveSprites.TryGetValue(selected, out Sprite selectedSprite))
        {
            spriteRenderer.sprite = selectedSprite;
        }

        wasHit = false;
    }

    public void SetClickable(bool clickable)
    {
        isClickable = clickable;
    }

    void OnMouseDown()
    {
        if (!isClickable || wasHit) return;

        wasHit = true;

        if (deadSprites.TryGetValue(CurrentColor, out Sprite deadSprite))
        {
            spriteRenderer.sprite = deadSprite;
        }
    }

    public void ResetDuck()
    {
        wasHit = false;

        if (aliveSprites.TryGetValue(CurrentColor, out Sprite sprite))
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
