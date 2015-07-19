using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class Level : ModelBase, ILanguage
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "levelId")]
        public int LevelId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }

        public static Level CreateAnyOption()
        {
            var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var localizedText = resourceLoader.GetString("any_level");

            var level = new Level()
            {
                Name = localizedText
            };

            return level;
        }
    }
}
