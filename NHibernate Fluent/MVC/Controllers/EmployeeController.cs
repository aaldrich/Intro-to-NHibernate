using System.Linq;
using System.Web.Mvc;
using NHibernateDemo.DataAccess.Employees;
using NHibernateDemo.DataAccess.Managers;
using NHibernateDemo.Entities.Employees;
using NHibernateDemo.MVC.Models;

namespace NHibernateDemo.MVC.Controllers
{
    public class EmployeeController : Controller
    {
        readonly IEmployeeRepository employee_repository;
        readonly IManagerRepository manager_repository;

        public EmployeeController(IEmployeeRepository employee_repository, IManagerRepository manager_repository)
        {
            this.employee_repository = employee_repository;
            this.manager_repository = manager_repository;
        }

        //
        // GET: /Employee/
        public ActionResult Index()
        {
            return View(employee_repository.get_all().ToList());
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(FormCollection collection)
        {
            return
                View("Index",employee_repository.find_by_name(collection["firstName"],collection["lastName"]));
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Employee/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Employee employee = new Employee()
                                        {
                                            Id = -1,
                                            FirstName = collection["firstName"],
                                            LastName = collection["lastName"]
                                        };
                employee_repository.save(employee);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var employee = employee_repository.find_by_id(id);
            employee_repository.delete(employee);
            return RedirectToAction("Index");
        }
        //
        // GET: /Employee/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(new EditViewModel
                            {
                                employee = employee_repository.find_by_id(id),
                                managers = manager_repository.get_all()
                            });
        }

        //
        // POST: /Employee/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Employee employee = employee_repository.find_by_id(id);
                employee.FirstName = collection["firstName"];
                employee.LastName = collection["lastName"];
                employee.Manager = manager_repository.get_by_id(int.Parse(collection["Manager"]));

                employee_repository.save(employee);
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
