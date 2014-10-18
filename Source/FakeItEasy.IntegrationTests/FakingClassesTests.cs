namespace FakeItEasy.IntegrationTests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Core;
    using FakeItEasy.Tests.Core;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class FakingClassesTests
    {
        [Test]
        public void Should_be_able_to_get_a_fake_value_of_uri_type()
        {
            using (Fake.CreateScope(new NullFakeObjectContainer()))
            {
                A.Fake<Uri>();
            }
        }

        [Test]
        public void Should_be_able_to_serialize_a_fake()
        {
            // arrange
            var person = A.Fake<Person>();

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                // act
                formatter.Serialize(stream, person);
            }
        }

        [Test]
        public void Should_be_able_to_serialize_and_deserialize_a_fake_and_use_the_deserialized_fake()
        {
            // arrange
            var person = A.Fake<Person>();

            Person deserializedPerson;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                // act
                formatter.Serialize(stream, person);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedPerson = (Person)formatter.Deserialize(stream);
            }

            deserializedPerson.Name = "name";

            // Assert
            deserializedPerson.Name.Should().Be("name");
        }

        [Test]
        public void Should_be_able_to_serialize_and_deserialize_a_fake_with_virtual_property()
        {
            // Arrange
            var person = A.Fake<Person>();
            person.Name = "name";

            // Act
            Person deserializedPerson;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, person);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedPerson = (Person)formatter.Deserialize(stream);
            }

            // Assert
            deserializedPerson.Name.Should().Be("name");
        }

        [Test, Ignore]
        public void Should_be_able_to_serialize_and_deserialize_a_fake_with_recoreded_calls()
        {
            // Arrange
            var person = A.Fake<Person>();
            person.AbstractMethod();

            // Act
            Person deserializedPerson;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, person);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedPerson = (Person) formatter.Deserialize(stream);
            }

            // Assert
            // TODO
        }

        [Test]
        public void Should_be_able_to_serialize_and_deserialize_a_fake_with_configured_method()
        {
            // Arrange
            var person = A.Fake<Person>();
            A.CallTo(() => person.AbstractMethod()).Returns("x");

            // Act
            Person deserializedPerson;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, person);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedPerson = (Person)formatter.Deserialize(stream);
            }

            // Assert
            // TODO
        }

        [Serializable]
        public abstract class Person
        {
            public virtual string Name { get; set; }

            public abstract string AbstractMethod();
        }
    }
}