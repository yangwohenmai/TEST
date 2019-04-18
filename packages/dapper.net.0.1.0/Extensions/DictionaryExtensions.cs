using System.Collections.Generic;
using Dapper.Net.Patterns.Chainable;

namespace Dapper.Net.Extensions {

    public static class DictionaryExtensions {
        public static ArgsMap ToArgsMap(this IDictionary<string, string> dictionary) => new ArgsMap(dictionary);
    }

}