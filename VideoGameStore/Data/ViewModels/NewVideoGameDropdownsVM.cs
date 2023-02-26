using VideoGameStore.Models;

namespace VideoGameStore.Data.ViewModels
{
    public class NewVideoGameDropdownsVM
    {
        public List<Developer> Developers { get; set; }
        public List<Publisher> Publishers { get; set; }

        public NewVideoGameDropdownsVM()
        {
            Developers = new List<Developer>();
            Publishers = new List<Publisher>();
        }

        
    }
}
