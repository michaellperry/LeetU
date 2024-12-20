﻿using System;
using System.Linq;
using System.Threading.Tasks;
using LeetU.Data.Repositories;
using LeetU.Data.Tests.DataContext;
using LeetU.Models;
using Xunit;

namespace LeetU.Services.Tests;

public class CourseServiceTests : IClassFixture<InMemoryDbContext>
{
    private readonly InMemoryDbContext _context;

    public CourseServiceTests(InMemoryDbContext context)
    {
        _context = context;
    }

    [Fact]
    public async void ShouldGetCourses()
    {
        //Arrange
        _context.Reset();
        var sut = new CourseService(new CourseRepository(_context.StudentContext));

        //Act
        var courses = await sut.GetCourses();

        //Act
        Assert.Equal(_context.StudentContext.Courses.Count(), courses.Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async void ShouldGetCoursesById(int courseId)
    {
        //Arrange
        _context.Reset();
        var sut = new CourseService(new CourseRepository(_context.StudentContext));

        //Act
        var courses = await sut.GetCourses(courseId);
        var course = courses.FirstOrDefault();

        //Act
        Assert.Equal(courseId, course!.Id);
    }

    [Fact]
    public async Task ShouldSetCourse()
    {
        //Arrange
        _context.Reset();
        var sut = new CourseService(new CourseRepository(_context.StudentContext));

        //Act
        var newCourse = new Course()
        {
            Description = "NewCourseDescription",
            Name = "NewCourse",
            StartDate = DateTime.Parse("01/01/2021")
        };

        await sut.SetCourseAsync(newCourse);

        var courses = await sut.GetCourses();
        var course = courses.LastOrDefault();

        //Assert
        Assert.Equal("NewCourse", course!.Name);
        Assert.Equal("NewCourseDescription", course.Description);
        Assert.Equal("1/1/2021", course.StartDate.ToShortDateString());
    }

    [Fact]
    public async Task ShouldUpdateCourse()
    {
        //Arrange
        _context.Reset();
        var sut = new CourseService(new CourseRepository(_context.StudentContext));
        //Act
        var courses = await sut.GetCourses();
        var course = courses.FirstOrDefault();
        course!.Name = "UpdatedCourseName";
        course.Description = "UpdatedCourseDescription";
        course.StartDate = DateTime.Parse("01/01/2021");
        await sut.UpdateCourseAsync(course);
        var updatedCourses = await sut.GetCourses(course!.Id);
        var updatedCourse = updatedCourses.FirstOrDefault();
        //Assert
        Assert.Equal("UpdatedCourseName", updatedCourse!.Name);
        Assert.Equal("UpdatedCourseDescription", updatedCourse.Description);
        Assert.Equal("1/1/2021", updatedCourse.StartDate.ToShortDateString());
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenDeletingCourseWithStudentsEnrolled()
    {
        //Arrange
        _context.Reset();
        var sut = new CourseService(new CourseRepository(_context.StudentContext));
        //Act
        var courses = await sut.GetCourses();
        var course = courses.FirstOrDefault();
        //Assert
        await Assert.ThrowsAsync<Exception>(async () => await sut.DeleteCourseAsync(course!.Id));
    }
}