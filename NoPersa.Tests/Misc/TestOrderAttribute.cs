namespace NoPersa.Tests.Misc
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestOrderAttribute : Attribute
    {
        public int Order { get; }

        public TestOrderAttribute(int order)
        {
            Order = order;
        }
    }

}
