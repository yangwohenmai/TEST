using Dapper.Net.Extensions;
using Dapper.Net.Patterns.Chainable.Syntax.Object;
using G = Dapper.Net.Patterns.Syntax.SqlGrammar;

namespace Dapper.Net.Patterns.Chainable.Syntax.Clause {

    public class SqlCondition : SqlClause<ISqlCondition>, ISqlCondition {

        public override string ToSql() => $"{G.Not(IsInvertedRaw)} {ExpressionRaw.ToSql()} {G.And(AndRaw)} {G.Or(OrRaw)}".TrimSqueeze();
        public ISqlCondition AndRaw { get; private set; }
        public ISqlCondition OrRaw { get; private set; }
        public ISqlExpression ExpressionRaw { get; }
        public bool IsInvertedRaw { get; }
        public DynamicParameters ParamRaw { get; }


        public SqlCondition(ISqlExpression clause, object param = null, bool isInverted = false) {
            ExpressionRaw = clause;
            ParamRaw = new DynamicParameters(param);
            IsInvertedRaw = isInverted;
        }

        public static ISqlCondition Not(ISqlExpression clause, object param = null) => new SqlCondition(clause, param, true);
        public static ISqlCondition Not(ISqlCondition clause) => Not(clause.ExpressionRaw, clause.ParamRaw);

        public ISqlCondition And(ISqlExpression clause, object param = null) => And(new SqlCondition(clause, param));
        public ISqlCondition And(ISqlCondition clause) => Chain(() => AndRaw = clause);
        public ISqlCondition AndNot(ISqlExpression clause, object param = null) => And(Not(clause, param));
        public ISqlCondition AndNot(ISqlCondition clause) => And(Not(clause));

        public ISqlCondition Or(ISqlExpression clause, object param = null) => Or(new SqlCondition(clause, param));
        public ISqlCondition Or(ISqlCondition clause) => Chain(() => OrRaw = clause);
        public ISqlCondition OrNot(ISqlExpression clause, object param = null) => Or(Not(clause, param));
        public ISqlCondition OrNot(ISqlCondition clause) => Or(Not(clause));

        public object GetParam() {
            throw new System.NotImplementedException();
        }
    }

}