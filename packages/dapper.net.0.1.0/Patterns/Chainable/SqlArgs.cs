namespace Dapper.Net.Patterns.Chainable {
    public class SqlArgs : Args
    {
        private readonly Args _args;

        public SqlArgs(Args args)
        {
            _args = args;
        }

    }
}