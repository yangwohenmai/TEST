using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.Net.Extensions;

namespace Dapper.Net.Patterns.Chainable {

    public interface IArgs {
        IEnumerable<string> List { get; }
        string CommaSeparated { get; }
        string AndSeparated { get; }
        string OrSeparated { get; }
        Args Except(params string[] exceptArgs);
        ArgsMap Map(params string[] values);
    }

    /// <summary>
    /// This class is the building block for working with strings
    /// </summary>
    public class Args : IArgs {
        protected readonly string[] ArgsRaw;

        public Args(params string[] args) {
            ArgsRaw = args;
        }

        public Args(IEnumerable<string> args) {
            ArgsRaw = args.ToArray();
        }

        public IEnumerable<string> List => ArgsRaw;
        public string CommaSeparated => string.Join(", ", ArgsRaw);
        public string AndSeparated => string.Join(" AND ", ArgsRaw);
        public string OrSeparated => string.Join(" OR ", ArgsRaw);

        public Args Except(params string[] exceptArgs) => ArgsRaw.Except(exceptArgs).ToArgs();
        public ArgsMap Map(params string[] values) {
            if(ArgsRaw.Length != values.Length) throw new ArgumentException($"Cannot map {ArgsRaw.Length} columns to {values.Length} values.");
            var dictionary = new Dictionary<string, string>();
            for(var i = 0; i < ArgsRaw.Length; i++) {
                dictionary.Add(ArgsRaw[i], values[i]);
            }
            return new ArgsMap(dictionary);
        }

        public override string ToString() => CommaSeparated;
    }

}