namespace RedditMetrics.DataLayer.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RedditAPINameAttribute : Attribute
    {
        public string Name { get; private set; }

        internal RedditAPINameAttribute(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
