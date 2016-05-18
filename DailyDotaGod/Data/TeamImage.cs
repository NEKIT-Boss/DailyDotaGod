namespace DailyDotaGod.Data
{
    public class TeamImage
    {
        public int Id { get; set; }

        public virtual Team Team { get; set; }
        public byte[] Data { get; set; }
    }
}