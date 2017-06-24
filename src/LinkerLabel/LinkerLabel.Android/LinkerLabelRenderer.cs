using System;
using System.ComponentModel;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using LinkerLabel.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(LinkerLabel.Shared.LinkerLabel), typeof(LinkerLabelRenderer))]
namespace LinkerLabel.Droid
{
	public class LinkerLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement is LinkerLabel.Shared.LinkerLabel)
			{
				(e.OldElement as Shared.LinkerLabel).PropertyChanged -= OnPropertyChanged;
			}
			if (e.NewElement is LinkerLabel.Shared.LinkerLabel)
			{
				(e.NewElement as Shared.LinkerLabel).PropertyChanged += OnPropertyChanged;
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(Shared.LinkerLabel.MatchWords)
				|| propertyChangedEventArgs.PropertyName == Shared.LinkerLabel.TextProperty.PropertyName
				|| propertyChangedEventArgs.PropertyName == Shared.LinkerLabel.LinkColorProperty.PropertyName)
			{
				UpdateLinker();
			}
		}

		private void UpdateLinker()
		{
			var ss = new SpannableString(Element.Text);
			var label = (Element as Shared.LinkerLabel);
			foreach (var matchWord in label.MatchWords)
			{
				ss.SetSpan(new AnonymousClickableSpan()
				{
					LinkColor = label.LinkColor.ToAndroid(),
					ONClickAction = view =>
					{
						var linkerLabel = Element as Shared.LinkerLabel;
						if (linkerLabel?.Command?.CanExecute(matchWord.Word) == true)
						{
							linkerLabel.Command.Execute(matchWord.Word);
						}
					}

				}, matchWord.StartPosition, matchWord.EndPositon, SpanTypes.InclusiveInclusive);
			}
			Control.TextFormatted = ss;
			Control.MovementMethod = LinkMovementMethod.Instance;
		}
		private class AnonymousClickableSpan : ClickableSpan
		{
			public Android.Graphics.Color? LinkColor { get; set; } 
			public Action<Android.Views.View> ONClickAction { get; set; }
			public override void OnClick(Android.Views.View widget)
			{
				ONClickAction?.Invoke(widget);
			}

			public override void UpdateDrawState(TextPaint ds)
			{
				if (LinkColor != null)
				{
					ds.LinkColor = LinkColor.Value;
				}				
				base.UpdateDrawState(ds);
			}
		}
	}
}