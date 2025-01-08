using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PlatonStudentApp.DataAccess
{
    public class EnrollmentRepository
    {
        private string connectionString;

        public EnrollmentRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public DataTable GetAvailableCoursesForStudent(int studentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT c.CourseID, c.CourseName, u.FirstName + ' ' + u.LastName AS TeacherName
                FROM Courses c
                LEFT JOIN Users u ON c.TeacherID = u.UserID
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Enrollments e
                    WHERE e.CourseID = c.CourseID AND e.StudentID = @StudentID
                )";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable coursesTable = new DataTable();

                conn.Open();
                adapter.Fill(coursesTable);

                return coursesTable;
            }
        }

        public DataTable GetEnrolledCoursesWithGrades(int studentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT e.CourseID, c.CourseName, e.Grade, u.FirstName + ' ' + u.LastName AS TeacherName
                FROM Enrollments e
                INNER JOIN Courses c ON e.CourseID = c.CourseID
                LEFT JOIN Users u ON c.TeacherID = u.UserID
                WHERE e.StudentID = @StudentID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable enrolledCoursesTable = new DataTable();

                conn.Open();
                adapter.Fill(enrolledCoursesTable);

                return enrolledCoursesTable;
            }
        }

        public bool EnrollStudentInCourse(int studentId, int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM Enrollments WHERE StudentID = @StudentID AND CourseID = @CourseID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@StudentID", studentId);
                checkCmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0) return false; // Already enrolled

                string enrollQuery = "INSERT INTO Enrollments (StudentID, CourseID, EnrollmentDate) VALUES (@StudentID, @CourseID, GETDATE())";
                SqlCommand enrollCmd = new SqlCommand(enrollQuery, conn);
                enrollCmd.Parameters.AddWithValue("@StudentID", studentId);
                enrollCmd.Parameters.AddWithValue("@CourseID", courseId);

                enrollCmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool UnenrollStudentFromCourse(int studentId, int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Enrollments WHERE StudentID = @StudentID AND CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
