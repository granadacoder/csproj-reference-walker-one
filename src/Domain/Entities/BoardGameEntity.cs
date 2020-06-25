namespace MyCompany.MyExamples.ProjectParser.Domain.Entities
{
    public class BoardGameEntity
    {
        ////[Key]
        public int BoardGameKey { get; set; }

        public string Title { get; set; }

        public string PublishingCompany { get; set; }

        public int MinPlayers { get; set; }

        public int MaxPlayers { get; set; }
    }
}