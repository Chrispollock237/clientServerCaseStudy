/*
    Name:           UpdateStatus.cs       
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        Hold the numerated values on whether the Update was Ok, Failed, 
                    or the data was stale
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public enum UpdateStatus
    {
        Ok = 1,
        Failed = -1,
        Stale = -2
    };
}
