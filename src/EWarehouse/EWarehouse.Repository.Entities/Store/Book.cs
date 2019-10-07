namespace EWarehouse.Repository.Entities.Store
{
    public class Book
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Author { set; get; }
        public string Isbn { set; get; }
        public decimal Price { set; get; }
        public int Quantity { set; get; }
        public int LanguageId { set; get; }
        public Language Language { get; set; }
        public BookCover Cover { get; set; }
        public BookContent Content { get; set; }
    }
}
