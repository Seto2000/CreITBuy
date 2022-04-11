using CreITBuy.Core.Contracts;
using CreITBuy.Core.ViewModels.JobRequest;
using CreITBuy.Infrastructure.Data.Common;
using CreITBuy.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreITBuy.Core.Services
{
    public class JobRequestService: IJobRequestService
    {
        private readonly IRepo repo;
        private readonly IValidationService validationService;
        public JobRequestService(IRepo _repo,
            IValidationService _validationService)
        {
            repo = _repo;
            validationService = _validationService;
        }
        
        public (bool isAdded,string errors) AddRequest(User fromUser, User toUser, JobRequestViewModel model)
        {
            (bool isValid, string errors) = validationService.ValidateModel(model);
            if (isValid)
            {
                
                
                try
                {
                    var job = new JobRequest()
                    {
                        Theme = model.Theme,
                        Description = model.Description,
                        ToUser = toUser,
                        ToUserId = toUser.Id
                    };
                    job.FromUserJobRequest = new UserJobRequest()
                    {
                        JobRequest = job,
                        JobRequestId = job.Id,
                        FromUser = fromUser,
                        FromUserId = toUser.Id,
                    };
                    toUser.JobRequests.Add(job);
                    repo.Add<JobRequest>(job);
                    repo.SaveChanges();
                    return (true, null);
                }
                catch
                {
                    return (false,"Canot add Request id database! Something went wrong!");
                }
            }
            return (false, errors);
           
        }

        public (bool isRemoved, string errors) Remove(string requestId)
        {
            try
            {
                JobRequest job = repo.All<JobRequest>().Include(x=>x.FromUserJobRequest).SingleOrDefault(x=>x.Id == requestId);
                UserJobRequest userJob = repo.All<UserJobRequest>().SingleOrDefault(x => x.Id == job.FromUserJobRequest.Id);
                repo.Remove<JobRequest>(job);
                repo.Remove<UserJobRequest>(userJob);
                repo.SaveChanges();
                return (true, null);
            }
            catch
            {
                return (false, "Cannot Delete request from database!");
            }
        }
    }
}
