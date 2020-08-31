using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RMStore.Domain
{
    //https://stackoverflow.com/questions/1386962/how-to-throw-a-sqlexception-when-needed-for-mocking-and-unit-testing
    public class SqlExceptionCreator
    {
        private static T Construct<T>(params object[] p)
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)ctors.First(ctor => ctor.GetParameters().Length == p.Length).Invoke(p);
        }

        internal static SqlException NewSqlException(int number = 1)
        {
            SqlErrorCollection collection = Construct<SqlErrorCollection>();
            SqlError error = null;
            if (number == 2)
            {
                error = Construct<SqlError>(number, (byte)2, (byte)3, "rm-server", "cannot open database", "database", 100, null);
            }
            else
            {
                error = Construct<SqlError>(number, (byte)2, (byte)3, "rm-server", "Could not find stored procedure", "proc", 100, null);
            }
 
            typeof(SqlErrorCollection)
                .GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new object[] { error });


            return typeof(SqlException)
                .GetMethod("CreateException", BindingFlags.NonPublic | BindingFlags.Static,
                    null,
                    CallingConventions.ExplicitThis,
                    new[] { typeof(SqlErrorCollection), typeof(string) },
                    new ParameterModifier[] { })
                .Invoke(null, new object[] { collection, "7.0.0" }) as SqlException;
        }
    }
}
