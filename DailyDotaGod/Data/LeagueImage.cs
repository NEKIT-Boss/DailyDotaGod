namespace DailyDotaGod.Data
{
    public class LeagueImage
    {
        public int Id { get; set; }

        public virtual League League { get; set; }
        public byte[] Data { get; set; }
    }
}