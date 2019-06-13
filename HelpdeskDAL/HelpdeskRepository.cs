/*
    Name:           HelpdeskRepository.cs
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        HelpdeskRepository holds methods that we will be calling and using in multiple 
                    classes and files in one place so that we nly have to implement them once,
                    and be able to use them for multiple objects and classes. We make templated
                    classes and methods for this.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace HelpdeskDAL
{
    // The HelpdeskRepository class holds all of the templated functions to be used on
    // any HelpdeskEntity, mainly the Department and Employee Objects
    public class HelpdeskRepository<T> : IRepository<T> where T : HelpdeskEntity
    {
        private HelpdeskContext ctx = null;

        // Create a constructor that will create a new HelpdeskContext object
        // Mainly for the Department and Employee and the set the properties
        // that they will be able to hold
        // (i.e. DepartmentName,DepartmentId, EmployeeTitle, EmployeeFirstname etc..)
        public HelpdeskRepository(HelpdeskContext context = null)
        {
            ctx = context != null ? context : new HelpdeskContext();
        }

        // GetAll() to get all the data types and Set them to the List
        // Mainly used for Department and Employee objects to be added
        // to a List
        public List<T> GetAll()
        {
            return ctx.Set<T>().ToList();
        }

        public List<T> GetByExpression(Expression<Func<T,bool>> lambdaExp)
        {
            return ctx.Set<T>().Where(lambdaExp).ToList();
        }

        // Add function to add and entity and save that entity in the database
        // Mainly used for adding a new employee
        public T Add(T entity)
        {
            ctx.Set<T>().Add(entity);
            ctx.SaveChanges();
            return entity;
        }

        // Update to check the status of the Entity. Make sure the data is not stale
        // when trying update an employee.
        public UpdateStatus Update(T updatedEntity)
        {
            UpdateStatus opStatus = UpdateStatus.Failed;

            try
            {
                HelpdeskEntity currentEntity = GetByExpression(ent => ent.Id == updatedEntity.Id).FirstOrDefault();
                ctx.Entry(currentEntity).OriginalValues["Timer"] = updatedEntity.Timer;
                ctx.Entry(currentEntity).CurrentValues.SetValues(updatedEntity);
                if (ctx.SaveChanges() == 1) // should throw exception if stale;
                    opStatus = UpdateStatus.Ok;
            }
            catch (DbUpdateConcurrencyException dbx)
            {
                opStatus = UpdateStatus.Stale;
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + dbx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + ex.Message);
            }
            return opStatus;
        }

        // Delete function to remove an entity from the database. Mainly used
        // to remove and Employee from the database
        public int Delete(int id)
        {
            // Creates an entity base on the type of entity whether it is a Deparment
            // or an Employee and retrieves the entity based on the ID passed to the function
            // Then removes that entity from the database and saves the changes
            T currentEntity = GetByExpression(ent => ent.Id == id).FirstOrDefault();
            ctx.Set<T>().Remove(currentEntity);
            return ctx.SaveChanges();
        }
    }
}
