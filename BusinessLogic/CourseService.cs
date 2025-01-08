using PlatonStudentApp.DataAccess;
using System;
using System.Data;

namespace PlatonStudentApp.BusinessLogic
{
    public class CourseService
    {
        private CourseRepository courseRepository;

        public CourseService()
        {
            courseRepository = new CourseRepository();
        }

        public DataTable GetAllCourses()
        {
            return courseRepository.GetAllCourses();
        }

        public DataTable GetTeachers()
        {
            return courseRepository.GetTeachers();
        }

        public void AddCourse(string courseName, string description, int credits, DateTime startDate, DateTime endDate, int teacherId)
        {
            courseRepository.AddCourse(courseName, description, credits, startDate, endDate, teacherId);
        }

        public void UpdateCourse(int courseId, string courseName, string description, int credits, DateTime startDate, DateTime endDate, int teacherId)
        {
            courseRepository.UpdateCourse(courseId, courseName, description, credits, startDate, endDate, teacherId);
        }

        public void DeleteCourse(int courseId)
        {
            courseRepository.DeleteCourse(courseId);
        }
    }
}
