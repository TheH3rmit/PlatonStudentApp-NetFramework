using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PlatonStudentApp.DataAccess
{
    public class UserRepository
    {
        private string connectionString;

        public UserRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        // LOGIN
        public (int UserID, string Role) ValidateUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Role FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int userId = Convert.ToInt32(reader["UserID"]);
                    string role = reader["Role"].ToString();
                    return (userId, role);
                }
            }

            return (0, null); // Return 0 and null if the user is not valid
        }

        public int? GetStudentID(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID FROM Users WHERE UserID = @UserID AND Role = 'Student'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                object studentId = cmd.ExecuteScalar();

                if (studentId != null)
                {
                    return Convert.ToInt32(studentId);
                }
            }

            return null; // Return null if no student record is found
        }

        // REGISTER
        public bool IsUsernameTaken(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }


        public bool RegisterUser(string username, string password, string email, string firstName, string lastName, string address, string phoneNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Users 
                                    (Username, Password, Role, Email, FirstName, LastName, Address, PhoneNumber, CreatedDate) 
                                    VALUES 
                                    (@Username, @Password, 'Student', @Email, @FirstName, @LastName, @Address, @PhoneNumber, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // Use hashed password in production
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
        //AdminDashboard
        public DataTable GetAllUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Email, Role, FirstName, LastName, PhoneNumber, Address, AdditionalData FROM Users";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable usersTable = new DataTable();
                conn.Open();
                adapter.Fill(usersTable);
                return usersTable;
            }
        }

        public bool AddUser(string username, string password, string email, string role, string firstName, string lastName, string phoneNumber, string address, string additionalData)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Users 
                                (Username, Password, Email, Role, FirstName, LastName, PhoneNumber, Address, AdditionalData, CreatedDate) 
                                VALUES 
                                (@Username, @Password, @Email, @Role, @FirstName, @LastName, @PhoneNumber, @Address, @AdditionalData, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // Password hashing should be added for production
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@AdditionalData", additionalData);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error adding user: " + ex.Message);
                return false;
            }
        }

        public bool UpdateUser(int userId, string username, string email, string role, string firstName, string lastName, string phoneNumber, string address, string additionalData)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Users 
                                    SET Username = @Username, Email = @Email, Role = @Role, FirstName = @FirstName, LastName = @LastName, 
                                        PhoneNumber = @PhoneNumber, Address = @Address, AdditionalData = @AdditionalData
                                    WHERE UserID = @UserID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@AdditionalData", additionalData);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error updating user: " + ex.Message);
                return false;
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Users WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error deleting user: " + ex.Message);
                return false;
            }
        }
    }
}
