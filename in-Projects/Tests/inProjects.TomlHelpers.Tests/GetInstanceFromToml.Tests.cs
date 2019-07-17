using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using NUnit.Framework;

namespace inProjects.TomlHelpers.Tests
{
    [TestFixture]
    public class GetInstanceFromToml
    {
        [Test]
        public void GetInstanceFromToml_tests()
        {
            Person loaded_person = TomlHelpers.GetInstanceFromToml<Person>(File.ReadAllText("./toml_sample_for_tests/person.toml"));
            Person created_person = new Person(); created_person.name = new Name();
            created_person.age = 12; created_person.name.first = "Paul"; created_person.name.last = "poulet";

            Assert.AreEqual(loaded_person.age, created_person.age);
            Assert.AreEqual(loaded_person.name.first, created_person.name.first);
            Assert.AreEqual(loaded_person.name.last, created_person.name.last);
        }
    }

    // some class used in tests
    class Person
    {
        public int age {get; set;}
        public Name name {get; set;}
    }

    class Name
    {
        public string first {get; set;}
        public string last {get; set;}
    }
}