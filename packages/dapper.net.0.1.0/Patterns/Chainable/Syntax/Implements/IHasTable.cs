using Dapper.Net.Patterns.Chainable.Syntax.Object;

namespace Dapper.Net.Patterns.Chainable.Syntax.Implements {

    public interface IHasTable {
        ISqlTable TableRaw { get; }
    }

}