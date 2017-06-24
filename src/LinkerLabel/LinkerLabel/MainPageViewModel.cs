using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace LinkerLabel
{
	public class MainPageViewModel: INotifyPropertyChanged
	{
		private string _baseText;
		private string _linkText;
		private string _inputText;

		public MainPageViewModel()
		{
			_linkWords = new ObservableCollection<string>();
			LinkWords = new ReadOnlyObservableCollection<string>(_linkWords);
			EntryCommand = new Command<string>((input) =>
			{
				_linkWords.Add(input);
				InputText = "";
			});

			LinkCommand = new Command<string>(word =>
			{
				MessagingCenter.Send(this, "Alert", word);
			});
		}

		public string BaseText => @"Xamarin（ザマリン）とは、2011年5月、Mono、MonoTouch、Mono for Androidの開発者により設立された企業である[2]。これら3つのソフトウェアはCommon Language Infrastructure（CLI）並びにCommon Language Specifications（これらを合わせたものはMicrosoft .NETと同等の環境である）のクロスプラットフォームな実装である。すなわち、それぞれLinuxなどのUnix系OS、iPhoneなどのiOS、Android上で動作する.NET環境である。 WikiPediaより";
		public ReadOnlyObservableCollection<string> LinkWords { get; }
		public ObservableCollection<string> _linkWords { get; }
		public string InputText
		{
			get => _inputText;
			set => SetProperty(ref _inputText, value);
		}

		public Command<string> EntryCommand { get; }

		public Command<string> LinkCommand { get; }


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(storage, value)) return false;

			storage = value;
			OnPropertyChanged(propertyName);

			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



		}
	}
}
