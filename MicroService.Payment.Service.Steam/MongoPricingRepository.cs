using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroService.Common.Core.Databases.Repository;
using MicroService.Payment.Service.Steam.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroService.Payment.Service.Steam
{
    public class MongoPricingRepository : IMongoPricingRepository
    {
        private readonly IRepository<SteamPricing, SteamGames> _repository;

        public MongoPricingRepository(IRepository<SteamPricing, SteamGames> repository)
        {
            _repository = repository;
        }

        public async Task InsertOrUpdateAsync(SteamPricing steamPricing)
        {
            await _repository.SaveAsync(steamPricing);
        }

        public async Task<SteamPricing> GetAllAsync(SteamGames steamGames)
        {
            return await _repository.GetAsync(steamGames);
        }
    }

    public class SteamPricing : IEntity<SteamGames>
    {
        [BsonId]
        public SteamGames Id { get; set; }

        public DateTimeOffset       LastUpdated { get; set; }
        public List<SteamItemPrice> Items       { get; set; }
    }

    public class SteamItemPrice
    {
        public decimal        Value           { get; set; }
        public DateTimeOffset LastUpdated     { get; set; }
        public int?           ContextId       { get; set; }
        public string         IconUrl         { get; set; }
        public string         MarketHashName  { get; set; }
        public string         NameColor       { get; set; }
        public string         BackgroundColor { get; set; }
    }

    public class SteamItem
    {
        public decimal        Value           { get; set; }
        public DateTimeOffset LastUpdated     { get; set; }
        public int            ContextId       { get; set; }
        public string         AssetId         { get; set; }
        public int            ClassId         { get; set; }
        public int            InstanceId      { get; set; }
        public string         IconUrl         { get; set; }
        public string         MarketHashName  { get; set; }
        public string         NameColor       { get; set; }
        public string         BackgroundColor { get; set; }
    }

    public interface IMongoPricingRepository
    {
        Task               InsertOrUpdateAsync(SteamPricing steamPricing);
        Task<SteamPricing> GetAllAsync(SteamGames steamGames);
    }
}