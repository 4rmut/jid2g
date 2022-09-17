using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace jid2g
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            List<Countries> countries = null;
            List<States> states = null;
            List<Cities> cities = null;
            List<Countries> newCountries = new List<Countries>();
            List<States> newStates = new List<States>();
            List<Cities> newCities = new List<Cities>();
            string path = Directory.GetCurrentDirectory() + "/datas/";

            using (StreamReader r = new StreamReader(path + "countries.json"))
            {
                string json = r.ReadToEnd();
                countries = JArray.Parse(json).ToObject<List<Countries>>();
                Console.WriteLine("Ülkeler json dosyasından okundu");

            }
            using (StreamReader r = new StreamReader(path + "states.json"))
            {
                string json = r.ReadToEnd();
                states = JArray.Parse(json).ToObject<List<States>>();
                Console.WriteLine("İller json dosyasından okundu");
            }
            using (StreamReader r = new StreamReader(path + "cities.json"))
            {
                string json = r.ReadToEnd();
                cities = JArray.Parse(json).ToObject<List<Cities>>();
                Console.WriteLine("İlçeler json dosyasından okundu");
            }

            foreach (var country in countries)
            {
                Guid country_id = Guid.NewGuid();
                country.gid = country_id;
                foreach (var state in states.Where(x => x.country_id == country.id))
                {
                    Guid state_id = Guid.NewGuid();
                    state.gid = state_id;
                    state.country_gid = country_id;
                    foreach (var city in cities.Where(x => x.state_id == state.id))
                    {
                        city.gid = Guid.NewGuid();
                        city.state_gid = state_id;
                        newCities.Add(city);
                        Console.WriteLine($"{city.name} ilçesi için GUID ataması yapıldı.");
                    }
                    newStates.Add(state);
                    Console.WriteLine($"{state.name} ili için GUID ataması yapıldı.");
                }
                newCountries.Add(country);
                Console.WriteLine($"{country.name} ülkesi için GUID ataması yapıldı.");
            }

            var saveCities = from c in newCities
                             select new
                             {
                                 id = c.gid,
                                 name = c.name,
                                 state_id = c.state_gid,
                                 state_code = c.state_code,
                                 state_name = c.state_name,
                                 country_code = c.country_code,
                                 country_name = c.country_name,
                                 latitude = c.latitude,
                                 longitude = c.longitude,
                                 wikiDataId = c.wikiDataId,
                             };
            File.WriteAllText(path + "newCities.json", JsonConvert.SerializeObject(saveCities));
            Console.WriteLine("Yeni ilçeler json dosyası oluşturuldu");

            var saveStates = from s in newStates
                             select new
                             {
                                 id = s.gid,
                                 name = s.name,
                                 country_id = s.country_gid,
                                 country_code = s.country_code,
                                 country_name = s.country_name,
                                 state_code = s.state_code,
                                 latitude = s.latitude,
                                 longitude = s.longitude,
                             };
            File.WriteAllText(path + "newStates.json", JsonConvert.SerializeObject(saveStates));
            Console.WriteLine("Yeni iller json dosyası oluşturuldu");

            var saveCountries = from c in newCountries
                             select new
                             {
                                 id = c.gid,
                                 name = c.name,
                                 iso3 = c.iso3,
                                 iso2 = c.iso2,
                                 numeric_code = c.numeric_code,
                                 phone_code = c.phone_code,
                                 capital = c.capital,
                                 currency = c.currency,
                                 currency_name = c.currency_name,
                                 currency_symbol = c.currency_symbol,
                                 tld = c.tld,
                                 native = c.native,
                                 region = c.region,
                                 subregion = c.subregion,
                                 latitude = c.latitude,
                                 longitude = c.longitude,
                                 translations = c.translations
                             };
            File.WriteAllText(path + "newCountries.json", JsonConvert.SerializeObject(saveCountries));
            Console.WriteLine("Yeni ülkeler json dosyası oluşturuldu");
            watch.Stop();
            Console.WriteLine($"Toplam işlem süresi {watch.ElapsedMilliseconds} ms");
        }
    }



    public class Countries
    {
        public int id;
        public Guid gid { get; set; }
        public string name;
        public string iso3;
        public string iso2;
        public string numeric_code;
        public string phone_code;
        public string capital;
        public string currency;
        public string currency_name;
        public string currency_symbol;
        public string tld;
        public string native;
        public string region;
        public string subregion;
        public Dictionary<string, object> translations;
        public string latitude;
        public string longitude;
    }
    public class States
    {
        public int id;
        public Guid gid { get; set; }
        public string name;
        public int country_id;
        public Guid country_gid { get; set; }
        public string country_code;
        public string country_name;
        public string state_code;
        public string latitude;
        public string longitude;
    }
    public class Cities
    {
        public int id;
        public Guid gid { get; set; }
        public string name;
        public int state_id;
        public Guid state_gid { get; set; }
        public string state_code;
        public string state_name;
        public string country_code;
        public string country_name;
        public string latitude;
        public string longitude;
        public string wikiDataId;
    }
    public class Translations {
        public string Language { get; set; }
        public string name { get; set; }
    }
}
