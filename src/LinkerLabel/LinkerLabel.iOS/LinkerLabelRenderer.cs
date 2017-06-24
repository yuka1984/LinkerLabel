#region

using System.ComponentModel;
using System.Linq;
using Foundation;
using LinkerLabel.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#endregion

[assembly: ExportRenderer(typeof(LinkerLabel.Shared.LinkerLabel), typeof(LinkerLabelRenderer))]

namespace LinkerLabel.iOS
{
	public class LinkerLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			Control.UserInteractionEnabled = true;
			if (e.OldElement is Shared.LinkerLabel)
				(e.OldElement as Shared.LinkerLabel).PropertyChanged -= OnPropertyChanged;
			if (e.NewElement is Shared.LinkerLabel)
				(e.NewElement as Shared.LinkerLabel).PropertyChanged += OnPropertyChanged;
			Element.TextColor = Control.TextColor.ToColor();

			Control.AddGestureRecognizer(new UITapGestureRecognizer(Tap));
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(Shared.LinkerLabel.MatchWords)
			    || propertyChangedEventArgs.PropertyName == Label.TextProperty.PropertyName
			    || propertyChangedEventArgs.PropertyName == Shared.LinkerLabel.LinkColorProperty.PropertyName)
				UpdateLinker();
		}

		private void UpdateLinker()
		{
			var d = new NSMutableAttributedString(Element.Text, Control.Font, Element.TextColor.ToUIColor());
			var label = Element as Shared.LinkerLabel;
			d.AddAttribute(UIStringAttributeKey.ForegroundColor, Element.TextColor.ToUIColor(),
				new NSRange(0, Element.Text.Length - 1));
			foreach (var matchWord in label.MatchWords)
			{
				var range = new NSRange(matchWord.StartPosition, matchWord.Length);

				d.AddAttribute(UIStringAttributeKey.ForegroundColor, label.LinkColor.ToUIColor(), range);
				d.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int) NSUnderlineStyle.Single), range);
			}

			Control.AttributedText = d;
		}

		private void Tap(UITapGestureRecognizer tap)
		{
			var location = tap.LocationInView(Control);
			// Fale UseTextView
			var fakeTextViwe = new UITextView(Control.Frame);
			fakeTextViwe.TextContainer.LineFragmentPadding = 0;
			fakeTextViwe.TextContainerInset = UIEdgeInsets.Zero;
			fakeTextViwe.AttributedText = Control.AttributedText;
			var selectedPositon = fakeTextViwe.GetClosestPositionToPoint(location);
			var position = (int) fakeTextViwe.GetOffsetFromPosition(fakeTextViwe.BeginningOfDocument, selectedPositon);

			var label = Element as Shared.LinkerLabel;
			if (label.MatchWords.Any(x => x.StartPosition <= position && x.EndPositon >= position))
			{
				var word = label.MatchWords.First(x => x.StartPosition <= position && x.EndPositon >= position);
				if (label.Command?.CanExecute(word.Word) == true)
					label.Command.Execute(word.Word);
			}
		}
	}
}