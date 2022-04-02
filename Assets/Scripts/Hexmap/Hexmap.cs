using UnityEngine;

public class Hexmap: MonoBehaviour {
    [SerializeField] HexTile[,] Tiles;

    TMXMapInfo info;

    public Vector2Int SizeInHexes => new Vector2Int(info.width, info.height);
    public Vector2 SizeInWorld => new Vector2(
        (float)info.width * (float)info.tilewidth * .75f - info.width,
        (float)info.height * (float)info.tileheight);

    public void Setup(TMXMapInfo info, int layerNum = 0) {
        this.info = info;

        DestroyAllTiles();
        Tiles = new HexTile[info.width, info.height];

        var idx = 0;
        for (var y=0; y<info.height; y++) {
            for (var x=0; x<info.width; x++) {
                var tileID = info.layers[layerNum].data[idx];
                if (tileID > 0) {
                    var pos = new Vector2Int(x, -y);
                    var tile = HexFactory.Inst.CreateHexTile(tileID, info, pos, transform);
                    Tiles[x, y] = tile;
                }
                idx++;
            }
        }
    }

    public void DestroyAllTiles() {
        if (Tiles != null) {
            for (var y=0; y<Tiles.GetLength(1); y++) {
                for (var x=0; x<Tiles.GetLength(0); x++) {
                    var tile = Tiles[x, y];
                    if (tile != null) {
                        Destroy(Tiles[x, y].gameObject);
                    }
                }
            }
        }
        Tiles = null;
    }

    public void MirrorX() {
        for (var y=0; y<info.height; y++) {
            for (var x=0; x<info.width / 2; x++) {
                var tmp = Tiles[x, y];
                var right = info.width - x - 1;
                var leftPos = (Tiles[x, y] == null) ? Vector3.zero : Tiles[x, y].transform.position;
                var rightPos = (Tiles[right, y] == null) ? Vector3.zero : Tiles[right, y].transform.position;

                Tiles[x, y] = Tiles[right, y];
                Tiles[right, y] = tmp;


                if (Tiles[x, y] != null) {
                    Tiles[x, y].transform.position = leftPos;
                }

                if (Tiles[right, y] != null) {
                    Tiles[right, y].transform.position = rightPos;
                }
            }
        }
    }
}