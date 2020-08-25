﻿using FluentAssertions;
using LiteDB.Realtime.Subscriptions;
using System;
using Xunit;

namespace LiteDB.Realtime.Test.Subscriptions
{
    public class DocumentSubscriptionBuilder_Should : SubscriptionBuilderTestBase
    {
        private class Model
        { }

        [Fact]
        public void Build_A_Document_Subscription()
        {
            var collectionName = "testCollection";
            var id = new BsonValue(Guid.NewGuid());
            new SubscriptionBuilder(_db.NotificationService)
                .Collection<Model>(null)
                .Id(id)
                .As<ISubscriptionBuilderBase>()
                .Subscription
                .Collection
                .Should()
                .BeNull();

            new SubscriptionBuilder(_db.NotificationService)
                .Collection<Model>(collectionName)
                .Id(id)
                .As<ISubscriptionBuilderBase>()
                .Subscription
                .Collection
                .Should()
                .Be(collectionName);

            var collSub = new CollectionSubscription<Model>(_db.NotificationService)
            {
                Collection = collectionName,
            };

            var collBuilder = new CollectionSubscriptionBuilder<Model>(_db.NotificationService, collSub);
            var docBuilder = collBuilder.Id(id) as DocumentSubscriptionBuilder<Model>;

            var docSub = (docBuilder as ISubscriptionBuilderBase).Subscription as DocumentSubscription<Model>;
            docSub.Collection.Should().Be(collectionName);
            docSub.Id.Should().Be(id);
            // before subscribing
            docSub.Observer.Should().BeNull();

            docBuilder.Subscribe(obj => { });

            // after subscribing
            docSub.Observer.Should().NotBeNull();
        }

    }
}