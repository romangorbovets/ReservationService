using Microsoft.EntityFrameworkCore.Storage;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.Repositories;
using ReservationService.Persistence.Repositories;

namespace ReservationService.Persistence;

/// <summary>
/// Реализация Unit of Work для управления транзакциями и координации работы репозиториев
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IReservationRepository? _reservations;
    private ITableRepository? _tables;
    private ICustomerRepository? _customers;
    private IRestaurantRepository? _restaurants;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReservationRepository Reservations =>
        _reservations ??= new ReservationRepository(_context);

    public ITableRepository Tables =>
        _tables ??= new TableRepository(_context);

    public ICustomerRepository Customers =>
        _customers ??= new CustomerRepository(_context);

    public IRestaurantRepository Restaurants =>
        _restaurants ??= new RestaurantRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction has not been started");

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            return;

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

