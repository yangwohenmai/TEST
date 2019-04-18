using System.Collections.Generic;
using System.Linq;
using Dapper.Net.Extensions;

namespace Dapper.Net.Patterns.Chainable
{
    public class ArgsMap : Args
    {
        public IDictionary<string, string> Map { get; }
        public Args Columns { get; }
        public Args Values { get; private set; }

        public ArgsMap(IDictionary<string, string> map) {
            Map = map;
            Columns = map.Keys.ToArgs();
            Values = map.Values.ToArgs();
        }

        public new ArgsMap Except(params string[] exceptArgs) => Map.Where(a => exceptArgs.All(except => except != a.Key)).ToArgsMap();
    }
}
