using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.EntityFrameworkCore;

namespace ExercisesDAL
{
    public class SomeSchoolRepository<T>: IRepository<T> where T: SchoolEntity
    {
        readonly private SomeSchoolContext _db = null;

        public SomeSchoolRepository(SomeSchoolContext context=null)
        {
            _db = context ?? new SomeSchoolContext();
        }
        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public List<T> GetByExpression(Expression<Func<T,bool>>match)
        {
            return _db.Set<T>().Where(match).ToList();
        }

        public T Add(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
            return entity;
        }
        public UpdateStatus Update(T updateEntity)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                SchoolEntity currentEntity = GetByExpression(entx => entx.Id == updateEntity.Id).FirstOrDefault();
                    _db.Entry(currentEntity).OriginalValues["Timer"] = updateEntity.Timer;
                    _db.Entry(currentEntity).CurrentValues.SetValues(updateEntity);
                if (_db.SaveChanges() == 1)
                {
                    operationStatus = UpdateStatus.Ok;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                operationStatus = UpdateStatus.Stale;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Problem in "  +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }

            return operationStatus;
        }

        public int Delete(int id)
        {
            T currentEntity = GetByExpression(entx => entx.Id == id).FirstOrDefault();
            _db.Set<T>().Remove(currentEntity);
            return _db.SaveChanges();
        }
    }
}
