namespace MSIMAU;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}
	protected override Window CreateWindow(IActivationState? activationState) {
		var window = new Window(new AppShell());
		window.Created += Window_Created;
		return window;
	}

	private async void Window_Created(object? sender, EventArgs e) {
	const int defaultWidth = 1200;
	const int defaultHeight = 800;

	var window = (Window)sender!;
	window.Width = defaultWidth;
	window.Height = defaultHeight;
	window.X = window.Y = -defaultWidth;

	await window.Dispatcher.DispatchAsync(() => {});

	var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
	window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
	window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
}


}