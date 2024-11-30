using LeetU.Models;

namespace LeetU.Services.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetStudents(params long[] studentIds);
    Task<IEnumerable<StudentWithCourses>> GetStudentsWithCourses(params long[] studentIds);
    Task<int> SetStudentCourseAsync(long studentId, long courseId);
    Task<bool> HasCourse(long studentId, long courseId);
}