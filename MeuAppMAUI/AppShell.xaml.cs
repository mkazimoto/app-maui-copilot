namespace MeuAppMAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register detail pages that are not part of the visual hierarchy.
		// Navigate to them with Shell.Current.GoToAsync("details").
		Routing.RegisterRoute("details", typeof(Views.DetailPage));
	}
}
