namespace GameFinder_WebAPI.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Production { get; set; }

        public string Date { get; set; }

        public float Vote { get; set; }
    }
}
