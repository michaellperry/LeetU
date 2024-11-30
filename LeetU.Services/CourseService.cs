using LeetU.Data.Interfaces;
using LeetU.Models;
using LeetU.Services.Interfaces;
using LeetU.Services.Mappers;

namespace LeetU.Services;

/// <summary>
/// The course service. Contains the BUSINESS LOGIC for anything to do with courses. This includes marshalling the data access for controllers to the data layer.
/// We use mappers to map Entities to Models and vice versa. We do not send entities over the wire, we always transform them tro models
/// </summary>
public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<Course>> GetCourses(params long[] courseIds)
    {
        var courses = _courseRepository.Get(c => courseIds.Any(id => c.Id == id) || courseIds.Length == 0);

        return courses.Select(EntityToModel.CreateCourseFromEntity);
    }

    public async Task<int> SetCourseAsync(Course course)
    {
        var entity = ModelToEntity.CreateEntityFromCourse(course);
        await _courseRepository.InsertAsync(entity);
        var rowsAffected = await _courseRepository.SaveChangesAsync();
        return rowsAffected;
    }
    
    public async Task<bool> UpdateCourseAsync(Course course)
    {
        var entity = ModelToEntity.UpdateEntityFromCourse(_courseRepository.Get(c => c.Id == course.Id).FirstOrDefault()!, course);
        _courseRepository.Update(entity);
        var rowsAffected = await _courseRepository.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteCourseAsync(long courseId)
    {
        var entity = _courseRepository.Get(c => c.Id == courseId).FirstOrDefault();
        if (entity == null)
            return false;

        if (entity.StudentCourses.Any())
            throw new Exception("Cannot delete a course that has students enrolled in it");

        _courseRepository.Delete(entity);
        var rowsAffected = await _courseRepository.SaveChangesAsync();
        return rowsAffected > 0;
    }
}