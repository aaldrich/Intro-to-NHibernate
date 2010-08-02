using FluentNHibernate.Mapping;

namespace NHibernateDemo.Entities.Managers
{
    public class ManagerMap : ClassMap<Manager>
    {
        public ManagerMap()
        {
            Id(x => x.Id).UnsavedValue(-1).GeneratedBy.Native();
            Map(x => x.FirstName).Length(50);
            Map(x => x.LastName).Length(50);
            HasMany(x => x.Employees)
                .LazyLoad()
                .Cascade
                .All();
        }
    }
}
