using Dapper.Net.Extensions;
using Dapper.Net.Patterns.Chainable.Syntax.Object;
using G = Dapper.Net.Patterns.Syntax.SqlGrammar;
using J = Dapper.Net.Patterns.Syntax.SqlJoinDirection;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public class SqlJoin : SqlClause<ISqlJoin>, ISqlJoin {

        public override string ToSql() => $"{G.Join(DirectionRaw)}  {On(OnRaw)} {G.Clause(JoinRaw)}".TrimSqueeze();
        public J DirectionRaw { get; }
        public ISqlTable TableRaw { get; }
        public ISqlOn OnRaw { get; private set; }
        public ISqlJoin JoinRaw { get; private set; }

        public SqlJoin(J direction, ISqlTable obj, ISqlOn clause) {
            DirectionRaw = direction;
            TableRaw = obj;
            OnRaw = clause;
        }

        public ISqlJoin On(ISqlExpression clause, object param = null) => On(new SqlCondition(clause, param));
        public ISqlJoin On(ISqlCondition clause) => On(new SqlOn(clause));
        public ISqlJoin On(ISqlOn clause) => Chain(() => OnRaw = clause);

        public ISqlJoin InnerJoin(ISqlTable obj, ISqlOn clause) => Join(J.Inner, obj, clause);
        public ISqlJoin LeftJoin(ISqlTable obj, ISqlOn clause) => Join(J.Left, obj, clause);
        public ISqlJoin RightJoin(ISqlTable obj, ISqlOn clause) => Join(J.Right, obj, clause);
        public ISqlJoin OuterJoin(ISqlTable obj, ISqlOn clause) => Join(J.Outer, obj, clause);
        public ISqlJoin Join(J direction, ISqlTable obj, ISqlOn clause) => Join(new SqlJoin(direction, obj, clause));
        public ISqlJoin Join(ISqlJoin clause) => Chain(() => JoinRaw = clause);

    }

}