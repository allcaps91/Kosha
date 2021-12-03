using System;

namespace ComBase
{
    public class MTSDisposable : IDisposable
    {
        public void Dispose()
        {

        }

        /// <summary>
        /// 관리되는 리소스 해제
        /// </summary>
        private void ReleaseManagedResources()
        {
            //Console.WriteLine("Releasing Managed Resources");
        }

        /// <summary>
        /// 관리되지 않는 리소스 해제
        /// </summary>
        void ReleaseUnmangedResources()
        {
            //Console.WriteLine("Releasing Unmanaged Resources");
        }

        protected virtual void Dispose(bool disposing)
        {
            //Console.WriteLine("Actual Dispose called with a " + disposing.ToString());
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();
        }

        ~MTSDisposable()
        {
            Console.WriteLine("Finalizer called");
            Dispose(false);
        }
    }
}
