using StorageDomain.Repositories;
using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Services
{
    public interface ITransactionValidationService
    {
        Task ValidateAsync(SingleTransaction transaction);

        Task ValidateAsync(DualTransaction transaction);
    }
}
