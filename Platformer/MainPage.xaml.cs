namespace Platformer;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		welcomeText.TranslateTo(100, -200);
		welcomeText.RotateTo(999999999, 99999999, Easing.BounceIn);
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		Preferences.Clear();
	}
}

