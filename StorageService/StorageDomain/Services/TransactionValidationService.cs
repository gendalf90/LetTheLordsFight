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
        private readonly IMapRepository mapRepository;

        private SingleTransaction sourceTransaction;
        private Segment sourceSegment;
        private Segment destinationSegment;

        public TransactionValidationService(IUserService userService, IMapRepository mapRepository)
        {
            this.userService = userService;
            this.mapRepository = mapRepository;
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

            if(!SourceAndDestinationAtOneSegment)
            {
                throw new ValidationException();
            }
        }

        private async Task InitializeAsync(DualTransaction transaction)
        {
            sourceTransaction = transaction.Decrease;
            sourceSegment = await mapRepository.GetSegmentAsync(transaction.Decrease.StorageId);
            destinationSegment = await mapRepository.GetSegmentAsync(transaction.Increase.StorageId);
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

        private bool SourceAndDestinationAtOneSegment
        {
            get
            {
                return sourceSegment.Equals(destinationSegment);
            }
        }
    }
}
