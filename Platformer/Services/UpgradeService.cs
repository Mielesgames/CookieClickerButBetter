using Platformer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Platformer.Services;

public class UpgradeService
{
    List<Upgrade> upgradeList = new();
    HttpClient httpClient;
    public UpgradeService()
    {
        this.httpClient = new HttpClient();
    }
    public async Task<List<Upgrade>> GetUpgrades()
    {
        if (upgradeList.Count > 0)
            return upgradeList;

        using var stream = await FileSystem.OpenAppPackageFileAsync("UpgradeData.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        upgradeList = JsonSerializer.Deserialize<List<Upgrade>>(contents);

        return upgradeList;
    }

}
