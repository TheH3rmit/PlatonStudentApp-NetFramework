using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PlatonStudentApp.DataAccess
{
    public class CourseRepository
    {
        private string connectionString;

        public CourseRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public DataTable GetAllCourses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT 
                    c.CourseID, 
                    c.CourseName, 
                    c.Description, 
                    c.Credits, 
                    c.StartDate, 
                    c.EndDate, 
                    c.TeacherID, 
                    u.FirstName + ' ' + u.LastName AS TeacherName
                FROM 
                    Courses c
                LEFT JOIN 
                    Users u ON c.TeacherID = u.UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable coursesTable = new DataTable();

                conn.Open();
                adapter.Fill(coursesTable);

                return coursesTable;
            }
        }

        public DataTable GetTeachers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, FirstName + ' ' + LastName AS FullName FROM Users WHERE Role = 'Teacher'";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable teachersTable = new DataTable();

                conn.Open();
                adapter.Fill(teachersTable);

                return teachersTable;
            }
        }

        public void AddCourse(string courseName, string description, int credits, DateTime startDate, DateTime endDate, int teacherId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO Courses (CourseName, Description, Credits, StartDate, EndDate, TeacherID) 
                    VALUES (@CourseName, @Description, @Credits, @StartDate, @EndDate, @TeacherID)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseName", courseName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Credits", credits);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCourse(int courseId, string courseName, string description, int credits, DateTime startDate, DateTime endDate, int teacherId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE Courses 
                    SET CourseName = @CourseName, 
                        Description = @Description, 
                        Credits = @Credits, 
                        StartDate = @StartDate, 
                        EndDate = @EndDate, 
                        TeacherID = @TeacherID 
                    WHERE CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Parameters.AddWithValue("@CourseName", courseName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Credits", credits);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCourse(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Delete related enrollments
                string deleteEnrollmentsQuery = "DELETE FROM Enrollments WHERE CourseID = @CourseID";
                using (SqlCommand cmd = new SqlCommand(deleteEnrollmentsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.ExecuteNonQuery();
                }

                // Delete the course
                string deleteCourseQuery = "DELETE FROM Courses WHERE CourseID = @CourseID";
                using (SqlCommand cmd = new SqlCommand(deleteCourseQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
