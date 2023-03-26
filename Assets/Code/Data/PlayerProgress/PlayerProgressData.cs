namespace Code.Data.PlayerProgress
{
    public class PlayerProgressData
    {
        public CoinsData CoinsData { get; private set; }
        public StatsProgressData StatsProgressData { get; private set; }

        public PlayerProgressData(AllStats allStats)
        {
            CoinsData = new CoinsData();
            StatsProgressData = new StatsProgressData(allStats);
        }

        public void Reset(AllStats allStats)
        {
            CoinsData.Reset();
            StatsProgressData.Reset(allStats);
        }
    }
}