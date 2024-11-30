﻿using LeetU.Data.Context;
using LeetU.Data.Entities;
using LeetU.Data.Interfaces;

namespace LeetU.Data.Repositories
{
    /// <summary>
    /// WHY do you ask. WHY is this empty? Well, if there any repository operations that are specialised, they go here and not in the CRUD base
    /// For example, if using Stored Procedures, the code to access them would be here.
    /// </summary>
    public class CourseRepository : RepositoryCrud<Course>, ICourseRepository
    {
        public CourseRepository(StudentContext context) : base(context)
        {
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return Context.Set<T>();
        }
    }
}
