using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class District : ModelBase, ILanguage
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "districtId")]
        public int DistrictId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }

        public static District CreateAnyOption()
        {
            var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var localizedText = resourceLoader.GetString("any_district");

            var district = new District()
            {
                Name = localizedText
            };

            return district;
        }
    }
}
