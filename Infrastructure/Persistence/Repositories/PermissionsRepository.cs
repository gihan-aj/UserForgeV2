using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Domain.Permissions;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using Application.Shared.Pagination;
using Application.Permissions.Queries.GetAll;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly IApplicationDbContext _context;

        public PermissionsRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Permission permission)
        {
            _context.Permissions.Add(permission);
        }

        public void Remove(Permission permission)
        {
            _context.Permissions.Remove(permission);
        }

        public void Update(Permission permission)
        {
            _context.Permissions.Update(permission);
        }

        public async Task<bool> NameExistsAsync(string permissionName)
        {
            return await _context.Permissions.AnyAsync(p => p.Name == permissionName);
        }

        public async Task<Permission?> GetByIdAsync(string id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task<PaginatedList<GetAllPermissionsResponse>> GetAllAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<Permission> permissionsQuery = _context.Permissions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                permissionsQuery = permissionsQuery
                    .Where(p => p.Name.Contains(searchTerm));
            }

            if (sortOrder?.Trim().ToLower() == "desc")
            {
                permissionsQuery = permissionsQuery
                    .OrderByDescending(GetSortProperty(sortColumn));
            }
            else
            {
                permissionsQuery = permissionsQuery
                    .OrderBy(GetSortProperty(sortColumn));
            }

            var permissionsResponseQuery = permissionsQuery
                .Select(p => new GetAllPermissionsResponse(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.IsActive));

            var permissions = await PaginatedList<GetAllPermissionsResponse>.CreateAsync(
                permissionsResponseQuery, 
                page,
                pageSize,
                cancellationToken);

            return permissions;
        }


        private static Expression<Func<Permission, object>> GetSortProperty(string? sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "name" => permission => permission.Name!,
                "id" => permission => permission.Id,
                _ => permission => permission.Id
            };
        }
    }
}
