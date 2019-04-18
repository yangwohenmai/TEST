namespace Dapper.Net.Patterns.Chainable
{
    public class SqlArgsMap
    {
        public SqlArgs Columns { get; }
        public SqlArgs Values { get; }

        public SqlArgsMap(ArgsMap argsMap)
        {
            Columns = new SqlArgs(argsMap.Columns);
            Values = new SqlArgs(Values);
        }
    }
}
