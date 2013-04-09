using System;
using CC.Core.DomainTools;
using KnowYourTurf.Core.Services;

namespace SystemSupport.Core
{
    public class SSSessionContext : SessionContext
    {
        private readonly IRepository _repository;

        public SSSessionContext(IRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public void SetClientId(int id)
        {
            AddUpdateSessionItem(new SessionItem {SessionKey = "ClientId", SessionObject = id, TimeStamp = DateTime.Now});
        }

        public override int GetClientId()
        {
            var sessionItem = RetrieveSessionItem("ClientId");
            if (sessionItem!=null) { return (Int32)sessionItem.SessionObject; }
            return 0;
        }
    }
}