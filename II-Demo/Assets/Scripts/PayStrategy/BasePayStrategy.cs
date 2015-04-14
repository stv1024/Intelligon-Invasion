namespace Assets.Scripts.PayStrategy
{
    public abstract class BasePayStrategy
    {
        public abstract void Pay(UserSelectedTrade trade);
    }
    public class UserSelectedTrade
    {
        /// <summary>
        /// "revive"表示复活。
        /// </summary>
        public string Name;//要能查询出充值目的
        public string DisplayName;
        public float Price;

        public UserSelectedTrade(string name, string displayName, float price)
        {
            Name = name;
            DisplayName = displayName;
            Price = price;
        }
    }
}