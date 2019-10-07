namespace EWarehouse.Web.Models.Store
{
    public class BookCoreVieweModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Author { set; get; }

        public string Isbn { set; get; }

        public decimal Price { set; get; }

        public int Quantity { set; get; }

        public int LanguageId { set; get; }

        public string Content { set; get; }
    }
}
