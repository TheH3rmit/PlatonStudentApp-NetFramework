using PlatonStudentApp.DataAccess;
using System.Data;

namespace PlatonStudentApp.BusinessLogic
{
    public class EnrollmentService
    {
        private EnrollmentRepository enrollmentRepository;

        //StudentDashboard
        public EnrollmentService()
        {
            enrollmentRepository = new EnrollmentRepository();
        }

        public DataTable GetAvailableCoursesForStudent(int studentId)
        {
            return enrollmentRepository.GetAvailableCoursesForStudent(studentId);
        }

        public DataTable GetEnrolledCoursesWithGrades(int studentId)
        {
            return enrollmentRepository.GetEnrolledCoursesWithGrades(studentId);
        }

        public bool EnrollStudentInCourse(int studentId, int courseId)
        {
            return enrollmentRepository.EnrollStudentInCourse(studentId, courseId);
        }

        public bool UnenrollStudentFromCourse(int studentId, int courseId)
        {
            return enrollmentRepository.UnenrollStudentFromCourse(studentId, courseId);
        }
    }
}
