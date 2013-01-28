using System.Collections.Generic;

namespace SystemSupport.Web.Models
{
    using CC.Core.CoreViewModelAndDTOs;

    public class BulkActionViewModel : ViewModel
    {
        public IEnumerable<int> EntityIds { get; set; }
    }
}