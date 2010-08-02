using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NHibernate;
using NHibernateDemo.DataAccess.Managers;
using NHibernateDemo.Entities.Managers;
using NHibernateDemo.UnitTests.NHibernate_Setup;
using NHibernateDemo.UnitTests.Setup;

namespace NHibernateDemo.UnitTests.Managers
{
    public abstract class manager_repository_concern 
    {
        Establish c = () =>
        {
            helper = new UnitTestNHibernateHelper();
            session = helper.get_new_session_with_clean_database();
            GlobalDataSetup.Session = session;

            manager_repository = new ManagerRepository(session);
        };

        protected static IManagerRepository manager_repository;
        static ISession session;
        static UnitTestNHibernateHelper helper;
    }

    [Subject("Get Manager by Id")]
    public class when_getting_a_manager_by_id : manager_repository_concern
    {
        Establish c = () =>
            GlobalDataSetup.add_stub_manager_to_database();
			
        Because b = () =>
            manager = manager_repository.get_by_id(1);

        It should_return_the_manager_found = () =>
            manager.Id.ShouldEqual(1);

        static Manager manager;
    }

    [Subject("Getting manager that doesnt exist")]
    public class when_getting_a_manager_by_id_that_doesnt_exist : manager_repository_concern
    {
        Because b = () =>
            manager = manager_repository.get_by_id(1);

        It should_return_a_null = () =>
            manager.ShouldBeNull(); 

        static Manager manager;
    }

    [Subject("Saving new manager")]
    public class when_saving_an_non_existing_manager: manager_repository_concern
    {
        Establish context = () =>
        {
            manager = GlobalDataSetup.get_stub_manager_values();
            manager.Id = -1;
        };

        Because of = () =>
            found_manager = manager_repository.save(manager);

        It should_save_the_new_manager_to_the_repository = () =>
            found_manager.Id.ShouldEqual(1);

        static Manager manager;
        static Manager found_manager;
    }

    [Subject("Saving an existing manager")]
    public class when_saving_an_existing_manager: manager_repository_concern
    {
        Establish context = () =>
        {
            GlobalDataSetup.add_stub_manager_to_database();
            existing_manager = GlobalDataSetup.Session.Get<Manager>(1);
            existing_manager.LastName = "Aldrich";
        };

        Because of = () => 
            manager_repository.save(existing_manager);

        It should_update_the_changes_to_the_manager_in_the_repository = () =>
        {
            found_manager = GlobalDataSetup.Session.Get<Manager>(1);
            found_manager.LastName.ShouldEqual("Aldrich");
        };

        static Manager existing_manager;
        static Manager found_manager;
    }

    [Subject("Deleting a Manager")]
    public class when_deleting_a_manager : manager_repository_concern
    {
        Establish context = () =>
        {
            GlobalDataSetup.add_stub_manager_to_database();
            existing_manager = GlobalDataSetup.Session.Get<Manager>(1);
        };

        Because of = () =>
            manager_repository.delete(existing_manager);

        It should_remove_the_manager_from_the_repository = () => 
            GlobalDataSetup.Session.Get<Manager>(1).ShouldBeNull();

        static Manager existing_manager;
    }

    [Subject("Get All Managers")]
    public class when_getting_all_the_managers_from_the_repository : manager_repository_concern
    {
        Establish context = () =>
        {
            GlobalDataSetup.add_stub_manager_to_database();
            GlobalDataSetup.add_stub_manager_to_database();
        };

        Because of = () =>
            managers = manager_repository.get_all();
                        

        It should_return_all_the_managers_in_the_repository = () => 
            managers.Count().ShouldEqual(2);

        static IEnumerable<Manager> managers;
    }
}