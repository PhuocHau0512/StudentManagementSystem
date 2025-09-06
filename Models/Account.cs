using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class Account
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public int RoleId { get; set; }
        public string Status { get; set; }
        public string BiometricId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Account() { }

        public Account(string userId, string studentId, string username, string passwordHash,
            string salt, int roleId, string status, string biometricId, DateTime createdAt)
        {
            UserId = userId;
            StudentId = studentId;
            Username = username;
            PasswordHash = passwordHash;
            Salt = salt;
            RoleId = roleId;
            Status = status;
            BiometricId = biometricId;
            CreatedAt = createdAt;
        }

        /// <summary>
        /// Factory: Create Account from DataRow (database mapping)
        /// </summary>
        public static Account FromDataRow(System.Data.DataRow row)
        {
            return new Account
            {
                UserId = row["USER_ID"].ToString(),
                StudentId = row["STUDENT_ID"].ToString(),
                Username = row["USERNAME"].ToString(),
                PasswordHash = row["PASSWORD_HASH"].ToString(),
                Salt = row["SALT"].ToString(),
                RoleId = row.Table.Columns.Contains("ROLE_ID") && row["ROLE_ID"] != DBNull.Value ? Convert.ToInt32(row["ROLE_ID"]) : 0,
                Status = row["STATUS"].ToString(),
                BiometricId = row.Table.Columns.Contains("BIOMETRIC_ID") ? row["BIOMETRIC_ID"].ToString() : "",
                CreatedAt = row.Table.Columns.Contains("CREATED_AT") && row["CREATED_AT"] != DBNull.Value
                    ? Convert.ToDateTime(row["CREATED_AT"]) : DateTime.MinValue
            };
        }

        public override string ToString()
        {
            return $"{UserId} | {Username} | {Status} | Role: {RoleId}";
        }
    }
}
