using CustomerLib;
using CustomerManagementApp.ViewModels;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Windows.Data;
using Xunit;

namespace CustomerManagementApp.Tests
{
    public class MainViewModelTests
    {
        [Fact]
        public void Constructor_NullRepository_ShouldThrow()
        {
            Action act = () => new MainViewModel(null);

            act.Should().Throw<ArgumentNullException>()
                .Where(e => e.Message.Contains("customerRepository"));
        }

        [Fact]
        public void Constructor_Customers_ShouldHaveValue()
        {
            var repository = A.Fake<ICustomerRepository>();
            var customers = new List<Customer>();
            A.CallTo(() => repository.Customers).Returns(customers);
            var vm = new MainViewModel(repository);

            vm.Customers.Should().BeEquivalentTo(customers);
        }

        [Fact]
        public void Constructor_SelectedCustomer_ShouldBeNull()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);

            vm.SelectedCustomer.Should().BeNull();
        }

        [Fact]
        public void AddCommand_ShouldAddInRepository()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);

            vm.AddCommand.Execute(null);
            A.CallTo(() => repository.Add(A<Customer>._)).MustHaveHappened();
        }

        [Fact]
        public void AddCommand_SelectedCustomer_ShouldNotBeNull()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            vm.AddCommand.Execute(null);
            vm.SelectedCustomer.Should().NotBeNull();
        }

        [Fact]
        public void AddCommand_ShouldNotifyCustomers()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            var wasNotified = false;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Customers")
                    wasNotified = true;
            };
            vm.AddCommand.Execute(null);
            wasNotified.Should().BeTrue();
        }

        [Fact]
        public void RemoveCommand_SelectedCustomerNull_ShouldNotRemoveInRepository()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            vm.RemoveCommand.Execute(null);
            A.CallTo(() => repository.Remove(A<Customer>._)).MustNotHaveHappened();
        }

        [Fact]
        public void RemoveCommand_SelectedCustomerNotNull_ShouldRemoveInRepository()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            vm.SelectedCustomer = new Customer();
            vm.RemoveCommand.Execute(null);
            A.CallTo(() => repository.Remove(A<Customer>._)).MustHaveHappened();
        }

        [Fact]
        public void RemoveCommand_SelectedCustomer_ShouldBeNull()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            vm.SelectedCustomer = new Customer();
            vm.RemoveCommand.Execute(null);
            vm.SelectedCustomer.Should().BeNull();
        }

        [Fact]
        public void RemoveCommand_ShouldNotifyCustomers()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            vm.SelectedCustomer = new Customer();
            var wasNotified = false;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Customers")
                    wasNotified = true;
            };
            vm.RemoveCommand.Execute(null);
            wasNotified.Should().BeTrue();
        }

        [Fact]
        public void SaveCommand_ShouldCommitInRepository()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            vm.SaveCommand.Execute(null);
            A.CallTo(() => repository.Commit()).MustHaveHappened();
        }

        [Fact]
        public void SearchCommand_WithText_ShouldSetFilter()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            vm.SearchCommand.Execute("text");
            var coll = CollectionViewSource.GetDefaultView(vm.Customers);
            coll.Filter.Should().NotBeNull();
        }

        [Fact]
        public void SearchCommand_WithoutText_ShouldSetFilter()
        {
            var repository = A.Fake<ICustomerRepository>();
            var vm = new MainViewModel(repository);
            vm.SearchCommand.Execute("");
            var coll = CollectionViewSource.GetDefaultView(vm.Customers);
            coll.Filter.Should().BeNull();
        }
    }
}
