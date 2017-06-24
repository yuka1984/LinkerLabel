#region

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

#endregion

namespace LinkerLabel.Shared
{
	public class LinkerLabel : Label
	{
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource",
			typeof(IEnumerable), typeof(LinkerLabel), null, propertyChanged: OnItemsSourceChanged);

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create("Command", typeof(ICommand), typeof(LinkerLabel), null);

		public static readonly BindableProperty LinkColorProperty =
			BindableProperty.Create("LinkColor", typeof(Color), typeof(LinkerLabel), Color.Blue);

		private List<MatchWord> _matchWords = new List<MatchWord>();


		public ICommand Command
		{
			get => (ICommand) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}


		public IEnumerable ItemsSource
		{
			get => (IEnumerable) GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public Color LinkColor
		{
			get => (Color)GetValue(LinkColorProperty);
			set => SetValue(LinkColorProperty, value);
		}

		internal MatchWord[] MatchWords => _matchWords.ToArray();

		private static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var label = bindable as LinkerLabel;

			if (oldvalue is INotifyCollectionChanged)
				(oldvalue as INotifyCollectionChanged).CollectionChanged -= label.OnCollectionChanged;
			if (newvalue is INotifyCollectionChanged)
				(newvalue as INotifyCollectionChanged).CollectionChanged += label.OnCollectionChanged;
		}

		private void OnCollectionChanged(object sender,
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			UpdateMatchWords();
		}

		private void UpdateMatchWords()
		{
			if(ItemsSource == null)
				return;
			
			var txt = Text;
			var buffer = new List<MatchWord>();
			var sources = ItemsSource.Cast<string>().ToList();
			foreach (var source in sources)
			{
				var matches = Regex.Matches(txt, source);
				if (matches.Count > 0)
					foreach (Match match in matches)
						if (!buffer.Any(x => x.StartPosition <= match.Index && x.EndPositon >= match.Index))
							buffer.Add(new MatchWord
							{
								Word = source,
								StartPosition = match.Index
							});
			}
			_matchWords = buffer;
			OnPropertyChanged(nameof(MatchWords));
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == TextProperty.PropertyName
				|| propertyName == LinkColorProperty.PropertyName
				|| propertyName == ItemsSourceProperty.PropertyName)
			{
				UpdateMatchWords();
			}
		}

		internal struct MatchWord
		{
			public string Word { get; set; }

			public int StartPosition { get; set; }

			public int Length => Word?.Length ?? 0;

			public int EndPositon => StartPosition + Length;
		}
	}
}