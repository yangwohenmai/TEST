using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Dapper.Net.Extensions;
using Dapper.Net.Patterns.Chainable;
using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using S = Dapper.Net.Patterns.Syntax.SqlTokens;

namespace Dapper.Net.Patterns.Syntax {

    public static class SqlGrammar {
        public static string Top(int? top) => top != null ? $"{S.Top}({top})" : null;
        public static string Distinct(bool isDistinct) => isDistinct ? S.Distinct : null;
        public static string NoLock(bool isNoLock) => isNoLock ? $"{S.With} ({S.NoLock})" : null;
        public static string Join(SqlJoinDirection direction) => direction.GetDescription();
        public static string On(ISqlOn onRaw) => onRaw != null ? $"{S.On} {Clause(onRaw)}" : null;
        public static string Clause(ISqlClause clause) => clause?.ToSql();
        public static string And(ISqlCondition condition) => condition != null ? $"{S.And} {condition.ToSql()}" : null;
        public static string Or(ISqlCondition condition) => condition != null ? $"{S.Or} {condition.ToSql()}" : null;
        public static string Not(bool isInverted) => isInverted ? $"{S.Not}" : null;
        public static string Columns(string columns) => columns != null ? $"({columns})" : null;
        public static string Values(string values) => values != null ? $"{S.Values} ({values})" : null;

        public static string EqualTo(ISqlSyntax first, ISqlSyntax second) => Paren(first, S.EqualTo, second);
        public static string LessThan(ISqlSyntax first, ISqlSyntax second) => Paren(first, S.LessThan, second);
        public static string LessThanEqualTo(ISqlSyntax first, ISqlSyntax second) => Paren(first, S.LessThanEqualTo, second);
        public static string GreaterThan(ISqlSyntax first, ISqlSyntax second) => Paren(first, S.GreaterThan, second);
        public static string GreaterThanEqualTo(ISqlSyntax first, ISqlSyntax second) => Paren(first, S.GreaterThanEqualTo, second);

        private static string Paren(ISqlSyntax first, string oper, ISqlSyntax second) => $"({first.ToSql()} {oper} {second.ToSql()})";

        public static string Comma<T>(IEnumerable<T> items) where T : ISqlSyntax {
            var q = new Queue<T>(items);
            if (!q.Any()) return null;
            var sb = new StringBuilder(q.Dequeue().ToSql());
            while (q.Any()) {
                sb.Append($",{q.Dequeue().ToSql()}");
            }
            return sb.ToString();
        }

    }

    public enum SqlJoinDirection {
        [Description(S.Inner + " " + S.Join)]
        Inner,

        [Description(S.Left + " " + S.Join)]
        Left,

        [Description(S.Right + " " + S.Join)]
        Right,

        [Description(S.Outer + " " + S.Join)]
        Outer
    }

}