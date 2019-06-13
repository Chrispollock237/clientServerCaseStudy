/*
    Name:           IRepository.cs       
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        Contains the interface which will have a List GetAll() and
                    GetByExpression() fucntion, an Add() entity, UpdateStatus()
                    to check for concurrency, and a Delete() function all to be
                    implemented in the Department or Employee Model classes.
                    
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{ 
    public interface IRepository<T>
    {
        List<T> GetAll();
        List<T> GetByExpression(Expression<Func<T, bool>> lambdaExp);
        T Add(T entity);
        UpdateStatus Update(T entity);
        int Delete(int i);
    }
}
