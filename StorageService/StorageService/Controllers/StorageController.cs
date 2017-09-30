using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StorageService.Common;
using StorageService.Commands;
using StorageDomain.ValueObjects;
using StorageService.Queries;
using Microsoft.AspNetCore.Authorization;
using StorageService.Extensions;
using StorageDomain.Exceptions;

namespace StorageService.Controllers
{
    [Authorize]
    [Route("api/v1/storage")]
    public class StorageController : Controller
    {
        private readonly ICommandFactory commandFactory;
        private readonly IQueryFactory queryFactory;
        
        public StorageController(ICommandFactory commandFactory, IQueryFactory queryFactory)
        {
            this.commandFactory = commandFactory;
            this.queryFactory = queryFactory;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddStorageAsync(string id)
        {
            var command = commandFactory.GetCreateStorageCommand(id);

            try
            {
                await command.ExecuteAsync();
            }
            catch (NotAuthorizedException)
            {
                return Forbid();
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStorageAsync(string id)
        {
            var query = queryFactory.CreateStorageQuery(id);

            try
            {
                return Ok(await query.AskAsync());
            }
            catch (NotAuthorizedException)
            {
                return Forbid();
            }
            catch(InvalidOperationException e) when (e.Message == "Sequence contains no elements")
            {
                return NotFound();
            }
        }

        [HttpPost("{storageId}/item/{itemName}/quantity/{itemCount}/increase")]
        public async Task<IActionResult> IncreaseItemAsync(SingleTransactionData transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var command = commandFactory.GetSingleTransactionCommand(SingleTransactionType.Increase, transaction);
            
            try
            {
                await command.ExecuteAsync();
            }
            catch(NotAuthorizedException)
            {
                return Forbid();
            }
            catch (ValidationException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("{storageId}/item/{itemName}/quantity/{itemCount}/decrease")]
        public async Task<IActionResult> DecreaseItemAsync(SingleTransactionData transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var command = commandFactory.GetSingleTransactionCommand(SingleTransactionType.Decrease, transaction);

            try
            {
                await command.ExecuteAsync();
            }
            catch (NotAuthorizedException)
            {
                return Forbid();
            }
            catch (ValidationException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("{fromStorageId}/item/{itemName}/quantity/{itemCount}/to/{toStorageId}")]
        public async Task<IActionResult> MoveItemAsync(DualTransactionData transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var command = commandFactory.GetDualTransactionCommand(transaction);

            try
            {
                await command.ExecuteAsync();
            }
            catch (NotAuthorizedException)
            {
                return Forbid();
            }
            catch (ValidationException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("snapshots")]
        public async Task<IActionResult> CreateSnapshotsAsync()
        {
            var createSnapshotCommand = commandFactory.GetCreateSnapshotCommand();

            try
            {
                await createSnapshotCommand.ExecuteAsync();
            }
            catch (NotAuthorizedException)
            {
                return Forbid();
            }

            return Ok();
        }
    }
}
