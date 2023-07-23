using AutoMapper;
using DemoWebAPIEF6HienLTH.Entities;
using DemoWebAPIEF6HienLTH.ViewModel;

namespace DemoWebAPIEF6HienLTH.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Category, CategoryModel>().ReverseMap();
        }
    }
}
