﻿namespace FakeItEasy.Tests.Core
{
    using System.Linq;
    using FakeItEasy.Core;
    using FakeItEasy.SelfInitializedFakes;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultFakeWrapperConfiguratorTests
    {
        private IFoo faked;
        private DefaultFakeWrapperConfigurer wrapperConfigurator;

        [SetUp]
        public void Setup()
        {
            this.faked = A.Fake<IFoo>();

            this.wrapperConfigurator = new DefaultFakeWrapperConfigurer();
        }

        [Test]
        public void ConfigureFakeToWrap_should_add_WrappedFakeObjectRule_to_fake_object()
        {
            // Arrange
            var wrapped = A.Fake<IFoo>();

            // Act
            this.wrapperConfigurator.ConfigureFakeToWrap(this.faked, wrapped, null);

            // Assert
            Assert.That(Fake.GetFakeManager(this.faked).Rules.ToArray(), Has.Some.InstanceOf<WrappedObjectRule>());
        }

        [Test]
        public void ConfigureFakeToWrap_should_add_self_initialization_rule_when_recorder_is_specified()
        {
            // Arrange
            var recorder = A.Fake<ISelfInitializingFakeRecorder>();
            var wrapped = A.Fake<IFoo>();

            // Act
            this.wrapperConfigurator.ConfigureFakeToWrap(this.faked, wrapped, recorder);

            // Assert
            Assert.That(Fake.GetFakeManager(this.faked).Rules.First(), Is.InstanceOf<SelfInitializationRule>());
        }

        [Test]
        public void ConfigureFakeToWrap_should_not_add_self_initialization_rule_when_recorder_is_not_specified()
        {
            // Arrange
            var wrapped = A.Fake<IFoo>();

            // Act
            this.wrapperConfigurator.ConfigureFakeToWrap(this.faked, wrapped, null);

            // Assert
            Assert.That(Fake.GetFakeManager(this.faked).Rules, Has.None.InstanceOf<SelfInitializationRule>());
        }
    }
}
