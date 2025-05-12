using AuthenticationAuthorization.Application.DTOs.BranchDTOs;
using AuthenticationAuthorization.Application.DTOs.CompanyDTOs;
using AuthenticationAuthorization.Application.DTOs.DepartmentDTOs;
using AuthenticationAuthorization.Application.DTOs.EmployeeDTOs;
using AuthenticationAuthorization.Application.DTOs.MenuDTOs;
using AuthenticationAuthorization.Application.DTOs.MenuPermissionDTOs;
using AuthenticationAuthorization.Application.DTOs.PermissionDTOs;
using AuthenticationAuthorization.Application.DTOs.RoleDTOs;
using AuthenticationAuthorization.Application.DTOs.RoleMenuPermissionDTOs;
using AuthenticationAuthorization.Application.DTOs.StaticDataDetailDTOs;
using AuthenticationAuthorization.Application.DTOs.StaticDataTypeDTOs;
using AuthenticationAuthorization.Application.DTOs.UserDTOs;
using AuthenticationAuthorization.Application.DTOs.UserRoleDTOs;
using AuthenticationAuthorization.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Employee mappings
            CreateMap<AddEmployeeDTO, Employee>();
            CreateMap<Employee, GetEmployeeDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => string.Join(" ", new[] { src.FirstName, src.MiddleName, src.LastName }
                            .Where(x => !string.IsNullOrEmpty(x)))))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.BranchName));
            CreateMap<UpdateEmployeeDTO, Employee>();

            // Static Data Type mappings
            CreateMap<AddStaticDataTypeDTO, StaticDataType>();
            CreateMap<StaticDataType, GetStaticDataTypeDTO>(); // StaticDataDetail to GetStaticDataTypeDTO
            CreateMap<UpdateStaticTypeDTO, StaticDataType>();

            //static Data Detail
            CreateMap<AddStaticDataDetailDTO, StaticDataDetail>();
            CreateMap<StaticDataDetail, GetStaticDataDetailDTO>();
            CreateMap<UpdateStaticDataDetailDTO, StaticDataDetail>();

            //company
            CreateMap<AddCompanyDTO, Company>();
            CreateMap<Company, GetCompanyDTO>();
            CreateMap<UpdateCompanyDTO, Company>();

            //branch
            CreateMap<AddBranchesDTO, Branch>();
            CreateMap<Branch, GetBranchesDTO>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName));
            CreateMap<UpdateBranchesDTO, Branch>();

            //dept
            CreateMap<AddDepartmentsDTO, Department>();
            CreateMap<Department, GetDepartmentsDTO>()
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.BranchName));
            CreateMap<UpdateDepartmentsDTO, Department>();

            //menu
            CreateMap<AddMenuDTO, Menu>();
            CreateMap<Menu, GetMenuDTO>();
            CreateMap<UpdateMenuDTO, Menu>();

            //user
            CreateMap<User, GetUserDTO>();


            //permission
            CreateMap<Permission, GetPermissionDTO>();
            CreateMap<AddPermissionDTO, Permission>();
            CreateMap<UpdatePermisionDTO, Permission>();

            //menupermission
            CreateMap<MenuPermission, GetMenuPermissionDTO>()
                .ForMember(dest => dest.Menu, opt => opt.MapFrom(src => src.Menu.MenuName))
                .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission.PermissionName));
            CreateMap<AddMenuPermissionDTO, MenuPermission>();
            CreateMap<UpdateMenuPermissionDTO, MenuPermission>();

            //role
            CreateMap<AddRoleDTO, Role>();
            CreateMap<Role, GetRoleDTO>();
            CreateMap<UpdateRoleDTO, Role>();

            //roleMenupermission
            CreateMap<RoleMenuPermission, GetRoleMenuPermissionDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<AddRoleMenuPermissionDTO, RoleMenuPermission>();
            CreateMap<UpdateRoleMenuPermissionDTO, RoleMenuPermission>();

            //userrole
            CreateMap<UserRole, GetUserRoleDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<AddUserRoleDTO, UserRole>();
            CreateMap<UpdateUserRoleDTO, UserRole>();

        }
    }
}
