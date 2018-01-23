using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.BusinessLayer
{
    /// <summary>
    /// Class BaseBL.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class BaseBL : IDisposable
    {
        /// <summary>
        /// The disposed
        /// </summary>
        bool disposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BaseBL"/> class.
        /// </summary>
        ~BaseBL()
        {
            Dispose(false);
        }

        // Protected implementation of Dispose pattern. 
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (disposing)
                {
                    this.Dispose();
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
                disposing = true;
            }
        }

        /// <summary>
        /// Disposes the managed resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {

        }
        /// <summary>
        /// Disposes the unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources() { }
    }
}
