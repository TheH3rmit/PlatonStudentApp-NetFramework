using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PlatonStudentApp.DataAccess
{
    public class TeacherRepository
    {
        private string connectionString;

        public TeacherRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public DataTable GetCoursesForTeacher(int teacherId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CourseID, CourseName 
                    FROM Courses 
                    WHERE TeacherID = @TeacherID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable coursesTable = new DataTable();

                conn.Open();
                adapter.Fill(coursesTable);

                return coursesTable;
            }
        }

        public DataTable GetStudentsForCourse(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        e.StudentID,
                        u.FirstName,
                        u.LastName,
                        e.Grade,
                        e.CourseID
                    FROM 
                        Enrollments e
                    INNER JOIN 
                        Users u ON e.StudentID = u.UserID
                    WHERE 
                        e.CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable studentsTable = new DataTable();

                conn.Open();
                adapter.Fill(studentsTable);

                return studentsTable;
            }
        }

        public bool UpdateStudentGrade(int studentId, int courseId, decimal grade)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Enrollments SET Grade = @Grade WHERE StudentID = @StudentID AND CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Grade", grade);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
