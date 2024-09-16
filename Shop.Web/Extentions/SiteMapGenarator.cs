using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shop.Web.Extentions
{
    public class SiteMapGenarator
    {
        public class SitemapNode
        {
            public string Loc { get; set; }
            public double? Priority { get; set; }
            public DateTime? LastModified { get; set; }
            public SiteMapFrequency Frequency { get; set; }
        }
        public enum SiteMapFrequency
        {
            Never,
            Yearly,
            Monthly,
            weekly,
            Daily,
            Hourly,
            Always


        }
        public void CreateSitemapXml(IEnumerable<SitemapNode>sitemaoNodes,string directoryPath)
        {
            XElement root = new XElement("urlset");

            foreach (var item in sitemaoNodes)
            {
                XElement urlElement = new XElement(
                    "url",new XElement("loc",Uri.EscapeUriString(item.Loc)),

                    item.LastModified==null ? null:new XElement("lastmodifies",item.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    item.Frequency== null ? null : new XElement("changefreq",item.Frequency.ToString().ToLowerInvariant()),
                    item.Priority == null ? null : new XElement("priority",item.Priority.Value.ToString("F1"))


                    );
          

                root.Add(urlElement);
            }
            XDocument doc = new XDocument(root);
            doc.Save(Path.Combine(directoryPath, "sitemap.xml"));
        }
    }
}
