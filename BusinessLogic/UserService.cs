using System;
using System.Data.SqlClient;
using System.Configuration;
using PlatonStudentApp.DataAccess;
using System.Data;

namespace PlatonStudentApp.BusinessLogic
{
    public class UserService
    {
        private UserRepository userRepository;

        public UserService()
        {
            userRepository = new UserRepository();
        }

        //Login
        public (int UserID, string Role) ValidateUser(string username, string password)
        {
            return userRepository.ValidateUser(username, password);
        }

        public int? GetStudentID(int userId)
        {
            return userRepository.GetStudentID(userId);
        }

        //Register
        public bool IsUsernameTaken(string username)
        {
            return userRepository.IsUsernameTaken(username);
        }

        public bool RegisterUser(string username, string password, string email, string firstName, string lastName, string address, string phoneNumber)
        {
            return userRepository.RegisterUser(username, password, email, firstName, lastName, address, phoneNumber);
        }

        //AdminDAshboard

        public DataTable GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }

        public bool AddUser(string username, string password, string email, string role, string firstName, string lastName, string phoneNumber, string address, string additionalData)
        {
            return userRepository.AddUser(username, password, email, role, firstName, lastName, phoneNumber, address, additionalData);
        }

        public bool UpdateUser(int userId, string username, string email, string role, string firstName, string lastName, string phoneNumber, string address, string additionalData)
        {
            return userRepository.UpdateUser(userId, username, email, role, firstName, lastName, phoneNumber, address, additionalData);
        }

        public bool DeleteUser(int userId)
        {
            return userRepository.DeleteUser(userId);
        }

        //
    }
}