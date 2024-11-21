using AssetManagementWebApplication.Dto;
using AssetManagementWebApplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementWebApplication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AssetController : ControllerBase
	{
		private readonly AssetManagementContext dbContext;
		public AssetController(AssetManagementContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllAssets()
		{
			await dbContext.SaveChangesAsync();
			return Ok(dbContext.Assets.ToList());
		}
		[HttpPost]
		public async Task<IActionResult> AddAsset(AddAssetDto addAssetDto)
		{
			var newAsset = new Asset
			{
				AssetName = addAssetDto.AssetName,
				AssetCategory = addAssetDto.AssetCategory,
				AssetModel = addAssetDto.AssetModel,
				ManufacturingDate = addAssetDto.ManufacturingDate,
				ExpiryDate = addAssetDto.ExpiryDate,
				AssetValue = addAssetDto.AssetValue,
				CurrentStatus = addAssetDto.CurrentStatus
			};
			dbContext.Assets.Add(newAsset);
			await dbContext.SaveChangesAsync();



			return Ok(newAsset);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetAssetById(int id)
		{
			var asset = dbContext.Assets.Find(id);
			if (asset == null)
			{
				return NotFound();
			}
			await dbContext.SaveChangesAsync();
			return Ok(asset);
		}
		[HttpPut]
		public async Task<IActionResult> UpdateAssetById(int id, UpdateAssetDto updateAssetDto)
		{
			var asset = dbContext.Assets.Find(id);
			if (asset == null) { return NotFound(); }

			asset.AssetName = updateAssetDto.AssetName;
			asset.AssetCategory = updateAssetDto.AssetCategory;
			asset.AssetModel = updateAssetDto.AssetModel;
			asset.ManufacturingDate = updateAssetDto.ManufacturingDate;
			asset.ExpiryDate = updateAssetDto.ExpiryDate;
			asset.AssetValue = updateAssetDto.AssetValue;
			asset.CurrentStatus = updateAssetDto.CurrentStatus;


			dbContext.Assets.Update(asset);
			await dbContext.SaveChangesAsync();
			dbContext.SaveChanges();
			return Ok(asset);
		}
		[HttpDelete]
		public async Task<IActionResult> DeleteAssetById(int id)
		{
			var asset = dbContext.Assets.Find(id);
			if (asset == null)
			{
				return NotFound();
			}
			dbContext.Assets.Remove(asset);
			await dbContext.SaveChangesAsync();
			dbContext.SaveChanges();
			return Ok(asset);
		}

	}
}
