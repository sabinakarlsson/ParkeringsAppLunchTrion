﻿using ParkeringsAppLunchTrion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Numerics;

namespace ParkeringsAppLunchTrion
{
    public class Helpers
    {
        public static void VehicleCheckIn(ParkingLot parkingLot, List<Vehicle> vehicles, List<MC> mCs)
        {
            //int randomVehicle = Random.Shared.Next(0, 3);
            //switch (randomVehicle)

            ConsoleKeyInfo key = Console.ReadKey();
            Console.Clear();
            switch (key.KeyChar)
            {
                case '0':
                    Console.WriteLine("En bil rullar in på parkeringen");
                    break;
                case '1':
                    Console.WriteLine("En buss rullar in på parkeringen");
                    break;
                case '2':
                    Console.WriteLine("En MC rullar in på parkeringen");
                    break;
            }

            string regNumber = "";
            while (regNumber == "")
            {
                Console.WriteLine("Ange registreringsnummer: ");
                string checkRegNumber = Console.ReadLine();
                checkRegNumber = checkRegNumber.ToUpper();
                var isValid = Regex.IsMatch(checkRegNumber, @"^[A-Z]{3}\d{3}$");
                if (isValid)
                {
                    regNumber = checkRegNumber;
                }
                else
                {
                    Console.WriteLine("Ange rätt regNummer i rätt format, ex: (ABC123)");
                }
            }

            Console.WriteLine("Ange färg på ditt fordon: ");
            string vehicleColor = Console.ReadLine();

            
            Console.WriteLine("Hur länge vill du parkera i sekunder? ");

            bool lyckad1 = false;
            int parkingTime = 0;
            while (lyckad1 == false)
            {
                lyckad1 = Int32.TryParse(Console.ReadLine(), out parkingTime);
                if (!lyckad1)
                {
                    Console.WriteLine("Du har inte angett sekunder med siffor! ");
                }
            }

            //switch (randomVehicle)

            switch (key.KeyChar)
            {
                case '0': //Bil
                    Console.WriteLine("Är det en elbil?");
                    Console.WriteLine("[1] ja");
                    Console.WriteLine("[2] nej");
                    ConsoleKeyInfo key1 = Console.ReadKey();
                    bool electric = true;
                    switch (key1.KeyChar)
                    {
                        case '1':
                            electric = true;
                            break;

                        case '2':
                            electric = false;
                            break;
                    }

                    bool deluxeAvaliable = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (parkingLot.ParkingSpots[i] == 4)
                        {
                            Console.WriteLine("\nVill du parkera på en deluxe-parkering? Parkeringsplats nära utgång för 0.75kr extra");
                            Console.WriteLine("[1] ja");
                            Console.WriteLine("[2] nej");
                            ConsoleKeyInfo key2 = Console.ReadKey();
                            switch (key2.KeyChar)
                            {
                                case '1':
                                    Car car1 = new Car(regNumber, vehicleColor, (electric == true ? true : false), true, parkingTime);
                                    vehicles.Add(car1);
                                    ParkingLot.ParkVehicle(parkingLot, car1, mCs);
                                    break;

                                case '2':
                                    Car car2 = new Car(regNumber, vehicleColor, (electric == true ? true : false), false, parkingTime);
                                    vehicles.Add(car2);
                                    ParkingLot.ParkVehicle(parkingLot, car2, mCs);
                                    break;
                            }
                            deluxeAvaliable = true;
                            break;
                        }
                            

                        
                    }

                    
                    if (deluxeAvaliable == false)
                    {
                        Console.WriteLine("\nVåra deluxeparkeringar är tyvärr fulla. Du kommer bli tilldelad en vanlig parkeringsplats");
                        Thread.Sleep(4000);
                        Car car3 = new Car(regNumber, vehicleColor, (electric == true ? true : false), false, parkingTime);
                        vehicles.Add(car3);
                        ParkingLot.ParkVehicle(parkingLot, car3, mCs);
                    }

                    break;

                case '1': //Buss
                    Console.WriteLine("Hur många platser är det i bussen?");
                    bool lyckad = false;
                    int numberOfSeats = 0;
                    while (lyckad == false)
                    {
                        lyckad = Int32.TryParse(Console.ReadLine(), out numberOfSeats);
                        if (!lyckad)
                        {
                            Console.WriteLine("Du har inte angett antal med siffor, ");
                        }
                    }
                    Bus bus1 = new Bus(regNumber, vehicleColor, numberOfSeats, parkingTime);
                    vehicles.Add(bus1);
                    ParkingLot.ParkVehicle(parkingLot, bus1, mCs);
                    break;

                case '2': //MC
                    Console.WriteLine("Vilket märke är det på motorcykeln?");
                    string mcBrand = Console.ReadLine();

                    MC mc1 = new MC(regNumber, vehicleColor, mcBrand, parkingTime);
                    vehicles.Add(mc1);
                    mCs.Add(mc1);
                    ParkingLot.ParkVehicle(parkingLot, mc1, mCs);
                    break;
            }

            Console.WriteLine("\n\nTryck på valfri knapp för att gå tillbaka till menyn! ");
            Console.ReadKey();
            return;

        }

        public static double CalculateParkedTime(DateTime checkOutTime, Vehicle vehicle)
        {
            TimeSpan timeSpan = checkOutTime - vehicle.StartingTime;

            int parkedTime = (int)timeSpan.TotalSeconds;

            return parkedTime;
        }

        public static void CalculateExendedTime(int extendedTime, Vehicle vehicle)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(extendedTime);

            vehicle.EndTime += timeSpan;

        }

        public static void CheckOut(Vehicle vehicle, ParkingLot parkingLot, List<Vehicle> vehicles, List<MC> mCs)
        {

            if (vehicle is Car && ((Car)vehicle).Deluxe == true)
            {
                parkingLot.ParkingSpots[vehicle.ParkingSpot] = 4;
            }

            else
            {
                if (vehicle is MC)
                {
                    parkingLot.ParkingSpots[vehicle.ParkingSpot]--;
                    mCs.Remove((MC)vehicle);
                }
                else
                {
                    parkingLot.ParkingSpots[vehicle.ParkingSpot] = 0;
                }
            }

            vehicles.Remove(vehicle);

            
        }


        public static int ExtendTime(int parkingTime, int chosenTime)
        {
            
            int extendedTime = parkingTime + chosenTime;

            return extendedTime;

        }


        //Gamla metoden för kostnadsuträkning (innan vi la till deluxeparkeringar)
        public static double CalculateCost(double parkedTime)
        {
            parkedTime = Math.Abs(parkedTime);

            return parkedTime * 1.5;
        }

        public static double CalculatePrice(double parkedTime, Vehicle vehicle)
        {
            parkedTime = Math.Abs(parkedTime);

            if (vehicle is Car && ((Car)vehicle).Deluxe == true)
            {
                return parkedTime * 2.25;
            }
            
            return parkedTime * 1.5;
        }

        public static int AddNumberOfVehicles(int numberOfVehicles)
        {
            numberOfVehicles++;

            return numberOfVehicles;
        }


        public static void AddTestVehicles (ParkingLot parkingLot, List<Vehicle> vehicles, List<MC> mCs)
        {
            Car testCar = new Car("ABC123", "Blå", true, false, 10);
            vehicles.Add(testCar);
            testCar.ParkingSpot = 3;
            parkingLot.ParkingSpots[3] = 2;

            MC testMC = new MC("DEF456", "Grå", "Yamaha", 650);
            vehicles.Add(testMC);
            testMC.ParkingSpot = 4;
            parkingLot.ParkingSpots[4] = 1;
            mCs.Add(testMC);
            

            Bus testBus = new Bus("GHI789", "Röd", 8, 550);
            vehicles.Add(testBus);
            testBus.ParkingSpot = 5;
            parkingLot.ParkingSpots[5] = 2;
            parkingLot.ParkingSpots[6] = 2;

            Car testDeluxeCar = new Car("JKL123", "Blå", true, true, 650);
            vehicles.Add(testDeluxeCar);
            testDeluxeCar.ParkingSpot = 0;
            parkingLot.ParkingSpots[0] = 2;

            Car testDeluxeCar2 = new Car("MNO456", "Blå", true, true, 850);
            vehicles.Add(testDeluxeCar2);
            testDeluxeCar2.ParkingSpot = 1;
            parkingLot.ParkingSpots[1] = 2;

        }


    }
}
