using StorageDomain.Entities;
using StorageDomain.Repositories;
using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Services
{
    public class TransactionValidationService : ITransactionValidationService
    {
        private readonly IUsersRepository userRepository;
        private readonly IMapRepository mapRepository;

        private SingleTransaction sourceTransaction;
        private UserEntity currentUser;
        private Coordinate sourceCoordinate;
        private Coordinate destinationCoordinate;

        public TransactionValidationService(IUsersRepository userRepository, IMapRepository mapRepository)
        {
            this.userRepository = userRepository;
            this.mapRepository = mapRepository;
        }

        public async Task ValidateAsync(SingleTransaction transaction)
        {
            await InitializeAsync(transaction);

            if((!IsCurrentUserSourceStorageOwner || SourceTransactionIsIncrease) && !IsCurrentUserSystemOrAdmin)
            {
                throw new Exception();
            }
        }

        private async Task InitializeAsync(SingleTransaction transaction)
        {
            currentUser = await userRepository.GetCurrentAsync();
            sourceTransaction = transaction;
        }

        public async Task ValidateAsync(DualTransaction transaction)
        {
            await InitializeAsync(transaction);

            if((!IsCurrentUserSourceStorageOwner || !IsSourceStorageAndDestinationStorageAtOnePoint) && !IsCurrentUserSystemOrAdmin)
            {
                throw new Exception();
            }
        }

        private async Task InitializeAsync(DualTransaction transaction)
        {
            currentUser = await userRepository.GetCurrentAsync();
            sourceTransaction = transaction.Decrease;
            sourceCoordinate = await mapRepository.GetStorageCoordinateAsync(transaction.Decrease.StorageId);
            destinationCoordinate = await mapRepository.GetStorageCoordinateAsync(transaction.Increase.StorageId);
        }

        private bool IsCurrentUserSourceStorageOwner
        {
            get
            {
                return currentUser.IsOwnerOf(sourceTransaction.StorageId);
            }
        }

        private bool IsCurrentUserSystemOrAdmin
        {
            get
            {
                return currentUser.IsAdminOrSystem;
            }
        }

        private bool IsSourceStorageAndDestinationStorageAtOnePoint
        {
            get
            {
                return sourceCoordinate.Equals(destinationCoordinate);
            }
        }

        private bool SourceTransactionIsIncrease
        {
            get
            {
                return sourceTransaction.Type == SingleTransactionType.Increase;
            }
        }
    }
}
