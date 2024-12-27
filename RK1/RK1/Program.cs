using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK1
{
    class House
    {
        private string _adres;
        private int _apartment;
        private int _employee;

        public House(string adres = "", int apartment = 1, int employee = 0)
        {
            _adres = adres;
            _apartment = apartment;
            _employee = employee;
        }

        public string Adres
        {
            get { return _adres; }
            set { _adres = value; }
        }

        public int Apartment
        {
            get { return _apartment; }
            set { _apartment = value; }
        }

        public int Employee
        {
            get { return _employee; }
            set { _employee = value; }
        }
    }

    class Street
    {
        private string _name;
        private int _age;
        private string _city;
        private string _country;

        public Street(int age = 0, string country = "", string city = "", string name = "")
        {
            _age = age;
            _city = city;
            _country = country;
            _name = name;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }


        public string City
        {
            get { return _city; }
            set { _city = value; }
        }


        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }
    }

    public class OneToMany<T> where T : class
    {
        public List<T> Items { get; private set; } = new List<T>();

        public void AddItem(T item)
        {
            Items.Add(item);
        }
    }

    public class ManyToMany<T, U> where T : class where U : class
    {
        private Dictionary<T, List<U>> relationMap = new Dictionary<T, List<U>>();

        public void AddRelation(T firstObject, U secondObject)
        {
            if (!relationMap.ContainsKey(firstObject))
            {
                relationMap[firstObject] = new List<U>();
            }
            relationMap[firstObject].Add(secondObject);
        }

        public List<U> GetRelatedObjects(T firstObject)
        {
            if (relationMap.ContainsKey(firstObject))
            {
                return relationMap[firstObject];
            }
            return new List<U>();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
           
            Street street1 = new Street(10, "Russia", "Moscow", "Twerskaya");
            Street street2 = new Street(15, "Russia", "Moscow", "Baumanskaya");
            Street street3 = new Street(20, "Russia", "Moscow", "Leninsky Prospect");
            Street street4 = new Street(25, "Russia", "Moscow", "Arbat");
            Street street5 = new Street(30, "Russia", "Moscow", "Novoslobodskaya");
            Street street6 = new Street(35, "Russia", "Moscow", "Sretensky Boulevard");
                
            
            House house1 = new House("House 1", 1, 1);
            House house2 = new House("House 2", 2, 2);
            House house3 = new House("House 3", 3, 3);
            House house4 = new House("House 4", 4, 4);
            House house5 = new House("House 5", 5, 5);
            House house6 = new House("House 6", 6, 6);
            House house7 = new House("House 7", 7, 7);
            House house8 = new House("House 8", 8, 8);
            House house9 = new House("House 9", 9, 9);
            House house10 = new House("House 10", 10, 10);

            OneToMany<House> streetHousesOneToMany = new OneToMany<House>();
            streetHousesOneToMany.AddItem(house1);
            streetHousesOneToMany.AddItem(house2);
            streetHousesOneToMany.AddItem(house3);
            streetHousesOneToMany.AddItem(house4);
            streetHousesOneToMany.AddItem(house5);


            ManyToMany<House, Street> houseStreetManyToMany = new ManyToMany<House, Street>();
            houseStreetManyToMany.AddRelation(house6, street1);
            houseStreetManyToMany.AddRelation(house6, street2);
            houseStreetManyToMany.AddRelation(house6, street3);
            houseStreetManyToMany.AddRelation(house7, street2);
            houseStreetManyToMany.AddRelation(house7, street4);
            houseStreetManyToMany.AddRelation(house7, street5);
            houseStreetManyToMany.AddRelation(house8, street1);
            houseStreetManyToMany.AddRelation(house8, street3);
            houseStreetManyToMany.AddRelation(house8, street5);
            houseStreetManyToMany.AddRelation(house9, street4);
            houseStreetManyToMany.AddRelation(house9, street6);
            houseStreetManyToMany.AddRelation(house10, street1);
            houseStreetManyToMany.AddRelation(house10, street2);
            houseStreetManyToMany.AddRelation(house10, street6);

            Street[] streets = { street1, street2, street3, street4, street5, street6 };
            House[] houses = { house1, house2, house3, house4, house5, house6, house7, house8, house9, house10 };

            var streetsWithProspectOrBoulevard = streets.Where(s => s.Name.Contains("Prospect") || s.Name.Contains("Boulevard"));

            Console.WriteLine("Улицы с 'Prospect' или 'Boulevard':");
            foreach (var street in streetsWithProspectOrBoulevard)
            {
                Console.WriteLine(street.Name);

                foreach (var house in houses) 
                {
                    if (houseStreetManyToMany.GetRelatedObjects(house).Contains(street)) 
                    {
                        Console.WriteLine($"\t\t{house.Adres}");
                    }
                }
            }


            Console.ReadKey();


        }
    }
}
