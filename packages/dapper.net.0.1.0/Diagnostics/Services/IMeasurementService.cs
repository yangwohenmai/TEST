using System;
using System.Threading.Tasks;
using Dapper.Net.Diagnostics.Models;

namespace Dapper.Net.Diagnostics.Services {

    /// <summary>
    /// Measures the efficiency of a generic operation.
    /// </summary>
    /// <typeparam name="T">The type of measurement.</typeparam>
    public interface IMeasurementService<T> where T : IMeasurement {
        T Measure<TOutput>(Func<TOutput> operation, Func<TOutput, int> toCount);
        Task<T> MeasureAsync<TOutput>(Func<Task<TOutput>> operationAsync, Func<TOutput, int> toCount);
    }

}
