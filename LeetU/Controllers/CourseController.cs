﻿using LeetU.Models;
using LeetU.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LeetU.Controllers;

/// <summary>
/// The course controller. All controllers should have MINIMAL code (why do you think minimal apis exist). We take the request and then just perform an action on the service
/// Having minimal code reduces the need to test logic in these controllers, which is pretty tricky.
/// We COULD use MediatR pattern here, but in this example we are just keeping things simple. Google MediatR if you're curious
/// </summary>
[ApiController]
[Route("course")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _courseService.GetCourses();
        return new OkObjectResult(courses);
    }

    [Route("{courseId}")]
    [HttpGet]
    public async Task<IActionResult> GetCourse([FromRoute] long courseId)
    {
        var courses = await _courseService.GetCourses(courseId);
        var course = courses.FirstOrDefault();

        return course == null ? new NotFoundResult() : new OkObjectResult(course);
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> SetCourse([FromBody] Course course)
    {
        var rowsAffected = await _courseService.SetCourseAsync(course);

        if (rowsAffected > 0)
            return new OkResult();

        throw new Exception("There was an error creating the course");
    }
}