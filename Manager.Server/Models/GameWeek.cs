namespace Manager.Server.Models
{
    public class GameWeek
    {
        public DateTime Checked { get; set; } = DateTime.Now;
        public int Id { get; set; } = 1;
        public bool IsActive { get; set; } = false;
        public bool CurrentGameWeek { get; set; } = true;
        public bool NextGameWeek { get; set; } = false;
        public bool PreviousGameWeek { get; set; } = false;
    }
}
