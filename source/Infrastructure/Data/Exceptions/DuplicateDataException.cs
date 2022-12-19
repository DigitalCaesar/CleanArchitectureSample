using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Exceptions;
public class DuplicateDataException : DataException
{
    public DuplicateDataException(string entityName, string entityId) 
        : base($"The {entityName} '{entityId}' already exists.") { }
}
