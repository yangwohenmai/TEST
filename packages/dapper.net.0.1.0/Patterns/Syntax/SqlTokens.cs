namespace Dapper.Net.Patterns.Syntax {

    public static class SqlTokens {
        #region boolean
        public const string And = "AND";
        public const string Or = "OR";
        public const string Not = "NOT";
        #endregion

        #region equality
        public const string EqualTo = "=";
        public const string GreaterThan = ">";
        public const string GreaterThanEqualTo = ">=";
        public const string LessThan = "<";
        public const string LessThanEqualTo = "<=";
        public const string NotEqualTo = "<>";
        #endregion


        #region keyword
        public const string Select = "SELECT";
        public const string Update = "UPDATE";
        public const string Insert = "INSERT";
        public const string Delete = "DELETE";
        public const string From = "FROM";
        public const string Where = "WHERE";
        public const string With = "WITH";
        public const string Join = "JOIN";
        public const string On = "ON";
        public const string Inner = "INNER";
        public const string Outer = "OUTER";
        public const string Left = "LEFT";
        public const string Right = "RIGHT";
        public const string Set = "SET";
        public const string Into = "INTO";
        public const string Values = "VALUES";
        public const string Distinct = "DISTINCT";
        public const string Top = "TOP";
        public const string NoLock = "NOLOCK";
        #endregion

    }

}
