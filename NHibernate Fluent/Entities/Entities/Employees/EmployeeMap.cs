using FluentNHibernate.Mapping;

namespace NHibernateDemo.Entities.Employees
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id).UnsavedValue(-1).GeneratedBy.Native();
            Map(x => x.FirstName).Length(50);
            Map(x => x.LastName).Length(50);
            References(x => x.Manager)
                .LazyLoad();
        }
    }
}