using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernateDemo.Entities.Employees;
using NHibernateDemo.Entities.Managers;

namespace NHibernateDemo.MVC.Models
{
    public class EditViewModel
    {
        public Employee employee { get; set; }
        public IEnumerable<Manager> managers { get; set; }
    }
}
