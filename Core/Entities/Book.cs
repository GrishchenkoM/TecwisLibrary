namespace Core.Entities
{
    public class Book : EntityBase
    {
        public int AuthorId { get; set; }
        public int Year { get; set; }
    }
}
