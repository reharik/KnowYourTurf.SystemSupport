using System.Web;
using KnowYourTurf.Core.Services;

namespace SystemSupport.Core
{
    using CC.Core.DomainTools;
    using NHibernate;

    public class SysUnitOfWork : UnitOfWork
    {

        public SysUnitOfWork(ISession session) : base(session)
        {
            var clientId = 0;
            if (HttpContext.Current.Session!=null && HttpContext.Current.Session["ClientId"] !=null)
            {
                clientId = (int)((SessionItem)HttpContext.Current.Session["ClientId"]).SessionObject;
            }
            _session = session;
            var enableDeletdFilter = _session.EnableFilter("IsDeletedConditionFilter");
            var enableStatusFilter = _session.EnableFilter("StatusConditionFilter");
            var enableCoFilter = _session.EnableFilter("ClientConditionFilter");
            if (enableCoFilter != null) enableCoFilter.SetParameter("ClientId", clientId);
            if (enableDeletdFilter != null) enableDeletdFilter.SetParameter("IsDeleted", false);
            if (enableStatusFilter != null) enableStatusFilter.SetParameter("condition", "Active");
        }
    }

}