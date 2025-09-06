using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public Role() { }

        public Role(int roleId, string roleName, string description)
        {
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
        }

        /// <summary>
        /// Create a Role from a DataRow (for database mapping)
        /// </summary>
        public static Role FromDataRow(System.Data.DataRow row)
        {
            return new Role
            {
                RoleId = row.Table.Columns.Contains("ROLE_ID") && row["ROLE_ID"] != DBNull.Value
                    ? Convert.ToInt32(row["ROLE_ID"]) : 0,
                RoleName = row["ROLE_NAME"].ToString(),
                Description = row.Table.Columns.Contains("DESCRIPTION") ? row["DESCRIPTION"].ToString() : ""
            };
        }

        public override string ToString()
        {
            return $"{RoleId} | {RoleName} | {Description}";
        }
    }
}
