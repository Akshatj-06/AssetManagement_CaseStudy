using AssetManagementWebApplication.Dto;
using AssetManagementWebApplication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementWebApplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AssetAllocationController : ControllerBase
	{
		private readonly AssetManagementContext dbContext;
		public AssetAllocationController(AssetManagementContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllAssetAllocations()
		{
			await dbContext.SaveChangesAsync();
			return Ok(dbContext.AssetAllocations.ToList());
		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> AddAssetAllocation(AddAssetAllocationDto addAssetAllocationDto)
		{
			try
			{
				var newAssetAllocation = new AssetAllocation
				{
					AssetId = addAssetAllocationDto.AssetId,
					UserId = addAssetAllocationDto.UserId,
					AllocationDate = addAssetAllocationDto.AllocationDate,
					ReturnDate = addAssetAllocationDto.ReturnDate,
					AllocationStatus = addAssetAllocationDto.AllocationStatus,
				};

				dbContext.AssetAllocations.Add(newAssetAllocation);
				await dbContext.SaveChangesAsync();

				return Ok(new { message = "Asset allocation added successfully!", assetAllocation = newAssetAllocation });
			}
			catch (Exception ex)
			{
				// Return the exception message to the user
				return StatusCode(StatusCodes.Status500InternalServerError,
								  new { message = "An error occurred while adding the asset allocation.", error = ex.Message });
			}
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAssetAllocationById(int id, UpdateAssetAllocationDto updateAssetAllocationDto)
		{
			var assetAllocation = dbContext.AssetAllocations.Find(id);
			if (assetAllocation == null) { return NotFound(); }

			assetAllocation.AssetId = updateAssetAllocationDto.AssetId;
			assetAllocation.UserId = updateAssetAllocationDto.UserId;
			assetAllocation.AllocationDate = updateAssetAllocationDto.AllocationDate;
			assetAllocation.ReturnDate = updateAssetAllocationDto.ReturnDate;
			assetAllocation.AllocationStatus = updateAssetAllocationDto.AllocationStatus;


			dbContext.AssetAllocations.Update(assetAllocation);
			await dbContext.SaveChangesAsync();
			return Ok(assetAllocation);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAssetAllocationById(int id)
		{
			var assetAllocation = dbContext.AssetAllocations.Find(id);
			if (assetAllocation == null)
			{
				return NotFound();
			}
			dbContext.AssetAllocations.Remove(assetAllocation);
			await dbContext.SaveChangesAsync();
			return Ok(assetAllocation);
		}
	}
}
