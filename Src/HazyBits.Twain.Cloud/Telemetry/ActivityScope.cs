using System;

namespace HazyBits.Twain.Cloud.Telemetry
{
    /// <summary>
    /// Activity scope is a named region that marks start and end of an operation.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class ActivityScope: IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityScope"/> class.
        /// </summary>
        /// <param name="name">The name of the scope.</param>
        protected ActivityScope(string name)
        {
            ActivityName = name;
        }

        #region Public Methods

        /// <summary>
        /// Gets the empty activity scope.
        /// </summary>
        public static ActivityScope Empty { get; } = new EmptyScope();

        /// <summary>
        /// Gets name of the current activity.
        /// </summary>
        public string ActivityName { get; }

        #endregion

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private class EmptyScope: ActivityScope
        {
            public EmptyScope() : base(string.Empty)
            { }

            protected override void Dispose(bool disposing)
            { }
        }
    }
}
