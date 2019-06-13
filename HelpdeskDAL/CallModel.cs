using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace HelpdeskDAL
{
    public class CallModel
    {
        // IRepository interface where the Call is a HelpdeskEntity
        IRepository<Call> repo;

        public CallModel()
        {
            repo = new HelpdeskRepository<Call>();
        }

        public Call GetById(int id)
        {
            // Create a List to hold the Employee you are retrieving by Id
            List<Call> selectedCall = null;

            try
            {
                // Trys to populate the List by checking the database to see if the Id 
                // past into the function exists in the database
                selectedCall = repo.GetByExpression(call => call.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the First instace of that Id found in the database
            return selectedCall.FirstOrDefault();
        }

        public List<Call> GetAll()
        {
            // Create the List to hold all of the Call objects
            List<Call> allCalls = new List<Call>();

            try
            {
                // Gets all of the Calls and adds the objects to the List
                allCalls = repo.GetAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the List populated with the Call objects
            return allCalls;
        }
        // Add() function to Add an employee object to the database
        public int Add(Call newCall)
        {
            try
            {
                // Try and Add the employee object to the database
                repo.Add(newCall);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // If the Add was successful then return the new Id that Call 
            // object will have
            return newCall.Id;
        }

        // UpdateStatus to UpdateForConcurrency, this is to make sure the data the
        // user is using is not stale when they try and update a Call
        public UpdateStatus Update(Call updatedCall)
        {
            // Initially set to Failed because we haven't tried the Update yet
            UpdateStatus opStatus = UpdateStatus.Failed;

            try
            {
                // Try and Update the Call, if the update is successful then will
                // change the status(an integer) to 1, and -2 if the data is stale 
                opStatus = repo.Update(updatedCall);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the Status(integer) to tell if the update was successful, failed,
            // or if the data was stale
            return opStatus;
        }

        // Delete function to delete a call using the ID of the call object
        public int Delete(int id)
        {
            // Number of calls deleted
            int callsDeleted = -1;

            try
            {
                // Try and delete the call from the database. Calls the delete
                // function from the HelpdeskRepository.
                callsDeleted = repo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the number of employees deleted (Should be 1)
            return callsDeleted;
        }

    }
}
