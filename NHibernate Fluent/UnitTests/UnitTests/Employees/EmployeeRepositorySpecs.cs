using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NHibernateDemo.DataAccess.Employees;
using NHibernateDemo.Entities.Employees;
using NHibernateDemo.UnitTests.Setup;

namespace NHibernateDemo.UnitTests.Employees
{
    public abstract class employee_repository_concern : base_concern
    {
        Establish c = () =>
        {
            employee_repository = new EmployeeRepository(session);
        };

        protected static IEmployeeRepository employee_repository;
    }

    [Subject("Get Employee By Id")]
    public class when_getting_an_employee_by_id : employee_repository_concern
    {
        Establish c = () => 
                      GlobalDataSetup.add_stub_employee_to_database();
			
        Because b = () =>
                    employee = employee_repository.find_by_id(1);

        It should_return_the_employee_found = () =>
                                              employee.Id.ShouldEqual(1);

        static Employee employee;
    }

    [Subject("Getting Employee that does not exist")]
    public class when_getting_an_employee_that_doesnt_exist : employee_repository_concern
    {
        Because b = () =>
                    employee = employee_repository.find_by_id(1);

        It should_return_a_null = () =>
                                  employee.ShouldBeNull();

        static Employee employee;
    }

    [Subject("Getting all employees that have a matching name")]
    public class when_getting_an_employee_by_name : employee_repository_concern
    {
        Establish c = () =>
        {
            //Add 2 employees with the same name to database
            GlobalDataSetup.add_stub_employee_to_database();
            GlobalDataSetup.add_stub_employee_to_database();

            //Add a 3rd employee with a different name to the database
            GlobalDataSetup.add_stub_employee_to_database();
            different_employee = GlobalDataSetup.Session.Get<Employee>(3);
            different_employee.FirstName = "Bill";
            different_employee.LastName = "Lumberg";
            GlobalDataSetup.Session.SaveOrUpdate(different_employee);
            GlobalDataSetup.Session.Flush();
        };

        Because b = () =>
            employees = employee_repository.find_by_name("Peter", "Gibbons");

        It should_return_a_collection_of_all_employees_found = () =>
            employees.Count().ShouldEqual(2); //Employee3 should not be in there

        static Employee different_employee;
        static IEnumerable<Employee> employees;
    }

    [Subject("Get All Employees")]
    public class when_getting_all_the_employees : employee_repository_concern
    {
        Establish c = () =>
        {
            GlobalDataSetup.add_stub_employee_to_database();
            GlobalDataSetup.add_stub_employee_to_database();
        };

        Because b = () =>
            employees = employee_repository.get_all();

        It should_return_a_collection_of_all_the_employees = () => 
            employees.Count().ShouldEqual(2);

        static IEnumerable<Employee> employees;
    }

    [Subject("Adding a new employee to the repository")]
    public class when_saving_a_new_employee_to_the_repository : employee_repository_concern
    {
        Establish c = () =>
        {
            employee = GlobalDataSetup.get_stub_employee_values();
            employee.Id = -1;
        };

        Because b = () =>
                    employee_repository.save(employee);

        It should_persist_the_employee_in_the_repository = () =>
        {
            found_employee = GlobalDataSetup.Session.Get<Employee>(1);
            found_employee.Id.ShouldEqual(1);
        };

        static Employee employee;
        static Employee found_employee;
    }

    [Subject("Updating an existing employee")]
    public class when_updating_an_employee : employee_repository_concern
    {
        Establish c = () =>
        {
            GlobalDataSetup.add_stub_employee_to_database();
            employee = GlobalDataSetup.Session.Get<Employee>(1);
            employee.FirstName = "Bill";
        };

        Because b = () =>
        {
            employee_repository.save(employee);
            GlobalDataSetup.Session.Clear(); //If you remove this then you will force a trip to the database instead of the cache
        };

        It should_persist_the_changes_to_the_employee = () =>
        {
            found_employee = GlobalDataSetup.Session.Get<Employee>(1);
            found_employee.FirstName.ShouldEqual("Bill");
        };

        static Employee employee;
        static Employee found_employee;
    }

    [Subject("Delete existing employee")]
    public class when_deleting_an_employee : employee_repository_concern
    {
        Establish c = () =>
        {
            GlobalDataSetup.add_stub_employee_to_database();
            employee = GlobalDataSetup.Session.Get<Employee>(1);
        };

        Because b = () => 
                    employee_repository.delete(employee);

        It should_remove_the_employee_from_the_repository = () =>
        {
            GlobalDataSetup.Session.Get<Employee>(1).ShouldBeNull();
        };

        static Employee employee;
    }
}