
namespace NoPersa.Tests.Misc
{
    public interface ITest
    {
        [TestInitialize]
        public void Setup();

        public void SeedTestData();

        [TestCleanup]
        public void Cleanup();
    }
}
