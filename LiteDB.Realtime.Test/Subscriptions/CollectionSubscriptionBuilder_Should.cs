﻿using FluentAssertions;
using LiteDB.Realtime.Subscriptions;
using System;
using Xunit;
using Xunit.Sdk;

namespace LiteDB.Realtime.Test.Subscriptions
{
    public class CollectionSubscriptionBuilder_Should : SubscriptionBuilderTestBase
    {
        private class Model
        { }

        public CollectionSubscriptionBuilder_Should()
            : base()
        {
        }

        [Fact]
        public void Build_A_Collection_Subscription()
        {
            var collectionName = "testCollection";
            new SubscriptionBuilder(_db.NotificationService)
                .Collection<Model>(null)
                .Subscription
                .Collection
                .Should()
                .BeNull();

            new SubscriptionBuilder(_db.NotificationService)
                .Collection<Model>(collectionName)
                .Subscription
                .Collection
                .Should()
                .Be(collectionName);

            var sub = new SubscriptionBuilder(_db.NotificationService)
                .Collection<Model>(collectionName)
                .Subscription;
            sub.Collection.Should().Be(collectionName);

            var castedSub = sub.As<CollectionSubscription<Model>>();
            castedSub.Collection.Should().Be(collectionName);

            var builder = new CollectionSubscriptionBuilder<Model>(_db.NotificationService, castedSub);
            // before subscribing
            castedSub.Observer.Should().BeNull();

            builder.Subscribe(listObj => { });

            // after subscribing
            castedSub.Observer.Should().NotBeNull();
        }

    }
}