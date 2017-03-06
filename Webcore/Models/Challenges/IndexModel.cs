namespace Webcore.Models.Challenges
{
    using System.Collections.Generic;
    using System.Web;
    using Business.Services;
    using Domain.Entities;

    public class IndexModel : ILayout
    {
        public IndexModel()
        {
            Categories = new List<CategoryModel>();
        }

        public IList<CategoryModel> Categories { get; set; }

        public List<KeyValuePair<KeyValue, KeyValue>> MetaTags { get; set; }

        public CustomPrincipal UserPrincipal { get; set; }

        public Section Section { get; set; }

        public Content Content { get; set; }

        public List<Banner> Banners { get; set; }

        public string PageTitle { get; set; }

        public string Layout { get; set; }

        public HtmlString DeepFollower { get; set; }

        public Language CurrentLanguage { get; set; }

        public int IdeasCountAll { get; set; }
    }
}