namespace CodeGeneratorCommon
{
    /// <summary>
    /// Interface for an object that may support thread-safe access.
    /// </summary>
    public interface IThreadSync
    {
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="IThreadSync"/>.
        /// </summary>
        object SyncRoot { get; }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="IThreadSync"/> is synchronized (thread safe).
        /// </summary>
        bool IsSynchronized { get; }
    }
}