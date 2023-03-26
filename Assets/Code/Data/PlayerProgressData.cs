namespace Code.Data
{
    public class PlayerProgressData
    {
        public CoinsData CoinsData { get; private set; }

        private readonly HeroDefaultData _defaultData;

        public PlayerProgressData(HeroDefaultData defaultData)
        {
            _defaultData = defaultData;
            CoinsData = new CoinsData();
        }

        public void Reset()
        {
            CoinsData.Reset();
        }
    }
}