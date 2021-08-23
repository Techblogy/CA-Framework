using CAF.Core.Entities;
using CAF.Core.ViewModel.User.Response;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

using System;

namespace CAF.MongoRepository.Core
{
    public class MongoMapping
    {
        public static void SetMapping()
        {

            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(BsonType.String));

            BsonClassMap.RegisterClassMap<UniqueEntity<Guid>>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(GuidGenerator.Instance);
            });

            //BsonClassMap.RegisterClassMap<UserUnReadMessageModel>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.MapIdMember(c => c.UserId).SetIdGenerator(GuidGenerator.Instance);
            //});

            if (!BsonClassMap.IsClassMapRegistered(typeof(LoginUserResponse)))
            {
                BsonClassMap.RegisterClassMap<LoginUserResponse>();
            }

        }

    }
}
