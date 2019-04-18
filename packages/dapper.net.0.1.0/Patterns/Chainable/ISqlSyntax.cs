namespace Dapper.Net.Patterns.Chainable {

    /// <summary>
    /// Represents a chainable sql building block
    /// </summary>
    public interface ISqlSyntax {
        string ToSql();
    }

}