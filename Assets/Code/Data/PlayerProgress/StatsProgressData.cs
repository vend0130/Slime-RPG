namespace Code.Data.PlayerProgress
{
    public class StatsProgressData
    {
        public StatProgressData AttackData { get; private set; }

        public StatsProgressData(AllStats allStats) =>
            AttackData = new StatProgressData(allStats.Attack);
    }
}