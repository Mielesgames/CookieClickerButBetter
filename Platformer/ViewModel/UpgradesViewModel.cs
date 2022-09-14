using Platformer.Models;
using System.Collections.ObjectModel;
using Platformer.Services;

namespace Platformer.ViewModel;

public partial class UpgradesViewModel : BaseViewModel
{
    public ObservableCollection<Upgrade> Upgrades { get; } = new();
    public Command GetUpgradesCommand { get; }
    UpgradeService upgradeService;

    public UpgradesViewModel(UpgradeService upgradeService)
    {
        this.upgradeService = upgradeService;
        GetUpgradesCommand = new Command(async () => await GetUpgradesAsync());
    }

    async Task GetUpgradesAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var upgrades = await upgradeService.GetUpgrades();

            if (upgrades.Count != 0)
                upgrades.Clear();
            foreach (var upgrade in upgrades)
                upgrades.Add(upgrade);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            Console.WriteLine("whoa you managed to get an error.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
