namespace EWarehouse.Services.Entities.StoreModels
{
    public class BookCoverServiceModel
    {
        public string NameOfFile { set; get; }
        public byte[] BodyOfFile = new byte[56000];
        public int LengthOfFile { set; get; }
        public string TypeOfFile { set; get; }
    }
}
