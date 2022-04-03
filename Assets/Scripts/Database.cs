using System;
using System.Text;
using UnityEngine;

public class Database: MonoBehaviour {
    public float HexSize = 16f;
    public float KeyDebounceTime = .05f;

    [Serializable]
    public class ResourcesInfo {
        public int StartEnergy = 5;
        public int StartScrap = 0;
        public int StartFungus = 0;
    }
    public ResourcesInfo Resources;

    [Serializable]
    public class BuildCost {
        public Cost Upgrade;
        public int Scrap;
        public int Energy;
        public int Batteries;
        public int Fungus;

        public override string ToString() {
            var sb = new StringBuilder();
            if (Scrap > 0) { sb.Append($"{Scrap} scrap"); }
            if (Batteries > 0) { maybeComma(sb); sb.Append($"{Batteries} batteries"); }
            if (Energy > 0) { maybeComma(sb); sb.Append($"{Energy} energy"); }
            if (Fungus > 0) { maybeComma(sb); sb.Append($"{Fungus} fungus"); }
            if (sb.Length == 0) { sb.Append("0 scrap"); }
            return sb.ToString();
        }

        void maybeComma(StringBuilder sb) {
            if (sb.Length > 0) {
                sb.Append(", ");
            }
        }
    }
    public BuildCost[] BuildCosts;

    public BuildCost GetCost(Cost item) {
        foreach (var cost in BuildCosts) {
            if (cost.Upgrade == item) {
                return cost;
            }
        }
        Debug.LogError($"Cannot find cost for {item}");
        return new BuildCost() { Upgrade = item };
    }

    [Serializable]
    public class MapInfo {
        public int ScrapAmount = 3;
        public int FungusAmount = 1;
        public int BatteryAmount = 5;
        public int BatteryEnergyAmount = 5;
    }
    public MapInfo Map;
}
