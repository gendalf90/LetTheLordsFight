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

namespace StorageService.Controllers
{
    [Authorize]
    [Route("api/v1/storage")]
    class StorageController : Controller
    {
        private readonly ICommandFactory commandFactory;
        private readonly IQueryFactory queryFactory;
        
        public StorageController(ICommandFactory commandFactory, IQueryFactory queryFactory)
        {
            this.commandFactory = commandFactory;
            this.queryFactory = queryFactory;
        }

        [HttpPost]
        public async Task<IActionResult> AddStorageAsync()
        {
            var id = Guid.NewGuid().ToBase64String();
            var command = commandFactory.GetCreateStorageCommand(id);

            try
            {
                await command.ExecuteAsync();
            }
            catch
            {

            }

            return Created($"api/v1/storage/{id}", null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStorageAsync(string id)
        {
            //if(id == 1)
            //{
            //    return Json(new { Id = 1, Items = new Dictionary<string, int> { ["Wood"] = 1,
            //                                                                    ["Iron"] = 1} });
            //}

            var query = queryFactory.CreateStorageQuery(id);

            try
            {
                var result = await query.AskAsync();
                return Json(result);
            }
            catch
            {

            }

            return NotFound();
        }

        //[HttpDelete("{id}")]
        //public IActionResult RemoveStorage(int id)
        //{
        //    return Ok();
        //}

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
            catch
            {

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
            catch
            {
                
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
            catch
            {

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
            catch
            {

            }

            return Ok();
        }
    }
}
