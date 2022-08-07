using UnityEngine;
public enum ColorType
{
    Red = 0,
    Yellow,
    Green
}
public class BubbleColor
{
    private ColorType _colorType;
    public BubbleColor(SpriteRenderer spriteRenderer)
    {
        _colorType = (ColorType) Random.Range(0, (int) ColorType.Green + 1);
        spriteRenderer.color = GetRandomColor(_colorType);
    }
    private Color GetRandomColor(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Red:
                return Color.red;
            case ColorType.Yellow:
                return Color.yellow;
            case ColorType.Green:
                return Color.green;
        }
        return Color.white;
    }
}