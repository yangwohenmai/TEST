using System;
using System.Collections.Generic;

namespace Dapper.Net.Patterns.Chainable.Syntax.Object {

    public interface ISqlTable : ISqlObject {
        string TableRaw { get; }
        string AliasRaw { get; }
    }

    public interface ISqlColumn : ISqlObject {
        string ColumnRaw { get; }
        string AliasRaw { get; }
    }

    public interface ISqlValue : ISqlObject {
        
    }

    public interface ISqlColumnSet : ISqlObject {
        IEnumerable<ISqlColumn> Get();
        ISqlColumn Get(int ordinal);
        ISqlColumn Get(string alias);
    }

    /// <summary>
    /// A boolean expression that returns TRUE FALSE or UNKNOWN
    /// </summary>
    /// <remarks><see cref="https://msdn.microsoft.com/en-us/library/ms173545.aspx"/></remarks>
    /// <typeparam name="TColumn"></typeparam>
    public interface ISqlPredicate<TColumn> : ISqlObject where TColumn : ISqlColumn {


        
    }

    /// <summary>
    /// A column name, constant, function, variable, scalar subquery
    /// </summary>
    public interface ISqlExpression : ISqlObject {
        
    }
        

    public interface ISqlOperator : ISqlObject {}

    public static class SqlOperators {
        public static ISqlOperator Equal = new BooleanOperator(@"=");
        public static ISqlOperator NotEqual = new BooleanOperator(@"<>");
        public static ISqlOperator NotEqualAlt = new BooleanOperator(@"!=");
        public static ISqlOperator Greater = new BooleanOperator(@">");
        public static ISqlOperator GreaterEqual = new BooleanOperator(@">=");
        public static ISqlOperator NotGreater = new BooleanOperator(@"!>");
        public static ISqlOperator Less = new BooleanOperator(@"<");
        public static ISqlOperator LessEqual = new BooleanOperator(@"<=");
        public static ISqlOperator NotLess = new BooleanOperator(@"!<");

        public sealed class BooleanOperator : ISqlOperator {
            public string ToSql() => _value;
            private readonly string _value;
            public BooleanOperator(string value) { _value = value; }
        }
    }
}