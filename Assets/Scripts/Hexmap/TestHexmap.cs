using UnityEngine;

public class TestHexmap: MonoBehaviour {
    [SerializeField] Hexmap map;
    [SerializeField] TextAsset tmxFile;

    void Start() {
        var tmx = TMXMapInfo.FromJSON(tmxFile.text);
        map.Setup(tmx, 0);

        var adjacent = new GameObject("Hexmap adjacent", new System.Type[] { typeof(Hexmap) });
        var newMap = adjacent.GetComponent<Hexmap>();
        newMap.Setup(tmx, 0);
        newMap.MirrorX();
        var firstMapSize = map.SizeInWorld;
        newMap.transform.localPosition = new Vector3(firstMapSize.x, -6, -0.05f);

        var thirdView = new GameObject("Hexmap adjacent", new System.Type[] { typeof(Hexmap) });
        var thirdMap = thirdView.GetComponent<Hexmap>();
        thirdMap.Setup(tmx, 0);
        thirdMap.MirrorX();
        thirdMap.transform.localPosition = new Vector3(firstMapSize.x/2 + 6, firstMapSize.y * .75f - 3f, 1);


    }
}
