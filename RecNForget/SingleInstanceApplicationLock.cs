using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecNForget
{
    // credit to https://codereview.stackexchange.com/a/141141 :)
    sealed class SingleInstanceApplicationLock : IDisposable
    {
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
                if (!_mutex.WaitOne(1000, false))
                    return false;
            }
            catch (AbandonedMutexException)
            {
                // Abandoned mutex, just log? Multiple instances
                // may be executed in this condition...
            }

            return _hasAcquiredExclusiveLock = true;
        }

        private const string MutexId = @"RecNForget{63A1DBD0-4181-4DCD-96F2-33F86F68F821}";
        private readonly Mutex _mutex = CreateMutex();
        private bool _hasAcquiredExclusiveLock, _disposed;

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed && _mutex != null)
            {
                try
                {
                    if (_hasAcquiredExclusiveLock)
                        _mutex.ReleaseMutex();

                    _mutex.Dispose();
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        private static Mutex CreateMutex()
        {
            var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var allowEveryoneRule = new MutexAccessRule(sid,
                MutexRights.FullControl, AccessControlType.Allow);

            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);

            var mutex = new Mutex(false, MutexId);
            mutex.SetAccessControl(securitySettings);

            return mutex;
        }
    }
}
