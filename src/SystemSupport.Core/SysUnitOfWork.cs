namespace SystemSupport.Core
{
    using CC.Core.DomainTools;
    using NHibernate;

    public class SysUnitOfWork : UnitOfWork
    {

        public SysUnitOfWork(ISession session) : base(session)
        {
            _session = session;
            var enableDeletdFilter = _session.EnableFilter("IsDeletedConditionFilter");
            var enableStatusFilter = _session.EnableFilter("StatusConditionFilter");
            if (enableDeletdFilter != null) enableDeletdFilter.SetParameter("IsDeleted", false);
            if (enableStatusFilter != null) enableStatusFilter.SetParameter("condition", "Active");
        }
    }

}