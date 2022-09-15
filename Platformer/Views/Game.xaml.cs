using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Platformer.Models;
using Platformer.ViewModel;
using Plugin.Maui.Audio;
using System.Runtime.Intrinsics.X86;
using System.Windows.Input;

namespace Platformer.Views;

public partial class Game : ContentPage
{

	private bool IsJumping = false;
    private double score = 0;

    // Upgrade cost variables \\
    double costShoe = 0;
    double MustacheCost = 0;

    // Score per jump \\
    double ScorePerJump = 1;
    // Other Methods/functions \\
    bool IsShopOpen = false;
    //
    private readonly IAudioManager audioManager;
    //

    public Game(UpgradesViewModel upgradesViewModel, IAudioManager audioManager)
	{
		InitializeComponent();
        BindingContext = upgradesViewModel;
        /////
        this.audioManager = audioManager;
        ////
        ShopGui.TranslateTo(500,0, 200);
        RefreshAll(1);
        if (Preferences.ContainsKey("SavedScore", ""))
        {
            score = Preferences.Get("SavedScore", 0.0);
        }
        else
        {
            Preferences.Set("SavedScore", 0.0);
        }

        if (Preferences.ContainsKey("PointsPerJump", ""))
        {
            ScorePerJump = Preferences.Get("PointsPerJump", 1.0);
        }
        else
        {
            Preferences.Set("PointsPerJump", ScorePerJump);
        }

        if (Preferences.ContainsKey("ShoeCost", ""))
        {
            costShoe = Preferences.Get("ShoeCost", 1.0);
        }
        else
        {
            Preferences.Set("ShoeCost", 10.0);
        }
        if (Preferences.ContainsKey("MustacheCost", ""))
        {
            MustacheCost = Preferences.Get("MustacheCost", 500.0);
        }
        else
        {
            Preferences.Set("MustacheCost", 500.0);
        }
        costShoe = Preferences.Get("ShoeCost", 0.0);
        MustacheCost = Preferences.Get("MustacheCost", 500.0);
        RefreshAll(1);

    }


	private async void Up_Pressed(object sender, EventArgs e)
	{
        var JumpSound = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("jump.wav"));
        if (IsJumping)
			return;
		else
		{
            Random rnd = new Random();
            int num = rnd.Next(1,100);
            IsJumping = true;
            JumpSound.Volume = 0.05;
            JumpSound.Play();
            if (num == 50)
            {
                player.Source = "mariojump.png";
                await player.TranslateTo(-this.X, this.Y - 700, 500, Easing.SinOut);
                await player.RotateTo(180);
                await player.TranslateTo(-this.X, 0, 1000, Easing.SinIn);
                player.Rotation = 0;
                var Shake = 0;
                player.Source = "mariostuck.png";
                score += 10 * ScorePerJump;
                RefreshAll(1);
                while (Shake < 5)
                {
                    Shake += 1;
                    await player.TranslateTo(20, 0, 100);
                    await player.TranslateTo(0, 0, 100);
                }
                player.Rotation = 180;
                player.Source = "mariojump.png";
                var b1 = player.TranslateTo(0, -300,500, Easing.SinOut);
                var b2 = player.RotateTo(0, 500);
                await Task.WhenAll(b1, b2);
                await player.TranslateTo(0, 0, 200);
                player.Source = "marioidle.png";
                IsJumping = false;
                score += ScorePerJump;
                Preferences.Set("SavedScore", score);
            }
            else
            {
                player.Source = "mariojump.png";
                await player.TranslateTo(-this.X, this.Y - 300, 500, Easing.SinOut);
                await player.TranslateTo(-this.X, 0, 350, Easing.SinIn);
                player.Source = "marioidle.png";
                IsJumping = false;
                score += ScorePerJump;
                Preferences.Set("SavedScore", score);
            }
            
            RefreshAll(1);

            SemanticScreenReader.Announce(ScoreDisplay.Text);
        }
	}


    private async void Shop_Pressed(object sender, EventArgs e)
    {
        if (!IsShopOpen)
        {
            IsShopOpen = true;
            await ShopGui.TranslateTo(0, 0, 500);
        }
        else
        {
            IsShopOpen = false;
            await ShopGui.TranslateTo(this.Width * 2, 0, 500);
        }
        
    }
    private void RefreshAll(object sender)
    {
        if (score == 1)
            ScoreDisplay.Text = $"Jumped {score} time";
        else
                if (score > 1000 && score < 1000000)
        {
            double convertedScore = Math.Round((double)score / 1000,1);
            ScoreDisplay.Text = $"{convertedScore}K Jumps";
        }
        else
        {
            ScoreDisplay.Text = $"{Math.Round(score,1)} Jumps";
        }
    }
    private void Upgrade_Pressed(object sender, EventArgs e)
    {
        var upgradeInfo = ((VisualElement)sender).BindingContext as Upgrade;
        if (upgradeInfo == null)
            return;
        var name = upgradeInfo.UpgradeName;
        double Cost = Preferences.Get($"{name}", 0.0);
        double multiplyBy = upgradeInfo.ExtraCostPerUpgrade;
        double newCost = Math.Round(Cost * multiplyBy,1);
        if (score > Cost)
        {
            upgradeInfo.DefaultCost = newCost;
            score -= Cost;
            Preferences.Set($"{name}", newCost);
            ScorePerJump += upgradeInfo.ExtraJumpsPerUpgrade;
            Preferences.Set("PointsPerJump", ScorePerJump);
            RefreshAll(1);
        }
        
    }
}