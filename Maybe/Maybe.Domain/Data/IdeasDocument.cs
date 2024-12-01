namespace Maybe.Domain.Data
{
    [Serializable]
    public class IdeasDocument
    {
        public List<Idea> Ideas { get; set; } = new List<Idea>();
    }
}
