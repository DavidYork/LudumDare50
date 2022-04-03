using UnityEngine;

public class BuildingManager: MonoBehaviour {
    public Cost CurrentProject { get; private set; }
    public float HoursRemaining { get; private set; }
    public bool IsBuilding => _projectDetails != null;

    Database.BuildCost _projectDetails;
    bool _working;
    float _workTimestamp;

    public bool CanAfford(Cost upgrade) {
        var cost = Ludum.Dare.Data.GetCost(upgrade);
        var res = Ludum.Dare.Resources;

        if (res.Scrap.Amount < cost.Scrap || res.Fungus.Amount < cost.Fungus ||
            res.Energy.Amount < cost.Energy || res.Energy.Max < cost.Batteries)
        {
            return false;
        }
        return true;
    }

    public void GetToWork() {
        Debug.Log("Getting to work");
        Ludum.Dare.Computer.SetText($"You get to work on {_projectDetails.Name}.");
        Ludum.Dare.Commands.LoseFocus();
        _working = true;
        _workTimestamp = Time.time;
    }

    public void StartBuilding(Cost building) {
        CurrentProject = building;
        var bc = Ludum.Dare.Data.GetCost(building);
        var res = Ludum.Dare.Resources;
        _projectDetails = bc;

        HoursRemaining = bc.Hours;
        res.Scrap.Amount -= bc.Scrap;
        res.Energy.Amount -= bc.Energy;
        res.Energy.Max -= bc.Batteries;
        res.Fungus.Amount -= bc.Fungus;
        GetToWork();
    }

    // Private

    void Update() {
        if (!_working || Time.time < _workTimestamp + Ludum.Dare.Data.HourInGameTime || Ludum.Dare.GameIsOver) {
            return;
        }

        _workTimestamp = Time.time;

        if (Ludum.Dare.Temperature.IsBedtime) {
            _working = false;
            Ludum.Dare.GM.DoGoToBed();
            return;
        }

        // Go to sleep?
        Ludum.Dare.Temperature.Hour += 1f;

        _projectDetails.Hours--;
        if (_projectDetails.Hours <= 0) {
            finishProject();
        }
    }

    void finishProject() {
        Ludum.Dare.Commands.GainFocus();
        Ludum.Dare.Commands.RebuildActions("Commands");
        Ludum.Dare.Computer.SetText($"You have finished the {_projectDetails.Name}.");
        Ludum.Dare.Events.SetEvent(_projectDetails.CompletionEvent, true);
        _projectDetails = null;
        _working = false;
    }
}
