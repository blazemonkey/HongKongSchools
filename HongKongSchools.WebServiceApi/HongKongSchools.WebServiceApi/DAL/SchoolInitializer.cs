using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.JsonService;
using System.Web.Hosting;

namespace HongKongSchools.WebServiceApi.DAL
{
    // DropCreateDatabaseIfModelChanges
    public class SchoolInitializer : DropCreateDatabaseAlways<SchoolContext>
    {
        private readonly IJsonService _json;

        public SchoolInitializer()
        {
            _json = new JsonService();
        }

        protected override void Seed(SchoolContext context)
        {
            SeedAddressesData(context);
            SeedDistrictsData(context);
            SeedFinanceTypesData(context);
            SeedGendersData(context);
            SeedLevelsData(context);
            SeedNamesData(context);
            SeedSchoolsData(context);
            SeedSessionsData(context);
            SeedSchoolSessionsData(context);
        }

        private void SeedAddressesData(SchoolContext context)
        {
            var json = GetJson("addresses.json");
            var addresses = _json.Deserialize<IEnumerable<Address>>(json);
            context.Addresses.AddRange(addresses);
        }

        private void SeedDistrictsData(SchoolContext context)
        {
            var json = GetJson("districts.json");
            var districts = _json.Deserialize<IEnumerable<District>>(json);
            context.Districts.AddRange(districts);
        }

        private void SeedFinanceTypesData(SchoolContext context)
        {
            var json = GetJson("finance_types.json");
            var financeTypes = _json.Deserialize<IEnumerable<FinanceType>>(json);            
            context.FinanceTypes.AddRange(financeTypes);
        }

        private void SeedGendersData(SchoolContext context)
        {
            var json = GetJson("genders.json");
            var genders = _json.Deserialize<IEnumerable<Gender>>(json);
            context.Genders.AddRange(genders);
        }

        private void SeedLevelsData(SchoolContext context)
        {
            var json = GetJson("levels.json");
            var levels = _json.Deserialize<IEnumerable<Level>>(json);
            context.Levels.AddRange(levels);
        }

        private void SeedNamesData(SchoolContext context)
        {
            var json = GetJson("names.json");
            var names = _json.Deserialize<IEnumerable<SchoolName>>(json);
            context.Names.AddRange(names);
        }

        private void SeedSchoolsData(SchoolContext context)
        {
            var json = GetJson("schools.json");
            var schools = _json.Deserialize<IEnumerable<School>>(json);
            context.Schools.AddRange(schools);
        }

        private void SeedSchoolSessionsData(SchoolContext context)
        {
            var json = GetJson("school_sessions.json");
            var schoolSessions = _json.Deserialize<IEnumerable<SchoolSession>>(json);
            context.SchoolSessions.AddRange(schoolSessions);
        }

        private void SeedSessionsData(SchoolContext context)
        {
            var json = GetJson("sessions.json");
            var sessions = _json.Deserialize<IEnumerable<Session>>(json);
            context.Sessions.AddRange(sessions);
        }

        private static string GetJson(string fileName)
        {
            var file = HostingEnvironment.MapPath(string.Format("{0}{1}", @"~/App_Data/SeedData/", fileName));
            if (file == null)
                return string.Empty;

            var json = File.ReadAllText(file);
            return json;
        }
    }
}