using System.Collections.Generic;

namespace EWarehouse.Repository.Entities.Store
{
    public class Language
    {
        public int Id { set; get; }
        public string ShortName { set; get; }
        public string FullName { set; get; }

        public List<Book> Books { get; set; }

        public Language()
        {
            Books = new List<Book>();
        }
    }
}
