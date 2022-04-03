using UnityEngine;

public class HeatManager: MonoBehaviour {
    public void ProcessHour(out bool dead) {
        dead = false;
        if (Ludum.Dare.Temperature.Hour > 24f) {
            Debug.LogWarning("Can we check temperature past midnight?");
            return;
        }

        var cold = Ludum.Dare.Temperature.CurrentColdDamage;

        while (cold > 0 && !dead) {
            dead = !shedOneHeat(ref cold);
        }
    }

    bool shedOneHeat(ref int cold) {
        var heat = Ludum.Dare.Resources.Heat;
        if (heat.Amount > 0) {
            heat.Amount--;
            cold--;
            return true;
        }

        var energy = Ludum.Dare.Resources.Energy;
        if (energy.Amount > 0) {
            energy.Amount--;
            heat.Amount += Ludum.Dare.Resources.EnergyToHeat.Amount;

            if (heat.Amount > 0) {
                heat.Amount--;
                cold--;
                return true;
            }
        }

        return false;
    }
}
