using MySql.Data.MySqlClient;
using System;

namespace UsersService.Database
{
    static class ErrorExtensions
    {
        private const int MySqlDublicateEntryErrorNumber = 1062;

        public static void ThrowIfDublicateEntry(this Exception current, Exception toThrow)
        {
            if(current.InnerException is MySqlException inner && inner.Number is MySqlDublicateEntryErrorNumber)
            {
                throw toThrow;
            }
        }
    }
}
