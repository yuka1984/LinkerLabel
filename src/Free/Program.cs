using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Free
{
    class Program
    {
        static void Main(string[] args)
        {
	        var txt = @"【2017年6月時点の情報で更新しました。】

Xamarin(ザマリンと読みます) とはなんぞや、個人開発者として使う時にどうなるの、的な事をさらっと書いてみようと思います。

Xamarin は 2016年2月、Microsoft に買収され、 Visual Studio に無償で同梱されることになりました。

【速報】Xamarin のこれからについて！ - Xamarin 日本語情報
Xamarin が Microsoft に買収された結果 - Qiita
Xamarin 自体は元企業名であり、その歴史は .NET の Linux 版を開発していた Ximian という企業が Novell に買収されて、その後レイオフされて作った企業で・・・した。
このあたりの歴史については @atsushieno さん や ちょまどさん のブログが（読み物としても）おもしろいです。
Microsoftに買収されたことにより企業としての Xamarin はなくなりますが、現在のところ Xamarin という開発ツールの名称は、Visual Studio や、Xamarin Studio の中に見ることができます。

Xamarin - Official site
Xamarin - Wikipedia
で、同社が開発した、 .NET技術で iOS や Android アプリが作成できる SDK が、Xamarin.iOS だったり、Xamarin.Android だったりするわけですが、それらに Mac アプリを開発できる Xamarin.Mac や、Xamarin Studio という統合開発環境を加えたツール群をまるごとひっくるめて Xamarin と呼んでいます。
（Xamarin社は他にも、クラウド上で実機テストができる Xamarin Test Cloud, C#のPlaygroundツールXamarin Workbooks などのプロダクトがあります）";

			var buffer = new List<MatchWord>();
			var e = new List<string> { "Xamarin", "Microsoft", "Xam", "ama", "rin", "Microsoft", "わっふる", "S" }.GetEnumerator();
	        while (e.MoveNext())
	        {
		        var matches = Regex.Matches(txt, e.Current);
		        if (matches.Count > 0)
		        {
			        foreach (Match match in matches)
			        {
				        if (!buffer.Any(x => x.StartPosition <= match.Index && x.EndPositon >= match.Index))
				        {
					        buffer.Add(new MatchWord
					        {
						        Word = e.Current,
								StartPosition = match.Index
					        });
				        }
			        }
		        }
	        }




	        e.Dispose();
			Console.WriteLine("Hello World!");
        }
    }
	internal class MatchWord
	{
		public string Word { get; set; }

		public int StartPosition { get; set; }

		public int Length => Word?.Length ?? 0;

		public int EndPositon => StartPosition + Length;
	}
}