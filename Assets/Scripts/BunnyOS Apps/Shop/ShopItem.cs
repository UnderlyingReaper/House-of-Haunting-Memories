using UnityEngine;

[CreateAssetMenu(fileName = "new Shop Item", menuName = "ScriptableObjects/Shop Item")]
public class ShopItem : ScriptableObject
{
    public GameObject customIconStyle;
    public Sprite sprite;
    public Color spriteCustomeColor = Color.white;
    public string nameOfItem;
    public float price;
}