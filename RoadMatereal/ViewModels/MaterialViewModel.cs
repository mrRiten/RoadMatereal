using RoadMatereal.Models;

namespace RoadMatereal.ViewModels
{
    public class MaterialViewModel
    {
        public IEnumerable<Material>? Materials { get; set; }

        public IEnumerable<Supplier>? Suppliers { get; set; }

        public int? SelectedSupplierId { get; set; }
    }
}
