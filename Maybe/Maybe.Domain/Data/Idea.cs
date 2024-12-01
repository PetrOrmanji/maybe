using EnsureThat;

namespace Maybe.Domain.Data
{
    [Serializable]
    public class Idea
    {
        public Idea()
        {

        }

        public Idea(string name, string description, string question)
        {
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
            EnsureArg.IsNotNullOrWhiteSpace(description, nameof(description));

            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Question = question;
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public DateTime? Publicated { get; set; }

        public override string ToString()
        {
            return
                $"{Name}" + Environment.NewLine + Environment.NewLine +
                $"{Description}" + Environment.NewLine + Environment.NewLine +
                $"{Question}";
        }
    }
}
