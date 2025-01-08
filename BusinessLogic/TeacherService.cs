using PlatonStudentApp.DataAccess;
using System.Data;

namespace PlatonStudentApp.BusinessLogic
{
    public class TeacherService
    {
        private TeacherRepository teacherRepository;

        public TeacherService()
        {
            teacherRepository = new TeacherRepository();
        }

        public DataTable GetCoursesForTeacher(int teacherId)
        {
            return teacherRepository.GetCoursesForTeacher(teacherId);
        }

        public DataTable GetStudentsForCourse(int courseId)
        {
            return teacherRepository.GetStudentsForCourse(courseId);
        }

        public bool UpdateStudentGrade(int studentId, int courseId, decimal grade)
        {
            return teacherRepository.UpdateStudentGrade(studentId, courseId, grade);
        }
    }
}
