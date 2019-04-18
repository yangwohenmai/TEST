using System;
using Dapper.Net.Patterns.Syntax;

namespace Dapper.Net.Patterns.Attributes {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class GrammarAttribute : Attribute {
        public string Text { get; }

        public GrammarAttribute(params string[] series) {
            Text = String.Concat(series);
        }

        public override string ToString() => Text;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class GrammarPadBothAttribute : GrammarAttribute {
        public GrammarPadBothAttribute(params string[] series) : base(LanguageTokens.Space, String.Concat(series), LanguageTokens.Space) {}
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class GrammarPadLeftAttribute : GrammarAttribute {
        public GrammarPadLeftAttribute(params string[] series) : base(LanguageTokens.Space, String.Concat(series)) {}
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class GrammarPadRightAttribute : GrammarAttribute {
        public GrammarPadRightAttribute(params string[] series) : base(String.Concat(series), LanguageTokens.Space) {}
    }

}
