using System.Linq;
using Machine.Specifications;
using NHibernate.Linq;
using NHibernateDemo.Entities.Employees;
using NHibernateDemo.Entities.Managers;
using NHibernateDemo.UnitTests.Setup;

namespace NHibernateDemo.UnitTests.Managers
{
    public abstract class manager_concern : base_concern
    {
        Establish c = () =>
        {
            GlobalDataSetup.add_stub_manager_with_2_employees_to_database();
        };
    }

    [Subject("Getting Manager by id")]
    public class when_getting_a_manager_by_id_using_nhibernate_get : manager_concern
    {
        Because b = () =>
                    manager = GlobalDataSetup.Session.Get<Manager>(1);

        It should_return_the_manager_with_matching_id = () => 
                                                        manager.Id.ShouldEqual(1);

        static Manager manager;
    }

    [Subject("Deleting a manager will delete employees if cascade is set to delete")]
    public class when_deleting_a_manager_with_cascade_employees_set_to_delete : manager_concern
    {
        Establish c = () =>
                      manager = GlobalDataSetup.Session.Get<Manager>(1);

        Because b = () => 
        {
            GlobalDataSetup.Session.Delete(manager);
            GlobalDataSetup.Session.Flush();
        };

        It should_delete_all_the_employees_with_the_deleted_manager = () =>
        {
            employee_count = (GlobalDataSetup.Session.Linq<Employee>()
                .Where(x=>x.Manager.Id == 1).Count());
            employee_count.ShouldEqual(0);
        };

        static Manager manager;
        static int employee_count;
    }
}