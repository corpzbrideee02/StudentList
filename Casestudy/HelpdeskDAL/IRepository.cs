using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HelpdeskDAL
{
    public interface IRepository<T>
    {
        List<T> GetAll();

        List<T> GetByExpression(Expression<Func<T, bool>> match);

        T Add(T entity);

        UpdateStatus Update(T enity);

        int Delete(int i);
    }
}
