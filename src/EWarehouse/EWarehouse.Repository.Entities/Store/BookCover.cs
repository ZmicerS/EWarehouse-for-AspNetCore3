namespace EWarehouse.Repository.Entities.Store
{
    public class BookCover
    {
        public int Id { set; get; }
        public string NameOfFile { set; get; }
        public int LengthOfFile { set; get; }
        public string TypeOfFile { set; get; }
        public byte[] BodyOfFile { set; get; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
