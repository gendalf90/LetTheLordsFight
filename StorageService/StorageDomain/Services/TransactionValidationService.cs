using StorageDomain.Entities;
using StorageDomain.Exceptions;
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
        private readonly IUserService userService;
        private readonly IDistanceService distanceService;

        private SingleTransaction sourceTransaction;

        public TransactionValidationService(IUserService userService, IDistanceService distanceService)
        {
            this.userService = userService;
            this.distanceService = distanceService;
        }

        public Task ValidateAsync(SingleTransaction transaction)
        {
            Initialize(transaction);

            if(SourceTransactionIsIncrease && IsCurrentUserNotSystem)
            {
                throw new NotAuthorizedException();
            }

            if(SourceTransactionIsDecrease && IsCurrentUserNotSourceStorageOwnerOrSystem)
            {
                throw new NotAuthorizedException();
            }

            return Task.CompletedTask;
        }

        private void Initialize(SingleTransaction transaction)
        {
            sourceTransaction = transaction;
        }

        public async Task ValidateAsync(DualTransaction transaction)
        {
            await InitializeAsync(transaction);

            if(IsCurrentUserNotSourceStorageOwnerOrSystem)
            {
                throw new NotAuthorizedException();
            }

            if(!AreSourceAndDestinationStoragesAtOnePoint)
            {
                throw new ValidationException();
            }
        }

        private async Task InitializeAsync(DualTransaction transaction)
        {
            sourceTransaction = transaction.Decrease;
            AreSourceAndDestinationStoragesAtOnePoint = await distanceService.IsDistanceBeetwenStoragesSufficientForTransactionAsync(transaction.Decrease.StorageId, transaction.Increase.StorageId);
        }

        private bool IsCurrentUserNotSourceStorageOwnerOrSystem
        {
            get
            {
                return !userService.IsCurrentUserOwnerOfThisStorageOrSystem(sourceTransaction.StorageId);
            }
        }

        private bool IsCurrentUserNotSystem
        {
            get
            {
                return !userService.IsCurrentUserSystem();
            }
        }

        private bool SourceTransactionIsIncrease
        {
            get
            {
                return sourceTransaction.Type == SingleTransactionType.Increase;
            }
        }

        private bool SourceTransactionIsDecrease
        {
            get
            {
                return sourceTransaction.Type == SingleTransactionType.Decrease;
            }
        }

        private bool AreSourceAndDestinationStoragesAtOnePoint { get; set; }
    }
}
