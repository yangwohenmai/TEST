using Dapper.Net.Extensions;
using Dapper.Net.Patterns.Chainable.Syntax.Object;
using S = Dapper.Net.Patterns.Syntax.SqlTokens;
using G = Dapper.Net.Patterns.Syntax.SqlGrammar;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public class SqlOn : SqlClause<ISqlOn>, ISqlOn {
        public override string ToSql() => $"{S.On} {G.Clause(ConditionRaw)}".TrimSqueeze();
        public ISqlCondition ConditionRaw { get; }
        public ISqlCondition AndRaw { get; private set; }
        public ISqlCondition OrRaw { get; private set; }

        public SqlOn(ISqlCondition clause) {
            ConditionRaw = clause;
        }

        public ISqlOn And(ISqlExpression clause, object param = null) => And(new SqlCondition(clause, param));
        public ISqlOn And(ISqlCondition clause) => Chain(() => AndRaw = clause);
        public ISqlOn AndNot(ISqlExpression clause, object param = null) => And(SqlCondition.Not(clause, param));
        public ISqlOn AndNot(ISqlCondition clause) => And(SqlCondition.Not(clause));

        public ISqlOn Or(ISqlExpression clause, object param = null) => Or(new SqlCondition(clause, param));
        public ISqlOn Or(ISqlCondition clause) => Chain(() => OrRaw = clause);
        public ISqlOn OrNot(ISqlExpression clause, object param = null) => Or(SqlCondition.Not(clause, param));
        public ISqlOn OrNot(ISqlCondition clause) => Or(SqlCondition.Not(clause));

    }

}