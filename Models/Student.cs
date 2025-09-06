using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string Fullname { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }      // Decrypted at runtime
        public string Phone { get; set; }      // Decrypted at runtime
        public string Address { get; set; }
        public string Major { get; set; }
        public string Class { get; set; }
        public DateTime CreatedAt { get; set; }

        public Student() { }

        public Student(
            string studentId, string fullname, DateTime dob, string gender,
            string email, string phone, string address, string major, string className, DateTime createdAt)
        {
            StudentId = studentId;
            Fullname = fullname;
            DOB = dob;
            Gender = gender;
            Email = email;
            Phone = phone;
            Address = address;
            Major = major;
            Class = className;
            CreatedAt = createdAt;
        }

        /// <summary>
        /// Factory method: Create from DataRow (e.g. for OracleDataAdapter).
        /// Decryption (if needed) should be performed after mapping.
        /// </summary>
        public static Student FromDataRow(System.Data.DataRow row)
        {
            return new Student
            {
                StudentId = row["STUDENT_ID"].ToString(),
                Fullname = row["FULLNAME"].ToString(),
                DOB = row["DOB"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["DOB"]),
                Gender = row["GENDER"].ToString(),
                Email = row.Table.Columns.Contains("EMAIL") ? row["EMAIL"].ToString() : "",
                Phone = row.Table.Columns.Contains("PHONE") ? row["PHONE"].ToString() : "",
                Address = row.Table.Columns.Contains("ADDRESS") ? row["ADDRESS"].ToString() : "",
                Major = row.Table.Columns.Contains("MAJOR") ? row["MAJOR"].ToString() : "",
                Class = row.Table.Columns.Contains("CLASS") ? row["CLASS"].ToString() : "",
                CreatedAt = row.Table.Columns.Contains("CREATED_AT") && row["CREATED_AT"] != DBNull.Value
                    ? Convert.ToDateTime(row["CREATED_AT"]) : DateTime.MinValue
            };
        }

        // Optionally, add ToString or validation helpers below
        public override string ToString()
        {
            return $"{StudentId} | {Fullname} | {Gender} | {Major} | {Class}";
        }
    }
}
