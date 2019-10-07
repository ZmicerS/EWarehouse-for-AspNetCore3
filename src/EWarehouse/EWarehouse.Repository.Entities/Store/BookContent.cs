namespace EWarehouse.Repository.Entities.Store
{
    public class BookContent
    {
        public int Id { set; get; }
        public string Content { set; get; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
