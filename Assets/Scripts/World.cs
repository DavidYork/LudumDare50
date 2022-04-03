using System.Collections.Generic;
using UnityEngine;

public class World: MonoBehaviour {
    [SerializeField] Transform mapRoot;
    [SerializeField] TextAsset _mapCenterOfWorld;

    Dictionary<Vector2Int, Hexmap> _maps;
    TMXMapInfo _mapData;
    Vector2Int _mapSizeInHexes;

    public HexType GetHexType(Vector2Int worldPos) {
        var tile = GetHexTileAt(worldPos);
        var rv = tile.ToHexType();
        return rv;
    }

    public HexTile GetHexTileAt(Vector2Int worldPos) {
        Vector2Int localPos;
        Vector2Int regionPos;
        worldPosToRegion(worldPos, out localPos, out regionPos);
        var hexData = _maps[regionPos];
        return hexData.GetHexTile(localPos);
    }

    public void SpawnMissingMaps() {
        Vector2Int regionPos, localPos;
        worldPosToRegion(Ludum.Dare.Rover.Position, out localPos, out regionPos);
        for (var x = -1; x <= 1; x++) {
            for (var y = -1; y <= 1; y++) {
                var adjacent = new Vector2Int(regionPos.x + x, regionPos.y + y);
                if (!_maps.ContainsKey(adjacent)) {
                    _maps.Add(adjacent, spawnMap(adjacent));
                }
            }
        }
    }

    // Private

    void Awake() {
        _maps = new Dictionary<Vector2Int, Hexmap>();
        _mapData = TMXMapInfo.FromJSON(_mapCenterOfWorld.text);
        _mapSizeInHexes = new Vector2Int(_mapData.width, _mapData.height);
        var centerGO = new GameObject("Center of world", new System.Type[] { typeof(Hexmap) });
        var centerView = centerGO.GetComponent<Hexmap>();
        centerView.Setup(_mapData, 0);
        centerView.transform.SetParent(mapRoot);
        centerView.transform.localPosition = Vector3.zero;
        _maps.Add(Vector2Int.zero, centerView);
    }

    void Start() {
        SpawnMissingMaps();
        Ludum.Dare.Rover.StartPosition = new Vector2Int(_mapSizeInHexes.x / 2, _mapSizeInHexes.y / 2);
        Ludum.Dare.Rover.SetPosition(Ludum.Dare.Rover.StartPosition);
    }

    Hexmap spawnMap(Vector2Int regionPos) {
        var idx = Random.Range(1, _mapData.layers.Length);
        var centerGO = new GameObject($"Region {regionPos}", new System.Type[] { typeof(Hexmap) });
        var centerView = centerGO.GetComponent<Hexmap>();
        centerView.Setup(_mapData, idx);
        centerView.transform.SetParent(mapRoot);
        centerView.transform.localPosition = new Vector3(
            regionPos.x * centerView.SizeInWorld.x,
            -regionPos.y * centerView.SizeInWorld.y,
            -(float)regionPos.y * 2f
        );
        return centerView;
    }

    void worldPosToRegion(Vector2Int worldPos, out Vector2Int localPos, out Vector2Int regionPos) {
        regionPos = Vector2Int.zero;
        localPos = worldPos;
        while (localPos.x < 0) {
            regionPos.x--;
            localPos.x += _mapSizeInHexes.x;
        }
        while (localPos.y < 0) {
            regionPos.y--;
            localPos.y += _mapSizeInHexes.y;
        }
        while (localPos.x >= _mapSizeInHexes.x) {
            regionPos.x++;
            localPos.x -= _mapSizeInHexes.x;
        }
        while (localPos.y >= _mapSizeInHexes.y) {
            regionPos.y++;
            localPos.y -= _mapSizeInHexes.y;
        }
    }
}
