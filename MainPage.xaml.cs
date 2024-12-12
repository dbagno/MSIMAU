using Microsoft.Maui.Controls;
using MSIMAU.Services;

namespace MSIMAU;

public partial class MainPage : ContentPage
{
	private MidiService _midiService;

	public MainPage()
	{
		 
		_midiService=new MidiService();
		InitializeComponent();
		this.Loaded += (sender, e) =>
		{
			//MainPageWindow.MaximumWidthRequest = this.WidthRequest;
			//MainPageWindow.MinimumHeightRequest = this.HeightRequest;
			// Your initialization or handling code here
		};
		this.LayoutChanged += (sender, e) =>
        {
	        MainPageWindow.MaximumWidthRequest = this.WidthRequest;
	        MainPageWindow.MinimumHeightRequest = this.HeightRequest;
        };
	}

	
}

