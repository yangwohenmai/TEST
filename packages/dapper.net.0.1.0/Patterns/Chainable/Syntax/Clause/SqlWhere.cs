using Dapper.Net.Patterns.Chainable.Syntax.Object;
using S = Dapper.Net.Patterns.Syntax.SqlTokens;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public class SqlWhere : SqlClause<ISqlWhere>, ISqlWhere { 
        public override string ToSql() => $"{S.Where} {ConditionRaw.ToSql()}";
        public object GetParam() {
            throw new System.NotImplementedException();
        }
        public ISqlCondition ConditionRaw { get; }
        public ISqlCondition AndRaw { get; private set; }
        public ISqlCondition OrRaw { get; private set; }

        public SqlWhere(ISqlCondition clause) {
            ConditionRaw = clause;
        }

        public ISqlWhere And(ISqlExpression clause, object param = null) => And(new SqlCondition(clause, param));
        public ISqlWhere And(ISqlCondition clause) => Chain(() => AndRaw = clause);
        public ISqlWhere AndNot(ISqlExpression clause, object param = null) => And(new SqlCondition(clause, param, true));
        public ISqlWhere AndNot(ISqlCondition clause) => Chain(() => AndRaw = SqlCondition.Not(clause));

        public ISqlWhere Or(ISqlExpression clause, object param = null) => Or(new SqlCondition(clause, param));
        public ISqlWhere Or(ISqlCondition clause) => Chain(() => OrRaw = clause);
        public ISqlWhere OrNot(ISqlExpression clause, object param = null) => Or(new SqlCondition(clause, param, true));
        public ISqlWhere OrNot(ISqlCondition clause) => Chain(() => SqlCondition.Not(clause));

    }
}