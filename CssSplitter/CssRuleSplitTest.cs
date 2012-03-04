using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace CssSplitter {
	[TestFixture]
	public class CssRuleSplitTest
	{
		[Test]
		public void single_rule_should_not_split() {
			var css = "div { height: 0 }";
			Assert.That(SplitRules(css, 1)[0], Is.EqualTo(css));
			Assert.That(SplitRules(css, 1)[1].Length, Is.EqualTo(0));
		}

		[Test]
		public void whitespace_trimming_should_be_ignored() {
			var css = "div { height: 0 } ";
			Assert.That(SplitRules(css, 1)[0], Is.EqualTo(css.Trim()));
			Assert.That(SplitRules(css, 1)[1].Length, Is.EqualTo(0));
		}

		[Test]
		public void two_rules_should_split_when_max_is_1() {
			var css = "div { height: 0 } a { color: #123; }";
			Assert.That(SplitRules(css, 1)[0], Is.EqualTo("div { height: 0 }"));
			Assert.That(SplitRules(css, 1)[1], Is.EqualTo("a { color: #123; }"));
		}

		[Test]
		public void three_rules_should_split_into_2n1_when_max_is_2() {
			var css = @"
			div { height: 0 }
			a { color: #123; }

			b { font: arial; }";
			Assert.That(SplitRules(css, 2)[0], Is.EqualTo("div { height: 0 } a { color: #123; }"));
			Assert.That(SplitRules(css, 2)[1], Is.EqualTo("b { font: arial; }"));
		}

		[Test]
		public void five_rules_should_split_into_3n2_when_max_is_3() {
			var css = @"
			div, h1 { height: 0 }
			a { color: #123; }

			b, h2 { font: arial; }";
			Assert.That(SplitRules(css, 3)[0], Is.EqualTo("div, h1 { height: 0 } a { color: #123; }"));
			Assert.That(SplitRules(css, 3)[1], Is.EqualTo("b, h2 { font: arial; }"));
		}

		[Test]
		public void jump_over_rule_when_crossing_limit() {
			var css = @"
			div, h1 { height: 0 }
			a, b { color: #123; }

			b, h2 { font: arial; }";
			Assert.That(SplitRules(css, 3)[0], Is.EqualTo("div, h1 { height: 0 }"));
			Assert.That(SplitRules(css, 3)[1], Is.EqualTo("a, b { color: #123; } b, h2 { font: arial; }"));
		}

		[Test]
		public void split_large_css_file() {
			var combined = File.ReadAllText(@"C:\Projects\Camping.info\Frontend.Web.CampingInfo\style\combined.css");
			var split = SplitRules(combined, 4095);
			File.WriteAllText(@"C:\Projects\Camping.info\Frontend.Web.CampingInfo\style\layout-2.css", split[1]);
		}

		private string[] SplitRules(string css, int maxRules) {
			var rules = css.Trim().Split(new[] { '}' }, StringSplitOptions.RemoveEmptyEntries);
			var rulesSum = 0;
			var rulesToMax = rules.TakeWhile(rule => (rulesSum += RuleCount(rule)) <= maxRules).ToList();
			return new[]
			       	{
			       		string.Join(" ", rulesToMax.Select(s => s.Trim() + " }")),
			       		string.Join(" ", rules.Skip(rulesToMax.Count()).Select(s => s.Trim() + " }"))
			       	};
		}

		private int RuleCount(string css)
		{
			return css == ""
					? 0
					: css.Count(c => c == ',' || c == '{');
			//var scount = new Regex("[^,{]+(,|{)").Matches(str).Count;
		}
	}
}