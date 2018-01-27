using System.Collections.Generic;
using FizzWare.NBuilder;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Core.ImportLists;
using NzbDrone.Core.Lifecycle;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test.ImportLists
{
    public class ImportListServiceFixture : DbTest<ImportListFactory, ImportListDefinition>
    {
        private List<IImportList> _importLists;

        [SetUp]
        public void Setup()
        {
            _importLists = new List<IImportList>();

            //_importLists.Add(Mocker.Resolve<Newznab>()); //TODO Uncomment when a we add lists
            //_importLists.Add(Mocker.Resolve<Omgwtfnzbs>());

            Mocker.SetConstant<IEnumerable<IImportList>>(_importLists);
        }

        [Test]
        public void should_remove_missing_import_lists_on_startup()
        {
            var repo = Mocker.Resolve<ImportListRepository>();

            Mocker.SetConstant<IImportListRepository>(repo);

            var existingImportLists = Builder<ImportListDefinition>.CreateNew().BuildNew();
            //existingImportLists.ConfigContract = typeof (NewznabSettings).Name; //TODO Uncomment when a we add lists

            repo.Insert(existingImportLists);

            Subject.Handle(new ApplicationStartedEvent());

            AllStoredModels.Should().NotContain(c => c.Id == existingImportLists.Id);
        }
    }
}