using UnityEngine;

public class ResourcesManager: MonoBehaviour {
    public ResourcesView Energy;
    public ResourcesView EnergyToHeat;
    public ResourcesView Heat;
    public ResourcesView Fungus;
    public ResourcesView Scrap;

    public void OnStartNewGame() {
        Energy.Amount = Energy.Max = Ludum.Dare.Data.Resources.StartEnergy;
        Scrap.Amount = Ludum.Dare.Data.Resources.StartScrap;
        Fungus.Amount = Ludum.Dare.Data.Resources.StartFungus;
        Heat.Amount = 0;
        EnergyToHeat.Amount = Ludum.Dare.Data.Resources.StartEnergyToHeat;
    }
}
