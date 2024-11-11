﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkeringsAppLunchTrion
{
    public class Vehicle
    {
        public string RegNr { get; set; }
        public string Color { get; set; }   
        public int ParkingTime { get; set; }
        public int ParkingSpot { get; set; }
        public int StartTime { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }

        public Vehicle(string regNr, string color, int parkingTime)
        {
            RegNr = regNr;
            Color = color;
            ParkingTime = parkingTime;
            StartTime = parkingTime;
            StartingTime = DateTime.Now;
            EndTime = CalculateEndTime(parkingTime, StartingTime);

        }
    }

    public class Car : Vehicle
    {
        public bool Electric { get; set; }

        public Car(string regNr, string color, bool electric, int parkingTime) : base(regNr, color, parkingTime)
        {
            Electric = electric;
            
        }
    }

    public class Bus : Vehicle
    {
        public int NrSeats { get; set; }

        public Bus(string regNr, string color, int nrSeats, int parkingTime) : base (regNr, color, parkingTime)
        {
            NrSeats = nrSeats;

        }
    }

    public class MC : Vehicle
    {
        public string Brand { get; set; }

        public MC(string regNr, string color, string brand, int parkingTime) : base(regNr, color, parkingTime)
        {
            Brand = brand;
        
        }

    }

    public static DateTime CalculateEndTime(int parkingTime, DateTime startingTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(parkingTime);

        DateTime endTime = startingTime + timeSpan;
        return endTime;
    }


}
