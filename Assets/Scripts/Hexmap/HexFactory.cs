using System.Collections.Generic;
using UnityEngine;

public class HexFactory: MonoBehaviour {
    public static HexFactory Inst;

    public bool HideHexesOnCreate;

    [SerializeField] HexTile hexTilePrefab;
    [SerializeField] float depthSpaceBetweenHexRows = .001f;

    Dictionary<string, Texture2D> tilesets;
    Dictionary<string, Sprite> hexSprites;

    public HexTile CreateHexTile(int mapTileID, TMXMapInfo map, Vector2Int pos, Transform parent) {
        int tileSrcId;
        TMXMapInfo.TMXTilesetInfo tileset;
        if (!map.GetSourceTile(out tileSrcId, out tileset, mapTileID)) {
            return null;
        }

        var mapOffset = new Vector3(
            ((float)map.tilewidth * .75f * (float)(map.width)) / 2f,
            map.tileheight * (map.height) / -2,
            0);

        var tile = Instantiate<HexTile>(hexTilePrefab);
        tile.Setup(pos, mapTileID, SpriteFromTileset(tileSrcId, tileset), HideHexesOnCreate);
        tile.transform.SetParent(parent);
        var spacing = new Vector2(map.tilewidth, map.tileheight);
        tile.transform.localPosition = getHexPos(pos, spacing) - mapOffset;
        tile.name = $"Tile {pos} ({mapTileID})";
        return tile;
    }

    public Sprite SpriteFromTileset(int id, TMXMapInfo.TMXTilesetInfo tileset) {
        Sprite rv;
        var spriteID = $"{tileset.UnityAssetName}+{id}";
        if (hexSprites.TryGetValue(spriteID, out rv)) {
            return rv;
        }

        Texture2D spritesheet;
        if (!tilesets.TryGetValue(tileset.UnityImageAssetName, out spritesheet)) {
            spritesheet = Resources.Load<Texture2D>(tileset.UnityImageAssetName);
            if (spritesheet == null) {
                Debug.LogError($"Cannot find {tileset.UnityImageAssetName}");
            }
            tilesets.Add(tileset.UnityImageAssetName, spritesheet);
        }

        var tileRect = new Rect(
            tileset.tilewidth * (id % tileset.columns),
            tileset.imageheight - tileset.tileheight * (id / tileset.columns) - tileset.tileheight,
            tileset.tilewidth,
            tileset.tileheight);
        var pivot = new Vector2(.5f, .5f);
        rv = Sprite.Create(spritesheet, tileRect, pivot, 1, 0, SpriteMeshType.FullRect, Vector4.zero, false);
        hexSprites.Add(spriteID, rv);
        return rv;
    }

    // Private

    void Awake() {
        Inst = this;
        tilesets = new Dictionary<string, Texture2D>();
        hexSprites = new Dictionary<string, Sprite>();
    }

    Vector3 getHexPos(Vector2Int worldCoords, Vector2 spacing) {
        var vX = (float)worldCoords.x * spacing.x * .75f - spacing.x / -2f - (float)worldCoords.x;
        var vY = (float)worldCoords.y * spacing.y - spacing.y / 2f;

        if (worldCoords.x % 2 == 0) {
            vY += spacing.y / 2f;
        }

        var vZ = worldCoords.y * depthSpaceBetweenHexRows;
        if (worldCoords.x % 2 == 0) {
            vZ += depthSpaceBetweenHexRows / 2f;
        }
        return new Vector3(vX, vY, vZ);
    }
}
