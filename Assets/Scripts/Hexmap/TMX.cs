using System;
using UnityEngine;

[Serializable]
public class TMXMapInfo {

    [Serializable]
    public class TMXMapLayerInfo {
        public int[] data;
        public int height;
        public int id;
        public string name;
        public int opacity;
        public string type;
        public bool visible;
        public int width;
        public int x;
        public int y;
    }

    [Serializable]
    public class TMXTilesetReferenceInfo {
        public int firstgid;
        public string source;
        public string UnityAssetName => toAssetName(source);
    }


    [Serializable]
    public class TMXTilesetInfo {
        public int columns;
        public string image;
        public int imageheight;
        public int imagewidth;
        public int margin;
        public string name;
        public int spacing;
        public int tilecount;
        public string tiledversion;
        public int tileheight;
        public int tilewidth;
        public string type;
        public string version;
        public string UnityAssetName => toAssetName(name);
        public string UnityImageAssetName => toAssetName(image);
        public static TMXTilesetInfo FromJSON(string json) => JsonUtility.FromJson<TMXTilesetInfo>(json);
    }

    public int compressionlevel;
    public int height;
    public int hexsidelength;
    public bool infinite;
    public TMXMapLayerInfo[] layers;
    public int nextlayerid;
    public int nextobjectid;
    public string orientation;
    public string renderorder;
    public string staggeraxis;
    public string staggerindex;
    public string tiledversion;
    public int tileheight;
    public TMXTilesetReferenceInfo[] tilesets;
    public TMXTilesetInfo[] TilesetInstances;
    public int tilewidth;
    public string type;
    public string version;
    public int width;

    public static TMXMapInfo FromJSON(string json) {
        var tmx = JsonUtility.FromJson<TMXMapInfo>(json);
        tmx.TilesetInstances = new TMXTilesetInfo[tmx.tilesets.Length];
        for (var i=0; i<tmx.tilesets.Length; i++) {
            var src = tmx.tilesets[i];
            var textAsset = Resources.Load<TextAsset>(src.UnityAssetName);
            if (textAsset == null) {
                Debug.LogError($"Cannot find resource {src.UnityAssetName}");
            }
            tmx.TilesetInstances[i] = TMXTilesetInfo.FromJSON(textAsset.text);
        }
        return tmx;
    }

    public bool GetSourceTile(out int srcID, out TMXTilesetInfo tileset, int mapID) {
        tileset = null;
        for (var i=0; i<tilesets.Length && tileset == null; i++) {
            var current = TilesetInstances[i];
            var refInfo = tilesets[i];

            if (mapID >= refInfo.firstgid && mapID < refInfo.firstgid + current.tilecount) {
                tileset = current;
                srcID = mapID - refInfo.firstgid;
                return true;
            }
        }

        Debug.LogError($"Cannot find tile with ID {mapID}");
        srcID = -1;
        return false;
    }

    // Private

    static string toAssetName(string src) => System.IO.Path.GetFileNameWithoutExtension(src);
}
