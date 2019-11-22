using System.Collections.Generic;

namespace TravelingSalesman
{
    class Individual
    {
        public List<int> Route { get; set; }
        public double Distance { get; set; }

        public Individual(List<int> route, double distance)
        {
            Route = route;
            Distance = distance;
        }
    }
}
