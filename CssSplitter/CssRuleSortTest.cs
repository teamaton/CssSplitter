using System;
using NUnit.Framework;

namespace CssSplitter {
	[TestFixture]
	public class CssRuleSortTest
	{
		[Test]
		public void single_rule_should_not_change() {
			var css = "div { height: 0 }";
			Assert.That(SortRules(css), Is.EqualTo(css));
		}

		[Test]
		public void sort_two_rules_by_length_of_selector() {
			var css = "div { height: 0; } a { color: #123; }";
			Assert.That(SortRules(css), Is.EqualTo("a { color: #123; } div { height: 0; }"));
		}

		private string SortRules(string css) {
			return css;
		}
	}
}