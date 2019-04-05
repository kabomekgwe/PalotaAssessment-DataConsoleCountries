using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace PalotaInterviewCS
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string countriesEndpoint = "https://restcountries.eu/rest/v2/all";

        static void Main(string[] args)
        {
            Country[] countries = GetCountries(countriesEndpoint).GetAwaiter().GetResult();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Palota Interview: Country Facts");
            Console.WriteLine();
            Console.ResetColor();

            Random rnd = new Random(); // random to populate fake answer - you can remove this once you use real values

            //TODO use data operations and data structures to optimally find the correct value (N.B. be aware of null values)

            /*
             * HINT: Sort the list in descending order to find South Africa's place in terms of gini coefficients
             * `Country.Gini` is the relevant field to use here           
             */
            var giniDescending = countries.OrderByDescending(x => x.Gini).ToArray();

            int southAfricanGiniPlace = Array.FindIndex(giniDescending, x => x.Name == "South Africa");
            Console.WriteLine($"1. South Africa's Gini coefficient is the {GetOrdinal(southAfricanGiniPlace + 1)} highest");


            /*
             * HINT: Sort the list in ascending order or just find the Country with the minimum gini coeficient          
             * use `Country.Gini` for the ordering then return the relevant country's name `Country.Name`
             */
            string lowestGiniCountry = countries.OrderByDescending(x => x.Gini).FirstOrDefault().Name;

            Console.WriteLine($"2. {lowestGiniCountry} has the lowest Gini Coefficient");

            /*
             * HINT: Group by regions (`Country.Region`), then count the number of unique timezones that the countries in each region span
             * Once you have done the grouping, find the group `Region` with the most timezones and return it's name and the number of unique timezones found          
             */
            var groupbyRegion = countries.GroupBy(x => x.Region)
                                        .Select(c => new
                                        {
                                            name = c.Select(x => x.Name).FirstOrDefault(),
                                            timeZones = c.Select(x => x.Timezones).Count()
                                        }).OrderByDescending(x => x.timeZones).FirstOrDefault();

            string regionWithMostTimezones = groupbyRegion.name; // Use correct value
            int amountOfTimezonesInRegion = groupbyRegion.timeZones; // Use correct value
            Console.WriteLine($"3. {regionWithMostTimezones} is the region that spans most timezones at {amountOfTimezonesInRegion} timezones");

            /*
             * HINT: Count occurances of each currency in all countries (check `Country.Currencies`)
             * Find the name of the currency with most occurances and return it's name (`Currency.Name`) also return the number of occurances found for that currency          
             */

            var currencies = countries.
                             GroupBy(x => x.Currencies)
                             .Select(x => new
                             {
                                 name = x.Select(y => y.Name).FirstOrDefault(),
                                 currency = x.FirstOrDefault().Currencies.FirstOrDefault().Name.Count()
                             }).OrderByDescending(x => x.currency).FirstOrDefault();

            string mostPopularCurrency = currencies.name; // Use correct value
            int numCountriesUsedByCurrency = currencies.currency; // Use correct value
            Console.WriteLine($"4. {mostPopularCurrency} is the most popular currency and is used in {numCountriesUsedByCurrency} countries");

            /*
             * HINT: Count the number of occurances of each language (`Country.Languages`) and sort then in descending occurances count (i.e. most populat first)
             * Once done return the names of the top three languages (`Language.Name`)
             */

            var topLanguages = (from c in countries

                                group c by c.Languages into l
                                select new
                                {
                                    langName = l.FirstOrDefault().Languages.FirstOrDefault().Name,
                                    langCount = l.FirstOrDefault().Languages.Count()
                                }).OrderByDescending(x => x.langCount).ToArray();

            //string[] mostPopularLanguages = topLanguages; ; // Use correct values

            Console.WriteLine($"5. The top three popular languages are {topLanguages[0].langName}, {topLanguages[1].langName} and {topLanguages[2].langName}");

            /*
             * HINT: Each country has an array of Bordering countries `Country.Borders`, The array has a list of alpha3 codes of each bordering country `Country.alpha3Code`
             * Sum up the population of each country (`Country.Population`) along with all of its bordering countries's population. Sort this list according to the combined population descending
             * Find the country with the highest combined (with bordering countries) population the return that country's name (`Country.Name`), the number of it's Bordering countries (`Country.Borders.length`) and the combined population
             * Be wary of null values           
             */

            string countryWithBorderingCountries = "ExampleCountry"; // Use correct value
            int numberOfBorderingCountries = rnd.Next(1, 10); // Use correct value
            int combinedPopulation = rnd.Next(1000000, 10000000); // Use correct value
            Console.WriteLine($"6. {countryWithBorderingCountries} and it's {numberOfBorderingCountries} bordering countries has the highest combined population of {combinedPopulation}");

            /*
             * HINT: Population density is calculated as (population size)/area, i.e. `Country.Population/Country.Area`
             * Calculate the population density of each country and sort by that value to find the lowest density
             * Return the name of that country (`Country.Name`) and its calculated density.
             * Be wary of null values when doing calculations           
             */

            var getPopulationDensity = (from p in countries
                                        where p.Area != null
                                        select new
                                        {
                                            name = p.Name,
                                            density = p.Population / p.Area
                                        });

            var lowestPopulationDensity = getPopulationDensity.OrderBy(x => x.density).FirstOrDefault();
            string lowPopDensityName = lowestPopulationDensity.name; // Use correct value
            double lowPopDensity = (double)lowestPopulationDensity.density * 100; // Use correct value
            Console.WriteLine($"7. {lowPopDensityName} has the lowest population density of {string.Format("{0:.##}", lowPopDensity)}");

            /*
             * HINT: Population density is calculated as (population size)/area, i.e. `Country.Population/Country.Area`
             * Calculate the population density of each country and sort by that value to find the highest density
             * Return the name of that country (`Country.Name`) and its calculated density.
             * Be wary of any null values when doing calculations. Consider reusing work from above related question           
             */

            var popDensity = getPopulationDensity.OrderByDescending(x => x.density).FirstOrDefault();
            string highPopDensityName = popDensity.name; // Use correct value
            double highPopDensity = (double)popDensity.density * 100; // Use correct value
            Console.WriteLine($"8. {highPopDensityName} has the highest population density of {string.Format("{0:.##}", highPopDensity)}");

            /*
             * HINT: Group by subregion `Country.Subregion` and sum up the area (`Country.Area`) of all countries per subregion
             * Sort the subregions by the combined area to find the maximum (or just find the maximum)
             * Return the name of the subregion
             * Be wary of any null values when summing up the area           
             */

            var subRegion = (from c in countries
                             where c.Area != null
                             group c by c.Subregion into g
                             select new
                             {
                                 total = g.Sum(s => s.Area),
                                 subReg = g.Select(s => s.Subregion).FirstOrDefault()
                             }
                       ).OrderByDescending(x => x.total).FirstOrDefault();

            string largestAreaSubregion = subRegion.subReg; // Use correct value
            Console.WriteLine($"9. {largestAreaSubregion} is the subregion that covers the most area");

            /*
             * HINT: Group by regional blocks (`Country.RegionalBlocks`). For each regional block, average out the gini coefficient (`Country.Gini`) of all member countries
             * Sort the regional blocks by the average country gini coefficient to find the lowest (or find the lowest without sorting)
             * Return the name of the regional block (`RegionalBloc.Name`) along with the calculated average gini coefficient
             */
            var regBlock = countries.GroupBy(x => x.RegionalBlocs)
                                    .Distinct()
                                    .Select(x =>
                                       new
                                       {
                                           name = x.Select(q => q.RegionalBlocs.Select(e => e.Name).FirstOrDefault()).FirstOrDefault(),
                                            // name = x.FirstOrDefault().RegionalBlocs.FirstOrDefault().Name,
                                            avg = x.Average(q => q.Gini)
                                       }).OrderByDescending(x => x.avg).FirstOrDefault();


            string mostEqualRegionalBlock = regBlock.name; // Use correct value
            double lowestRegionalBlockGini = (double)regBlock.avg; // Use correct value
            Console.WriteLine($"10. {mostEqualRegionalBlock} is the regional block with the lowest average Gini coefficient of {lowestRegionalBlockGini}");

            Console.ReadLine();
        }

        /// <summary>
        /// Gets the countries from a specified endpiny
        /// </summary>
        /// <returns>The countries.</returns>
        /// <param name="path">Path endpoint for the API.</param>
        static async Task<Country[]> GetCountries(string path)
        {
            Country[] countries = null;

            //TODO get data from endpoint and convert it to a typed array using Country.FromJson
            HttpResponseMessage response = await client.GetAsync(path);

            var result = response.Content.ReadAsStringAsync().Result;

            countries = JsonConvert.DeserializeObject<Country[]>(result);

            return countries;
        }

        /// <summary>
        /// Gets the ordinal value of a number (e.g. 1 to 1st)
        /// </summary>
        /// <returns>The ordinal.</returns>
        /// <param name="num">Number.</param>
        public static string GetOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }
    }
}
