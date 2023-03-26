namespace Code.Data.PlayerProgress
{
    public class StatProgressData
    {
        public string Name { get; private set; }
        public int Level;
        public float Number;
        public int Price;

        public StatProgressData(StatData statData)
        {
            Name = statData.Name;
            Level = 1;
            Number = statData.DefaultNumber;
            Price = statData.DefaultPrice;
        }
    }
}