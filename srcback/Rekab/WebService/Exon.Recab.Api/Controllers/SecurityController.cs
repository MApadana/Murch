using Exon.Recab.Api.Models.Role;
using Exon.Recab.Service.Implement.User;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Net.Http;
using System.Web.Http;

namespace Exon.Recab.Api.Controllers
{
    public class SecurityController : ApiController
    {
        private readonly RoleManagementService _roleService;

        public SecurityController()
        {
            _roleService = new RoleManagementService();
        }

        #region Add
        [HttpPost]
        public HttpResponseMessage AddRole(AddRoleModel model)
        {

            return _roleService.AddRole(name: model.title).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddResource(AddResourceModel model)
        {

            return _roleService.AddResource(title: model.title,
                                            url: model.url,
                                            type: ((int)model.type),
                                            parentId: model.parentId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddUserRole(AddUserRoleModel model)
        {

            return _roleService.AddRoleToUser(userId: model.cumUserId, roleIds: model.roleIds).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddPermission(AddPermissionModel model)
        {
            return _roleService.AddPermission(roleId: model.roleId, ResourceId: model.resourceId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage SetPermissions(AddPermissionsModel model)
        {
            return _roleService.SetPermissions(roleId: model.roleId, resourceIds: model.resourceIds).GetHttpResponse();
        }
        #endregion

        #region Report
        [HttpPost]
        public HttpResponseMessage ListUserRole(AdminFindUserModel model)
        {

            return _roleService.UserRols(userId: model.cumUserId).GetHttpResponseWithCount(10);
        }

        [HttpPost]
        public HttpResponseMessage ListRole(RoleFindModel model)
        {
            long count = 0;
            return _roleService.ListRols(ref count, model.pageSize, model.pageIndex).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage ListUser(AdminUserSearchModel model)
        {
            long count = 0;

            return _roleService.ListUser(count: ref count,
                                                size: model.pageSize,
                                                skip: model.pageIndex,
                                                roleId: model.roleId,
                                                name: model.name,
                                                email: model.email).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage UserDetail(AdminFindUserModel model)
        {
            return _roleService.UserDetail(model.cumUserId).GetHttpResponse();


        }

        [HttpPost]
        public HttpResponseMessage AccessDetail(AccessDetailModel model)
        {
            return _roleService.GetUserResources(userId: model.userId, resourceId: model.resourceId).GetHttpResponse();

            //Commented by hamèd
            //return _roleService.ResourcePermissionDetail(userId: model.userId, resourceId: model.resourceId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ResourcePermissonDetail(DetailPermissionModel model)
        {
            return _roleService.ResourcePermissionDetail(userId: model.userId, resourceId: model.resourceId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ListResources()
        {
            return _roleService.ListResources().GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage GetResourceByRole(GetRoleModel model)
        {
            return _roleService.GetResourceByRole(roleId: model.roleId).GetHttpResponse();
        }
        #endregion

        #region Edit
        [HttpPost]
        public HttpResponseMessage EditRole(EditRoleModel model)
        {
            return _roleService.EditRole(roleId: model.roleId, title: model.title).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ChangeRoleStatus(RoleChangeStatusModel model)
        {
            return _roleService.ChangeRoleStatus(roleId: model.roleId, status: model.status).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage EditUser(UserEditModel model)
        {

            return _roleService.EditUse(userId: model.cumUserId,
                                        name: model.firstName,
                                        last: model.lastName,
                                        email: model.email,
                                        mobile: model.mobile,
                                        status: model.status,
                                        gender: model.gender).GetHttpResponse();
        }


        [HttpPost]
        public HttpResponseMessage ChengStatus(UserChangeStatusModel model)
        {

            return _roleService.ChangeUserStatus(userId: model.cumUserId, status: model.status).GetHttpResponse();
        }


        #endregion

        #region Delete
        [HttpPost]
        public HttpResponseMessage DeleteRole(DeleteRoleModel model)
        {
            return _roleService.DeleteRole(roleId: model.roleId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteUserRole(UserRoleDeleteModel model)
        {
            return _roleService.DeleteUserRole(model.cumUserId, model.roleId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeletePermission(DeletePermissionModel model)
        {
            return _roleService.DeletePermission(permissionId: model.permissionId).GetHttpResponse();
        }
        #endregion
    }
}
