using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using Exon.Recab.Infrastructure.Exception;
using System.Net;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Service.Resource;
using Exon.Recab.Domain.Constant.User;

namespace Exon.Recab.Service.Implement.PolicySystemConfig
{
    public class UserPolicyService
    {
        public RecabSystemConfig SystemConfig;

        public SdbContext _sdb;

        internal UserPolicyService(ref SdbContext sdb)
        {
            _sdb = sdb;

            SystemConfig = _sdb.RecabSystemConfig.First();

            if (SystemConfig == null)
                throw new RecabException("SQL Managment Error", HttpStatusCode.ExpectationFailed);
        }


        public UserPolicyService()
        {
            _sdb = new SdbContext();

            SystemConfig = _sdb.RecabSystemConfig.First();

            if (SystemConfig == null)
                throw new RecabException("SQL Managment Error", HttpStatusCode.ExpectationFailed);
        }

        public void EnforcementChangeStatusDealership(ref Dealership dealership)
        {
            long userId = dealership.UserId;

            bool roleExist = _sdb.UserRoles.Any(ur => ur.UserId == userId && ur.Role.Title == PolicyConfig.Dealership);

            if (!roleExist && dealership.Status == DealershipStatus.فعال)
            {

                Role role = _sdb.Roles.FirstOrDefault(r => r.Title == PolicyConfig.Dealership);

                if (role == null)
                { dealership.User.UserRoles.RemoveAll(c => true); }

                _sdb.UserRoles.Add(new UserRole { UserId = userId, RoleId = role.Id });
                _sdb.SaveChanges();
                return;
            }

            //if (!roleExist && dealership.Status == DealershipStatus.غیر_فعال)
            //    throw new RecabException("system Config has error  for userId # " + userId.ToString());

            //if (roleExist && dealership.Status == DealershipStatus.غیر_فعال) 
            //{
            //    if (_sdb.Dealerships.Where(d => d.UserId == userId).Count() == 1)
            //    {

            //        UserRole userRole = _sdb.UserRoles.FirstOrDefault(ur => ur.UserId == userId && ur.Role.Title == PolicyConfig.Dealership);

            //        _sdb.UserRoles.Remove(userRole);
            //        _sdb.SaveChanges();
            //        return;
            //    }

            //}

        }

    }
}
