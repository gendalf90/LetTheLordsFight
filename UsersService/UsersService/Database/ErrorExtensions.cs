using MySql.Data.MySqlClient;
using System;

namespace UsersService.Database
{
    static class ErrorExtensions
    {
        private const int MySqlDublicateEntryErrorNumber = 1062;

        public static void ThrowIfDublicateEntry(this Exception current, Exception toThrow)
        {
            var inner = current.InnerException as MySqlException;

            if (inner?.Number == MySqlDublicateEntryErrorNumber)
            {
                throw toThrow;
            }
        }
    }
}
