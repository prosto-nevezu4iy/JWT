using System;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        void Commit();
    }
}
