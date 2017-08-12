using StorageDomain.ValueObjects;
using StorageService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Commands
{
    interface ICommandFactory
    {
        ICommand GetSingleTransactionCommand(SingleTransactionType type, SingleTransactionData data);

        ICommand GetDualTransactionCommand(DualTransactionData data);

        ICommand GetCreateSnapshotCommand();

        ICommand GetCreateStorageCommand(string newStorageId);
    }
}
