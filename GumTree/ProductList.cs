using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GumTree
{
    public class ProductList
    {
        public string postad_Id_string { get; set; }
        public string edit_url { get; set; }
        public string edit_time { get; set; }
        public string username { get; set; }

        public string contactname { get; set; }
        public string password { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string postcode { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string locationId { get; set; }
        public string Description { get; set; }
        public string phonenumber { get; set; }
        public string Images { get; set; }
        public string mainImage { get; set; }
        public bool sellertype { get; set; }
        public string Price { get; set; }
        public List<string> imageIds = new List<string>();
        public string makePostJson()
        {
            string post_json_string = "";

            return post_json_string;
        }
        public void clear()
        {
            username = "";
            contactname = "";
            password = "";
            Id = "";
            postcode = "";
            Type = "";
            Title = "";
            Category = "";
            Description = "";
            Images = "";
            Price = "";
        }
    }
}
