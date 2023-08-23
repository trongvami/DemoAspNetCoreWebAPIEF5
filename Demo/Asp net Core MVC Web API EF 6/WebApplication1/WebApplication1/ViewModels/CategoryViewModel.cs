namespace WebApplication1.ViewModels
{
    public class CategoryViewModel
    {
        public PagedList.Core.PagedList<CategoriesListViewModel> pagedCategories;
        public PagedList.Core.PagedList<LevelsListViewModel> pagedLevels;
        public PagedList.Core.PagedList<ParentsListViewModel> pagedParents;
    }
}
