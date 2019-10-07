namespace EWarehouse.Services.Entities.StoreModels
{
    public class BookServiceModel : BookCoreServiceModel
    {
        public BookCoverServiceModel imageOfCover { set; get; }
    }
}
