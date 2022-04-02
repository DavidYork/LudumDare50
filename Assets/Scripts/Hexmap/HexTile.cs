using UnityEngine;

public class HexTile: MonoBehaviour {
    [SerializeField] SpriteRenderer ground;
    [SerializeField] SpriteRenderer fogOfWar;

    public Vector2Int Pos { get; private set; }
    public int TileID { get; set; }

    public bool HiddenByFog {
        get => fogOfWar.gameObject.activeSelf;
        set {
            fogOfWar.gameObject.SetActive(value);
            ground.gameObject.SetActive(!value);
        }
    }

    public void Setup(Vector2Int pos, int tileID, Sprite ground, bool hiddenByFog) {
        Pos = pos;
        TileID = tileID;
        HiddenByFog = hiddenByFog;
        this.ground.sprite = ground;
    }
}
