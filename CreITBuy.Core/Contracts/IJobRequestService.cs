using CreITBuy.Core.ViewModels.JobRequest;
using CreITBuy.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Core.Contracts
{
    public interface IJobRequestService
    {
        public (bool isAdded, string errors) AddRequest(User fromUser, User toUser, JobRequestViewModel model);
        (bool isRemoved, string errors) Remove(string requestId);
    }
}
