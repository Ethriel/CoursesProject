using Infrastructure.Models;
using ServerAPI.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Helpers
{
    public class AddCourses
    {
        public async static Task<bool> Add(IUnitOfWork unit)
        {
            var course = new TrainingCourse
            {
                Title = "Nonprofit Financial Stewardship Webinar: Introduction to Accounting and Financial Statements",
                Description = $@"The Introduction to Nonprofit Accounting and Financial Statements webinar series is designed to introduce basic nonprofit accounting concepts and financial statements to individuals who have had little or no experience with finance or accounting.

This self-paced webinar is being held in conjunction with the Nonprofit Financial Stewardship Executive Education program at Harvard Kennedy School. The webinar features Eric Schwartz, Managing Director in PricewaterhouseCoopers National Assurance Health Services.

Please note you will not receive a certificate for completing this webinar series.",
                Cover = @"./share/img/courses/1.jpg",
                StartDate = new DateTime(2020, 7, 31)
            };
            await unit.Courses.AddAsync(course);

            course = new TrainingCourse
            {
                Title = "CS50: Introduction to Computer Science",
                Description = $@"This is CS50x, Harvard University's introduction to the intellectual enterprises of computer science and the art of programming for majors and non-majors alike, with or without prior programming experience. 

An entry-level course taught by David J. Malan, CS50x teaches students how to think algorithmically and solve problems efficiently. Topics include abstraction, algorithms, data structures, encapsulation, resource management, security, software engineering, and web development. Languages include C, PHP, and JavaScript plus SQL, CSS, and HTML. Problem sets inspired by real-world domains of biology, cryptography, finance, forensics, and gaming.

As of Fall 2015, the on-campus version of CS50x, CS50, was Harvard's largest course.",
                Cover = @"./share/img/courses/2.jpg",
                StartDate = new DateTime(2020, 7, 31)
            };
            await unit.Courses.AddAsync(course);

            course = new TrainingCourse
            {
                Title = "CS50's Introduction to Game Development",
                Description = $@"In a quest to understand how video games themselves are implemented, you'll explore the design of such childhood games as: Super Mario Bros., Pong, Flappy Bird, Breakout, Match 3, Legend of Zelda, Angry Birds, Pokémon, 3D Helicopter Game, Dreadhalls, and Portal.

Via lectures and hands-on projects, the course explores principles of 2D and 3D graphics, animation, sound, and collision detection using frameworks like Unity and LÖVE 2D, as well as languages like Lua and C#. By class’s end, you'll have programmed several of your own games and gained a thorough understanding of the basics of game design and development.",
                Cover = @"./share/img/courses/3.jpg",
                StartDate = new DateTime(2020, 7, 31)
            };
            await unit.Courses.AddAsync(course);

            return true;
        }
    }
}
