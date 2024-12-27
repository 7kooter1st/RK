using Microsoft.VisualStudio.TestTools.UnitTesting;
using RK1;
using System.Collections.Generic;
using System.Linq;

namespace RK1Tests
{
    [TestClass]
    public class RK1Tests
    {
        [TestMethod]
        public void OneToMany_AddItem_Test()
        {
            OneToMany<House> oneToMany = new OneToMany<House>();
            House house = new House("Test Address", 10, 5);
            oneToMany.AddItem(house);
            Assert.AreEqual(1, oneToMany.Items.Count);
            Assert.AreEqual(house, oneToMany.Items[0]);
        }

        [TestMethod]
        public void ManyToMany_AddRelation_Test()
        {
            ManyToMany<House, Street> manyToMany = new ManyToMany<House, Street>();
            House house = new House();
            Street street = new Street();
            manyToMany.AddRelation(house, street);
            Assert.AreEqual(1, manyToMany.GetRelatedObjects(house).Count);
            Assert.AreEqual(street, manyToMany.GetRelatedObjects(house)[0]);
        }

        [TestMethod]
        public void ManyToMany_GetRelatedObjects_Empty_Test()
        {
            ManyToMany<House, Street> manyToMany = new ManyToMany<House, Street>();
            House house = new House();
            List<Street> streets = manyToMany.GetRelatedObjects(house);
            Assert.AreEqual(0, streets.Count);
        }

        [TestMethod]
        public void ManyToMany_GetRelatedObjects_MultipleRelations_Test()
        {
            ManyToMany<House, Street> manyToMany = new ManyToMany<House, Street>();
            House house = new House();
            Street street1 = new Street();
            Street street2 = new Street();
            manyToMany.AddRelation(house, street1);
            manyToMany.AddRelation(house, street2);
            List<Street> streets = manyToMany.GetRelatedObjects(house);
            Assert.AreEqual(2, streets.Count);
            Assert.IsTrue(streets.Contains(street1));
            Assert.IsTrue(streets.Contains(street2));
        }

        [TestMethod]
        public void StreetFiltering_Test()
        {
            Street street1 = new Street(10, "Russia", "Moscow", "Twerskaya");
            Street street2 = new Street(15, "Russia", "Moscow", "Baumanskaya Prospect");
            Street street3 = new Street(20, "Russia", "Moscow", "Leninsky Boulevard");

            Street[] streets = { street1, street2, street3 };

            var filteredStreets = streets.Where(s => s.Name.Contains("Prospect") || s.Name.Contains("Boulevard"));

            Assert.AreEqual(2, filteredStreets.Count());
            Assert.IsTrue(filteredStreets.Contains(street2));
            Assert.IsTrue(filteredStreets.Contains(street3));
        }


        [TestMethod]
        public void IntegrationTest()
        {
            // Simplified setup for testing
            Street street1 = new Street(0, "", "", "Leninsky Prospect");
            House house1 = new House("House 1", 1, 1);
            House house2 = new House("House 2", 2, 2);

            ManyToMany<House, Street> houseStreetManyToMany = new ManyToMany<House, Street>();
            houseStreetManyToMany.AddRelation(house1, street1);


            Street[] streets = { street1 };
            House[] houses = { house1, house2 };

            var streetsWithProspectOrBoulevard = streets.Where(s => s.Name.Contains("Prospect") || s.Name.Contains("Boulevard"));

            List<string> houseAddresses = new List<string>();

            foreach (var street in streetsWithProspectOrBoulevard)
            {
                foreach (var house in houses)
                {
                    if (houseStreetManyToMany.GetRelatedObjects(house).Contains(street))
                    {
                        houseAddresses.Add(house.Adres);
                    }
                }
            }

            Assert.AreEqual(1, houseAddresses.Count);
            Assert.AreEqual("House 1", houseAddresses[0]);
        }
    }
}