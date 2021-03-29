using Application.Common.Interfaces;
using System;
using System.Data;
using Infrastructure.Repositories;
using System.Data.SqlClient;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _dbConnection;
        private IDbTransaction _transaction;
        private IUserRepository _userRepository;
        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
            _dbConnection.Open();
            _transaction = _dbConnection.BeginTransaction();
        }

        public IUserRepository Users
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_transaction)); }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _dbConnection.BeginTransaction();
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _userRepository = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_dbConnection != null)
                    {
                        _dbConnection.Dispose();
                        _dbConnection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
