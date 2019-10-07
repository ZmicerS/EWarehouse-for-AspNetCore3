namespace EWarehouse.Web.Models.Store
{
    public class BookCoverViewModel
    {
        public string NameOfFile { set; get; }
        public byte[] BodyOfFile = new byte[56000];
        public int LengthOfFile { set; get; }
        public string TypeOfFile { set; get; }
    }
}
