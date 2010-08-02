using System.Linq;
using System.Web.Mvc;
using NHibernateDemo.DataAccess.Managers;
using NHibernateDemo.Entities.Managers;

namespace NHibernateDemo.MVC.Controllers
{
    public class ManagerController : Controller
    {
        readonly IManagerRepository manager_repository;

        public ManagerController(IManagerRepository manager_repository)
        {
            this.manager_repository = manager_repository;
        }

        //
        // GET: /Manager/

        public ActionResult Index()
        {
            return View(manager_repository.get_all().ToList());
        }

        //
        // GET: /Manager/Details/5

        public ActionResult Details(int id)
        {
            return View(manager_repository.get_by_id(id));
        }

        //
        // GET: /Manager/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Manager/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Manager manager = new Manager
                                      {
                                          FirstName = collection["firstName"],
                                          LastName = collection["lastName"],
                                          Id = -1
                                      };
                
                manager_repository.save(manager);
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Manager/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(manager_repository.get_by_id(id));
        }

        //
        // POST: /Manager/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Manager manager = manager_repository.get_by_id(id);
                manager.FirstName = collection["firstName"];
                manager.LastName = collection["lastName"];
                manager_repository.save(manager);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var manager = manager_repository.get_by_id(id);
            manager_repository.delete(manager);
            return RedirectToAction("Index");
        }
    }
}
