namespace CappuChat
{
    public class SimpleGroup
    {
        public string UniqueGroupName { get; set; }
        public string Name { get; set; }

        public SimpleGroup(string uniqueGroupName, string name)
        {
            UniqueGroupName = uniqueGroupName;
            Name = name;
        }
    }
}
