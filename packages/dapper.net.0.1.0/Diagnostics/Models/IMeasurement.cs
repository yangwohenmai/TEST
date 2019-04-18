namespace Dapper.Net.Diagnostics.Models {

    public interface IMeasurement {
        /// <summary>
        /// The elapsed time the operation took to run in milliseconds.
        /// </summary>
        long Elapsed { get; }

        /// <summary>
        /// The count of items affected by the operation.
        /// </summary>
        int Affected { get; }

        /// <summary>
        /// The count of items affected per second.
        /// </summary>
        double Efficiency { get; }
    }

}