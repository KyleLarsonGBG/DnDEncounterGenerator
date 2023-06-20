using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DnDEncounterGenerator.Data.Models.Entity
{
    public class Monster : IEntity
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
    }
}
