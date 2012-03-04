using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace CssSplitter {
	[TestFixture]
	public class CssRuleCountTest {
		[Test]
		public void empty_css_should_yield_count_0() {
			var css = "";
			Assert.That(RuleCount(css), Is.EqualTo(0));
		}

		[Test]
		public void single_rule_should_yield_count_1() {
			var css = @"div {height:0px}";
			Assert.That(RuleCount(css), Is.EqualTo(1));
		}

		[Test]
		public void single_rule_with_two_selectors_should_yield_2() {
			var css = @"div, a {height:0px}";
			Assert.That(RuleCount(css), Is.EqualTo(2));
		}

		[Test]
		public void single_rule_with_three_selectors_should_yield_3() {
			var css = @"div, a, h1 {height:0px}";
			Assert.That(RuleCount(css), Is.EqualTo(3));
		}

		[Test]
		public void two_rules_with_one_selector_should_yield_2() {
			var css = @"div { height:0px } a { color: #123 }";
			Assert.That(RuleCount(css), Is.EqualTo(2));
		}

		[Test]
		public void two_rules_with_two_selectors_should_yield_4() {
			var css = @"div, p { height:0px } a, h1 { color: #123 }";
			Assert.That(RuleCount(css), Is.EqualTo(4));
		}

		[Test]
		public void two_rules_with_two_and_three_selectors_should_yield_5() {
			var css = @"div, p { height:0px } a, h1, h3 { color: #123 }";
			Assert.That(RuleCount(css), Is.EqualTo(5));
		}

		[Test]
		public void camping_info_layout_css() {
			var css = File.ReadAllText(@"C:\Projects\Camping.info\Frontend.Web.CampingInfo\style\layout.css");
			Console.WriteLine(RuleCount(css));
		}

		private int RuleCount(string css) {
			return css == ""
			       	? 0
			       	: css.Count(c => c == ',' || c == '{');
			//var scount = new Regex("[^,{]+(,|{)").Matches(str).Count;
		}
	}
}