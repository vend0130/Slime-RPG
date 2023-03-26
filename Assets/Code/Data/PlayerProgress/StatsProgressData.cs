namespace Code.Data.PlayerProgress
{
    public class StatsProgressData
    {
        public StatProgressData AttackData { get; private set; }
        public StatProgressData HPData { get; private set; }
        public StatProgressData ASPDData { get; private set; }
        public StatProgressData CriticalChanceData { get; private set; }
        public StatProgressData CriticalHitDamageData { get; private set; }

        public StatsProgressData(AllStats allStats)
        {
            AttackData = new StatProgressData(allStats.Attack);
            HPData = new StatProgressData(allStats.HP);
            ASPDData = new StatProgressData(allStats.ASPD);
            CriticalChanceData = new StatProgressData(allStats.CriticalChance);
            CriticalHitDamageData = new StatProgressData(allStats.CriticalHitDamage);
        }

        public void Reset(AllStats allStats)
        {
            AttackData.Reset(allStats.Attack);
            HPData.Reset(allStats.HP);
            ASPDData.Reset(allStats.ASPD);
            CriticalChanceData.Reset(allStats.CriticalChance);
            CriticalHitDamageData.Reset(allStats.CriticalHitDamage);
        }
    }
}