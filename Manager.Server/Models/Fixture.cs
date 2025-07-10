namespace Manager.Server.Models
{
    public class Fixture
    {
        public int Code { get; set; }
        public int Id { get; set; }
        public int Team_H { get; set; }
        public int Team_A { get; set; }
        public bool Finished { get; set; }
        public DateTime? Kickoff_Time { get; set; }

        public override string ToString()
        {
            return $"Team_a: {Team_A}, Team_H: {Team_H}, Finished: {Finished}";
        }
    }
}
