using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernateDemo.Entities.Employees;
using NHibernateDemo.UnitTests.Setup;

namespace NHibernateDemo.UnitTests.Employees
{
    public abstract class employee_concern : base_concern
    {
       
    }

    public abstract class employee_concern_with_a_different_first_name : base_concern
    {
        Establish c = () =>
        {
            GlobalDataSetup.add_stub_employee_to_database();
            GlobalDataSetup.add_stub_employee_to_database();
            GlobalDataSetup.add_stub_employee_to_database();

            var different_employee = GlobalDataSetup.Session.Get<Employee>(3);
            different_employee.FirstName = "Bob";
            GlobalDataSetup.Session.SaveOrUpdate(different_employee);
            GlobalDataSetup.Session.Flush();
        };
    }

    [Subject("Getting an employee by id using nhibernate")]
    public class when_getting_an_employee_by_id_using_nhibernate : employee_concern
    {
        Establish c = () =>
                      GlobalDataSetup.add_stub_employee_to_database();
	
        Because b = () =>
                    employee = GlobalDataSetup.Session.Get<Employee>(1);

        It should_return_the_employee_found = () =>
                                              employee.Id.ShouldEqual(1);

        static Employee employee;
    }

    [Subject("Getting non existant employee by id using nhibernate")]
    public class when_getting_an_employee_by_id_that_doesnt_exist_using_nhibernate_get : employee_concern
    {
        Establish context = () => 
                            GlobalDataSetup.add_stub_employee_to_database();

        Because b = () =>
        {
            employee = GlobalDataSetup.Session.Get<Employee>(2);
        };

        It should_return_null = () => 
                                employee.ShouldBeNull();

        static Employee employee;
    }

    [Subject("Getting employee that doesnt exist using nhibernate load")]
    public class when_getting_an_employee_that_doesnt_exist_using_nhibernate_load : employee_concern
    {
        Establish c = () =>
                      GlobalDataSetup.add_stub_employee_to_database();

        Because b = () =>
                    employee = GlobalDataSetup.Session.Load<Employee>(2);

        It should_return_a_proxy_object = () =>
                                          employee.ShouldNotBeOfType(typeof (Employee));

        It should_throw_an_nhibernate_object_not_found_exception_when_trying_to_use_the_employee = () =>
        {
            try
            {
                var e = employee.Id; //When you try to access something from the proxy
                //it will throw an exception.
            }
            catch (Exception exception)
            {
                exception.ShouldBeOfType<ObjectNotFoundException>();
            }
        };

        static Employee employee;
    }

    [Subject("Get Employees by First Name using Criteria Query")]
    public class when_getting_all_employees_by_first_name_using_critiera_query 
        : employee_concern_with_a_different_first_name 
    {
        Because b = () =>
        {
            employees = GlobalDataSetup.Session
                .CreateCriteria<Employee>()
                .Add(Restrictions.Eq("FirstName", "Peter"))
                .List<Employee>();
        };

        It should_return_all_the_employees_with_matching_values = () =>
        {
            employees.Count.ShouldBeGreaterThan(0);

            foreach (var employee in employees)
            {
                employee.FirstName.ShouldEqual("Peter");
            }
        };

        static IList<Employee> employees;
    }

    [Subject("Getting Employees by FirstName using HQL")]
    public class when_getting_employees_by_firstname_using_nhibernate_hql 
        : employee_concern_with_a_different_first_name
    {
        Because b = () =>
        {
            employees = GlobalDataSetup.Session
                .CreateQuery("from Employee e where e.FirstName = :firstname")
                .SetParameter("firstname", "Peter")
                .List<Employee>();
        };

        It should_return_all_the_employees_with_matching_first_name = () =>
        {
            employees.Count.ShouldBeGreaterThan(0);

            foreach (var employee in employees)
            {
                employee.FirstName.ShouldEqual("Peter");
            }
        };

        static IList<Employee> employees;
    }

    [Subject("Getting Employees by FirstName using Linq To NHibernate")]
    public class when_getting_employees_by_firstname_using_linq_to_nhibernate
        : employee_concern_with_a_different_first_name
    {
        Because b = () =>
        {
            employees = (from e in GlobalDataSetup.Session.Linq<Employee>()
                         where e.FirstName == "Peter"
                         select e).ToList();
        };

        It should_return_all_the_employees_with_matching_first_name = () =>
        {
            employees.Count.ShouldBeGreaterThan(0);

            foreach (var employee in employees)
            {
                employee.FirstName.ShouldEqual("Peter");
            }
        };

        static IList<Employee> employees;
    }

    [Subject("Getting Employees with a specified Manager using Linq to NHibernate")]
    public class when_getting_all_employees_with_a_manager_named_bill : employee_concern
    {
        Establish c = () =>
        {
            GlobalDataSetup.add_stub_manager_with_2_employees_to_database();
        };

        Because b = () =>
        {
            employees = (from e in GlobalDataSetup.Session.Linq<Employee>()
                         where e.Manager.FirstName == "Bill"
                         select e)
                .ToList();
        };

        It should_return_all_the_employees_with_the_specified_manager = () =>
        {
            employees.Count.ShouldBeGreaterThan(0);

            foreach (var employee in employees)
            {
                employee.Manager.FirstName.ShouldEqual("Bill");
            }
        };

        static IList<Employee> employees;
    }
}