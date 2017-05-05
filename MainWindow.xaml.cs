using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            long totalticksathome;
            List<Visit> visits = PrepData();
            Location home = FindHome(visits, out totalticksathome);
            if (home != null)
            {
                MessageBox.Show("We found where he lives ;-)");
                MessageBox.Show("Home latitude:" + home.lat + ", home longitude" + home.lon); //show home lat/long
                MessageBox.Show("He spent " + new TimeSpan(totalticksathome).ToString() + " at his home last week!"); //show total time at home
            }
            else
            {
                MessageBox.Show("No home was found!");
            }
            
        }

        private static List<Visit> PrepData(int testcase = 0)
        {
            List<Visit> visitList = new List<Visit>();
            switch (testcase)
            {
                case 0:
                    //Tally: home = 47h ( that falls into 8 pm ~ 8am), work = 3 h, night club = 8h, restaurant = 0
                    visitList.Add(new Visit(49.123, -113.123, new DateTime(2017, 5, 4, 20, 0, 0), new DateTime(2017, 5, 4, 4, 0, 0)));//night club - 8 h
                    visitList.Add(new Visit(46.123, -119.123, new DateTime(2017, 5, 4, 11, 0, 0), new DateTime(2017, 5, 4, 23, 0, 0)));// work - 3h
                    visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 5, 5, 21, 0, 0), new DateTime(2017, 5, 5, 10, 0, 0))); // home sick - 35h
                    visitList.Add(new Visit(46.123, -119.123, new DateTime(2017, 5, 5, 15, 0, 0), new DateTime(2017, 5, 5, 18, 0, 0))); //work - 0h
                    visitList.Add(new Visit(48.123, -115.123, new DateTime(2017, 5, 2, 13, 0, 0), new DateTime(2017, 5, 2, 15, 0, 0))); // restaurant 0h
                    visitList.Add(new Visit(46.123, -119.123, new DateTime(2017, 5, 2, 10, 0, 0), new DateTime(2017, 5, 2, 13, 0, 0)));// work - 0h
                    visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 5, 3, 19, 0, 0), new DateTime(2017, 5, 3, 9, 0, 0)));//home - 12 h
                    visitList.Add(new Visit(46.123, -119.123, new DateTime(2017, 5, 3, 12, 0, 0), new DateTime(2017, 5, 3, 18, 0, 0)));// work - 0 h
                    break;
                default: //other test cases
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 17, 17, 0, 0), new DateTime(2017, 1, 18, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    //visitList.Add(new Visit(45.123, -118.123, new DateTime(2017, 1, 1, 12, 0, 0), new DateTime(2017, 1, 1, 12, 0, 0)));
                    break;
            }
            return visitList;
        }

        //this method's signature doesn't exactly match requirements but I added the last parameter to verify values
        //it should be taken out
        private Location FindHome(List<Visit> visits, out long totalticksathome)
        {
            try
            {
                List<CalendarDateRange> acceptableRanges = GetAcceptableRanges(); //list of date time ranges within the past week from 8p - 8a next day
                Dictionary<Location, long> possibleHomes = new Dictionary<Location, long>(); //potential hits
                foreach (Visit v in visits)
                {
                    if (!(v.departure_time_local < acceptableRanges.Last().Start)) //filter out visits older than earliest acceptable daterange
                    {
                        long d = CalculateDuration(acceptableRanges, v);
                        if (d > 0) // if hours spent in location < or = 0 ignore
                        {
                            Location l = new Location(v.latitude, v.longitude);
                            if (possibleHomes.Any((p) => p.Key.lat == v.latitude && p.Key.lon == v.longitude))
                            {
                                l = possibleHomes.Where((p) => p.Key.lat == v.latitude && p.Key.lon == v.longitude).First().Key;
                            }
                            else
                            {
                                possibleHomes.Add(l, 0);
                            }
                            possibleHomes[l] = possibleHomes[l] + d; //accumulate hours to dictionary value
                        }
                    }
                }
                totalticksathome = possibleHomes != null && possibleHomes.Any() ? possibleHomes.Values.Max() : default(long); //max hours spent in winning location
                return possibleHomes.FirstOrDefault(x => x.Value == possibleHomes.Values.Max()).Key; // return the location with the highest number of hours
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private long CalculateDuration(List<CalendarDateRange> acceptableRanges, Visit v)
        {
            long targetVisit = 0;

            foreach (CalendarDateRange dr in acceptableRanges)
            { //http://baodad.blogspot.com/2014/06/date-range-overlap.html
                long d = Math.Min(dr.End.Subtract(dr.Start).Ticks, dr.End.Subtract(v.arrival_time_local).Ticks);
                d = Math.Min(d, v.departure_time_local.Subtract(v.arrival_time_local).Ticks);
                d = Math.Min(d, v.departure_time_local.Subtract(dr.Start).Ticks);
                d = Math.Max(d, 0); // eliminiate negative ranges
                targetVisit += d;
            }
            return targetVisit; //return time spent in location within acceptable ranges if any
        }

        private List<CalendarDateRange> GetAcceptableRanges()
        {
            List<CalendarDateRange> dataRanges = new List<CalendarDateRange>();
            for (int n = 0; n < 7; n++)
            { // TODO: simplify this
                DateTime startDay = DateTime.Now.Subtract(new TimeSpan(n + 1, 0, 0, 0)); ;
                DateTime endDay = DateTime.Now.Subtract(new TimeSpan(n, 0, 0, 0));
                CalendarDateRange dr = new CalendarDateRange(new DateTime(startDay.Year, startDay.Month, startDay.Day, 20, 0, 0), new DateTime(endDay.Year, endDay.Month, endDay.Day, 8, 0, 0));
                dataRanges.Add(dr);
            }
            return dataRanges;
        }
    }
    public class Visit
    {
        public double latitude; //float (e.g. 45.12345)
        public double longitude;// float (e.g. -118.12345)
        public DateTime arrival_time_local; // datetime (e.g. 5/30/2015 10:12:35)
        public DateTime departure_time_local;//datetime (e.g. 5/30/2015 18:12:35)
        public Visit(double lat, double lon, DateTime ar, DateTime dp)
        {
            latitude = Convert.ToDouble(lat.ToString("N3")); //adjust 3 decimal precision
            longitude = Convert.ToDouble(lon.ToString("N3"));
            arrival_time_local = ar;
            departure_time_local = dp;
        }
    }
    public class Location
    {
        public double lat;
        public double lon;
        public Location(double lt, double ln)
        {
            lat = lt;
            lon = ln;
        }
    }

}
