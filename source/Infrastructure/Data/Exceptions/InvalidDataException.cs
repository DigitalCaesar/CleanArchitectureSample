using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Exceptions;
public class InvalidDataException : DataException
{
    public InvalidDataException(string entityName) 
        : base($"The {entityName} is in an invalid state.") { }
}
