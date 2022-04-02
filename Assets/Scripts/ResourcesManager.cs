using UnityEngine;

public class ResourcesManager: MonoBehaviour {
    public ResourcesView Energy;
    public ResourcesView Scrap;
    public ResourcesView Fungus;

    public void OnStartNewGame() {
        Energy.Amount = Energy.Max = Ludum.Dare.Data.Resources.StartEnergy;
        Scrap.Amount = Ludum.Dare.Data.Resources.StartScrap;
        Fungus.Amount = Ludum.Dare.Data.Resources.StartFungus;
    }
}
