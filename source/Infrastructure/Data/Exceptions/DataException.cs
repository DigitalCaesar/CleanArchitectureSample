using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Exceptions;
public abstract class DataException : Exception
{
    protected DataException(string message) : base(message) { }
}
