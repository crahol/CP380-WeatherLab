using System;
using System.Linq;


namespace WeatherLab
{
    class Program
    {
        static string dbfile = @".\data\climate.db";

        static void Main(string[] args)
        {
            var measurements = new WeatherSqliteContext(dbfile).Weather;

            var total_2020_precipitation = measurements
                .Where(data => data.year == 2020)
                .Select(r => r.precipitation);
            Console.WriteLine($"Total precipitation in 2020: {total_2020_precipitation.Sum()} mm\n");

            //
            // Heating Degree days have a mean temp of < 18C
            //   see: https://en.wikipedia.org/wiki/Heating_degree_day
            //

            // ?? TODO ??

            //
            // Cooling degree days have a mean temp of >=18C
            //

            // ?? TODO ??

            //
            // Most Variable days are the days with the biggest temperature
            // range. That is, the largest difference between the maximum and
            // minimum temperature
            //
            // Oh: and number formatting to zero pad.
            // 
            // For example, if you want:
            //      var x = 2;
            // To display as "0002" then:
            //      $"{x:d4}"
            //
            Console.WriteLine("Year\tHDD\tCDD");
            // ?? TODO ??
            var group = measurements
                .GroupBy(r => r.year)
                .Select(grp => new
                {
                    year = grp.Key,
                    cdd = grp.Where(g => g.meantemp >= 18).Count(),
                    hdd = grp.Where(g => g.meantemp < 18).Count()
                });

            foreach (var g in group)
            {
                Console.WriteLine($"{g.year}\t{g.hdd}\t{g.cdd}");
            }

            Console.WriteLine("\nTop 5 Most Variable Days");
            Console.WriteLine("YYYY-MM-DD\tDelta");

            // ?? TODO ??
            var mostVariableDays = measurements
                .Select(row => new
                    {
                        date = $"{row.year}-{row.month:d2}-{row.day:d2}",
                        diff = (row.maxtemp - row.mintemp)
                    })
                .OrderByDescending(r => r.diff);

            int count = 0;
            foreach (var m in mostVariableDays)
            {
                if (count < 5)
                {
                    Console.WriteLine($"{m.date}\t{m.diff}");
                    count++;
                } else
                {
                    break;
                }
            }
        }
    }
}
