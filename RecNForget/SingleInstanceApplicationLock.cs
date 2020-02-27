using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace RecNForget
{
    // credit to https://codereview.stackexchange.com/a/141141 :)
    public sealed class SingleInstanceApplicationLock : IDisposable
    {
        private const string MutexId = @"RecNForget{63A1DBD0-4181-4DCD-96F2-33F86F68F821}";
        private readonly Mutex mutex = CreateMutex();
        private bool hasAcquiredExclusiveLock, disposed;

        ~SingleInstanceApplicationLock()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool TryAcquireExclusiveLock()
        {
            try
            {
                if (!mutex.WaitOne(1000, false))
                {
                    return false;
                }
            }
            catch (AbandonedMutexException)
            {
                // Abandoned mutex, just log? Multiple instances
                // may be executed in this condition...
            }

            return hasAcquiredExclusiveLock = true;
        }

        private static Mutex CreateMutex()
        {
            var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var allowEveryoneRule = new MutexAccessRule(sid, MutexRights.FullControl, AccessControlType.Allow);

            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);

            var mutex = new Mutex(false, MutexId);
            mutex.SetAccessControl(securitySettings);

            return mutex;
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !disposed && mutex != null)
            {
                try
                {
                    if (hasAcquiredExclusiveLock)
                    {
                        mutex.ReleaseMutex();
                    }

                    mutex.Dispose();
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}
