using SlurmFortress.Core.Models;
using System.Linq.Expressions;

namespace SlurmFortress.Core.Data;

public interface IDataAccess
{
    IDataTransaction CreateTransaction();
    void ExecuteCommands(IReadOnlyList<IStoreCommand> commands);
    IQueryable<T> Include<T, TP1>(IQueryable<T> query, Expression<Func<T, TP1>> expr1) where T : class;
    IQueryable<T> Include<T, TP1, TP2>(IQueryable<T> query, Expression<Func<T, TP1>> expr1, Expression<Func<TP1, TP2>> expr2) where T : class;
    IQueryable<T> Include<T, TP1, TP2>(IQueryable<T> query, Expression<Func<T, ICollection<TP1>>> expr1, Expression<Func<TP1, TP2>> expr2) where T : class;
    IQueryable<T> Query<T>() where T : class, IEntity;
    IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includes) where T : class, IEntity;
}
