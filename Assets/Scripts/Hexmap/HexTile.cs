using UnityEngine;

public class HexTile: MonoBehaviour {
    [SerializeField] SpriteRenderer ground;
    [SerializeField] SpriteRenderer fogOfWar;

    public Vector2Int Pos { get; private set; }
    public int TileID { get; set; }

    TMXMapInfo _map;

    public bool HiddenByFog {
        get => fogOfWar.gameObject.activeSelf;
        set {
            fogOfWar.gameObject.SetActive(value);
            ground.gameObject.SetActive(!value);
        }
    }

    public void ChangeHexType(int newID) {
        TileID = newID;
        int tileSrcId;
        TMXMapInfo.TMXTilesetInfo tileset;

        if (!_map.GetSourceTile(out tileSrcId, out tileset, newID)) {
            Debug.LogError($"Cannot find tile {newID}");
            return;
        }

        ground.sprite = HexFactory.Inst.SpriteFromTileset(tileSrcId, tileset);
    }

    public void Setup(TMXMapInfo map, Vector2Int pos, int tileID, Sprite ground, bool hiddenByFog) {
        _map = map;
        Pos = pos;
        TileID = tileID;
        HiddenByFog = hiddenByFog;
        this.ground.sprite = ground;
    }
}
