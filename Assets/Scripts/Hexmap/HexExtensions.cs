using UnityEngine;

public static class HexExtensions {
    public static Vector2Int Move(this Vector2Int src, Direction dir) {
        var rv = new Vector2Int(src.x, src.y);

        switch (dir) {
        case Direction.South:
            rv.y += 1;
            break;
        case Direction.SouthEast:
            rv.x += 1;
            rv.y += 1;
            break;
        case Direction.NorthEast:
            rv.x += 1;
            break;
        case Direction.North:
            rv.y -= 1;
            break;
        case Direction.NorthWest:
            rv.x -= 1;
            break;
        case Direction.SouthWest:
            rv.x -= 1;
            rv.y += 1;
            break;
        }

        if (src.x % 2 == 0 && (dir != Direction.North && dir != Direction.South)) {
            rv.y -= 1;
        }

        return rv;
    }

    public static HexType ToHexType(this HexTile tile) {
        var id = tile.TileID;

        switch (id) {
        case 1:
            return HexType.Silt;
        case 17:
        case 31:
            return HexType.Fungus;
        case 19:
        case 20:
            return HexType.Hab;
        case 21:
        case 23:
        case 29:
            return HexType.Scrap;
        case 25:
        case 27:
            return HexType.Battery;
        case 38: case 39: case 44: case 45:
        case 54:
            return HexType.Rough;
        }

        var row = (id - 1) / 16;
        if (row % 2 == 0 && id >= 64) {
            return HexType.Blocked;
        }
        if (id <= 16) {
            return HexType.Blocked;
        }
        return HexType.Open;
    }
}
