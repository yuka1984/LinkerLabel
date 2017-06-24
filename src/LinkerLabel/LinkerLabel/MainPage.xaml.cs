using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LinkerLabel
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			MessagingCenter.Subscribe<MainPageViewModel, string>(this, "Alert", (model, s) =>
			{
				DisplayAlert("Link Click", s, "Close");
			});
		}
	}
}
