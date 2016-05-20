using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.SqlServer;
using System;
using Exon.Recab.Infrastructure.Exception;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Exon.Recab.Service.Model.UserModel;
using Exon.Recab.Infrastructure.Utility.Security;
using Exon.Recab.Domain.Constant.Resource;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Domain.Constant.CS.Exception;

namespace Exon.Recab.Service.Implement.User
{
    public class RoleManagementService
    {
        private readonly SdbContext _sdb;

        internal RoleManagementService(ref SdbContext sdb)
        {
            _sdb = sdb;
        }

        public RoleManagementService()
        {
            _sdb = new SdbContext();
        }

        #region PRIVETE METHODS
        private List<ResourceViewModel> BuildTree(List<ResourceViewModel> items)
        {
            items.ForEach(i => i.childs = items.Where(ch => ch.parentId == i.id).ToList());
            return items.Where(i => i.parentId == null).ToList();
        }
        #endregion

        #region ADD

        public bool AddRole(string name)
        {
            Role Role = new Role
            {
                Title = name ?? " ",
                Status = RoleStatus.Deactive
            };

            _sdb.Roles.Add(Role);

            _sdb.SaveChanges();



            return true;

        }

        public bool AddRoleToUser(List<long> roleIds, long userId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            List<Role> ListRole = new List<Role>();

            foreach (var item in roleIds)
            {
                Role role = _sdb.Roles.Find(item);

                if (role == null)
                    throw new RecabException((int)ExceptionType.RoleNotFound);

                ListRole.Add(role);
            }

            foreach (var item in ListRole)
            {
                _sdb.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = item.Id, Status = RoleStatus.Active });
            }


            _sdb.SaveChanges();


            return true;
        }

        public bool AddResource(string title, string url, int type, long? parentId)
        {
            if (parentId.HasValue)
            {
                if (_sdb.Resources.Find(parentId.Value) == null)
                    throw new RecabException((int)ExceptionType.ResourceParentNotFound);
            }

            Domain.Entity.Resource Resource = new Domain.Entity.Resource
            {
                Key = CodeHelper.NewKey(),
                Type = (ResourceType)type,
                Url = url ?? " ",
                Title = title ?? " ",
                ParentId = parentId
            };

            _sdb.Resources.Add(Resource);


            _sdb.SaveChanges();


            return true;

        }

        public bool AddPermission(long roleId, long ResourceId)
        {
            Domain.Entity.Resource resource = _sdb.Resources.Find(ResourceId);
            if (resource == null)
                throw new RecabException((int)ExceptionType.ResourceNotFound);

            Role role = _sdb.Roles.Find(roleId);
            if (role == null)
                throw new RecabException((int)ExceptionType.RoleNotFound);

            Permission permission = new Permission { ResourceId = resource.Id, RoleId = role.Id };

            _sdb.Permission.Add(permission);


            _sdb.SaveChanges();


            return true;
        }

        public bool SetPermissions(long roleId, List<long> resourceIds)
        {
            bool result = false;
            var permissions = _sdb.Permission.Where(w => w.RoleId == roleId);
            _sdb.Permission.RemoveRange(permissions);

            try
            {
                _sdb.SaveChanges();
                result = true;
            }
            catch (Exception eX)
            {
                throw new RecabException(eX.Message);
            }

            if (resourceIds != null)
            {
                foreach (var item in resourceIds)
                {
                    result = AddPermission(roleId, item);
                }
            }

            return result;
        }
        #endregion

        #region  edit
        public bool EditRole(long roleId, string title)
        {
            var role = _sdb.Roles.FirstOrDefault(fod => fod.Id == roleId);

            if (role == null)
                throw new RecabException((int)ExceptionType.RoleNotFound);

            role.Title = title;

            _sdb.SaveChanges();

            return true;
        }

        public bool ChangeRoleStatus(long roleId, int status)
        {


            var role = _sdb.Roles.FirstOrDefault(fod => fod.Id == roleId);
            if (role == null)
                throw new RecabException((int)ExceptionType.RoleNotFound);

            role.Status = (RoleStatus)status;

            _sdb.SaveChanges();

            return true;
        }

        public bool EditUse(long userId, string name, string last, string email, string mobile, int status, int gender)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            UserStatus userStatus = (UserStatus)status;
            UserGender userGender = (UserGender)gender;

            user.FirstName = name;
            user.LastName = last;
            user.Mobile = mobile;
            user.Status = userStatus;
            user.Email = email;
            user.GenderType = userGender;

            _sdb.SaveChanges();


            return true;
        }

        public bool ChangeUserStatus(long userId, int status)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            UserStatus userStatus = (UserStatus)status;

            user.Status = userStatus;


            _sdb.SaveChanges();


            return true;

        }


        #endregion

        #region Report
        public List<RoleDetailViewModel> ListRols(ref long count, int size = 1, int skip = 0)
        {
            var roles = _sdb.Roles;

            count = roles.Count();
            if (roles.Count() == 0)
                return new List<RoleDetailViewModel>();

            return roles.OrderBy(r => r.Id).Skip(size * skip).Take(size)
                .Select(r => new RoleDetailViewModel { id = r.Id, title = r.Title, status = r.Status.ToString() }).ToList();
        }

        public List<RoleDetailViewModel> UserRols(long userId)
        {

            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            List<UserRole> userRole = _sdb.UserRoles.Where(ur => ur.UserId == user.Id).ToList();

            if (userRole.Count() == 0)
                return new List<RoleDetailViewModel>();

            return userRole.Select(ur => new RoleDetailViewModel { id = ur.Role.Id, title = ur.Role.Title, status = ur.Role.Status.ToString() }).ToList();

        }

        public List<UserDetailAdminViewModel> ListUser(ref long count, string name, string email, long? roleId, int size = 1, int skip = 0)
        {

            List<UserDetailAdminViewModel> model = new List<UserDetailAdminViewModel>();

            List<Domain.Entity.User> userList = _sdb.Users.ToList();
            if (roleId.HasValue)
            {
                Role role = _sdb.Roles.Find(roleId.Value);

                if (role != null)
                    userList = _sdb.Users.Where(u => u.UserRoles.Any(r => r.RoleId == role.Id)).ToList();
            }

            if (name != null && name != "")
                userList = userList.Where(u => (u.FirstName + u.LastName).Contains(name)).ToList();

            if (email != null && email != "")
                userList = userList.Where(u => u.Email.Contains(email)).ToList();


            count = userList.Count;
            if (count == 0)
                return model;


            var temp = new List<UserDetailAdminViewModel>();

            foreach (var u in userList.OrderBy(u => u.Id).Skip(size * skip).Take(size))
            {
                temp.Add(new UserDetailAdminViewModel
                {
                    cumUserId = u.Id,
                    name = u.FirstName + " " + u.LastName,
                    email = u.Email,
                    mobile = u.Mobile,
                    status = (int)u.Status,
                    credit = u.Credit.Count() > 0 ? u.Credit.Sum(c => c.Amount) : 0

                });
            }

            return temp;

        }

        public UserDetailViewModel UserDetail(long userId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            return new UserDetailViewModel
            {
                cumUserId = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                status = (int)user.Status,
                gender = (int)user.GenderType,
                mobile = user.Mobile,
                email = user.Email
            };

        }

        public List<PermissionDetailViewModel> ResourcePermissionDetail(long userId, long resourceId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            List<Permission> ListPermission = _sdb.UserRoles.Join(_sdb.Permission,
                                                                    ur => ur.RoleId,
                                                                    p => p.RoleId,
                                                                    (ur, p) => (p)).ToList();

            if (!ListPermission.Any(p => p.ResourceId == resourceId))
                throw new RecabException((int)ExceptionType.AccessDenied);

            Permission permission = ListPermission.First(p => p.ResourceId == resourceId);

            ListPermission = ListPermission.Where(p => p.Resource.ParentId.HasValue && p.Resource.ParentId.Value == permission.ResourceId).ToList();

            List<PermissionDetailViewModel> model = new List<PermissionDetailViewModel>();
            foreach (var item in ListPermission)
            {
                if (!model.Any(m => m.permission == item.Id.ToString()))
                    model.Add(new PermissionDetailViewModel
                    {
                        permission = item.Id.ToString(),
                        url = item.Resource.Url,
                        title = item.Resource.Title
                    });
            }


            return model;
        }

        public List<UserResourceViewModel> GetUserResources(long userId, long resourceId)
        {
            var user = _sdb.Users.Find(userId);

            if (user == null)
            {
                throw new RecabException((int)ExceptionType.UserNotFound);
            }

            var result = (from userRole in _sdb.UserRoles
                          join permission in _sdb.Permission
                               on userRole.RoleId equals permission.RoleId
                          join resource in _sdb.Resources
                               on permission.ResourceId equals resource.Id
                          where userRole.UserId == userId &&
                               (resource.Id == resourceId || resource.ParentId == resourceId)
                          select new UserResourceViewModel
                          {
                              id = resource.Id,
                              title = resource.Title,
                              url = resource.Url
                          }).Distinct();

            return result.ToList();
        }

        public List<ResourceViewModel> ListResources()
        {
            var result = new List<ResourceViewModel>();

            var resources = _sdb.Resources;

            var mapped = resources.Select(item => new ResourceViewModel
            {
                id = item.Id,
                title = item.Title,
                url = item.Url,
                type = (int)item.Type,
                parentId = item.ParentId,
                key = item.Key,
            }).ToList();

            result = BuildTree(mapped);


            return result;
        }

        public List<ResourceViewModel> GetResourceByRole(long roleId)
        {
            var result = new List<ResourceViewModel>();
            ///must be lumbada  ad coorect senario
            try
            {
                var query = from permisssion in _sdb.Permission
                            join resource in _sdb.Resources
                            on permisssion.ResourceId equals resource.Id
                            where permisssion.RoleId == roleId
                            select new ResourceViewModel
                            {
                                id = resource.Id,
                                title = resource.Title,
                                url = resource.Url,
                                type = (int)resource.Type,
                                parentId = resource.ParentId,
                                key = resource.Key
                            };

                if (query != null)
                {
                    result = query.ToList();
                }
                else
                {
                    throw new RecabException("Role not found or there is no permission has set for it.", HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception eX)
            {
                throw new RecabException(eX.Message, HttpStatusCode.InternalServerError);
            }

            return result;
        }
        #endregion

        #region Delete

        public bool DeleteRole(long roleId)
        {
            Role role = _sdb.Roles.Find(roleId);
            if (role == null)
                throw new RecabException((int)ExceptionType.RoleNotFound);

            if (_sdb.Permission.Any(p => p.RoleId == role.Id))
                throw new RecabException((int)ExceptionType.RoleHasPermission);

            if (_sdb.UserRoles.Any(ur => ur.RoleId == role.Id))
                throw new RecabException((int)ExceptionType.RoleHasUserRole);

            _sdb.Roles.Remove(role);


            _sdb.SaveChanges();


            return true;

        }

        public bool DeleteUserRole(long userId, long roleId)
        {
            UserRole userRole = _sdb.UserRoles.FirstOrDefault(ur => ur.RoleId == roleId && ur.UserId == userId);
            if (userRole == null)
                throw new RecabException((int)ExceptionType.UserRoleNotFound);

            _sdb.UserRoles.Remove(userRole);

            _sdb.SaveChanges();


            return true;

        }

        public bool DeletePermission(long permissionId)
        {
            Permission permission = _sdb.Permission.Find(permissionId);
            if (permission == null)
                throw new RecabException((int)ExceptionType.PermissionNotFound);

            _sdb.Permission.Remove(permission);


            _sdb.SaveChanges();


            return true;

        }
        #endregion
    }
}
