using AssinaturaDigital.Models;
using FluentAssertions;
using NUnit.Framework;
using Prism.Commands;
using System.Collections.Generic;
using System.Linq;

namespace AssinaturaDigital.UnitTests.Models
{
    public class NotifyCommandCollectionModelTests
    {
        [Test]
        public void WhenCreatingModelShouldSetCommandToNotifyToEachItem()
        {
            var expectedCommandToNotify = new DelegateCommand(() => { });

            var expectedItems = new List<NotifyCommandModelFake>
            {
                new NotifyCommandModelFake(),
                new NotifyCommandModelFake(),
                new NotifyCommandModelFake()
            };

            var notifyCommandCollectionModel = new NotifyCommandCollectionModel<NotifyCommandModelFake>(expectedCommandToNotify,
                expectedItems);

            var items = notifyCommandCollectionModel.Items;

            items.Should().BeEquivalentTo(expectedItems);
            items.Any(x => x.CommandToNotify != expectedCommandToNotify).Should().BeFalse();
        }
    }

    public class NotifyCommandModelFake : NotifyCommandModel
    {
        public DelegateCommand CommandToNotify => _commandToNotify;
    }
}
