using Infrastructure.DbContext;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.DataPresentation;
using ServicesAPI.DTO;
using ServicesAPI.MapperWrappers;
using ServicesAPI.Responses;
using ServicesAPI.Services.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesAPI.Services.Implementations
{
    public class CoursesService : ICoursesService
    {
        private readonly IMapperWrapper<TrainingCourse, TrainingCourseDTO> mapperWrapper;
        private readonly IExtendedDataService<TrainingCourse> courses;
        private readonly IExtendedDataService<SystemUser> users;
        private readonly IImageWorker imageWorker;
        private readonly CoursesSystemDbContext context;

        public CoursesService(IMapperWrapper<TrainingCourse,
                              TrainingCourseDTO> mapperWrapper,
                              IExtendedDataService<TrainingCourse> courses,
                              IExtendedDataService<SystemUser> users,
                              IImageWorker imageWorker,
                              CoursesSystemDbContext context)
        {
            this.mapperWrapper = mapperWrapper;
            this.courses = courses;
            this.users = users;
            this.imageWorker = imageWorker;
            this.context = context;
        }

        public async Task<ApiResult> CheckCourseAsync(int userId, int courseId)
        {
            var result = default(ApiResult);

            // find user
            var user = await users.GetByIdAsync(userId);

            if (user == null)
            {
                var message = "User not found";
                var loggerMessage = "Unable to fetch course data";
                var errors = new string[] { message };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                // find course
                var course = await courses.GetByIdAsync(courseId);

                if (course == null)
                {
                    var message = "Course not found";
                    var loggerMessage = "Unable to fetch course data";
                    var errors = new string[] { "Course not found" };
                    result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
                }
                else
                {
                    // check if user has course
                    var userCourse = user.SystemUsersTrainingCourses
                                         .FirstOrDefault(x => x.TrainingCourseId
                                                               .Equals(courseId));

                    var courseData = new CourseData();

                    // set course data
                    if (userCourse == null)
                    {
                        courseData.IsPresent = false;
                    }
                    else
                    {
                        courseData.IsPresent = true;
                        courseData.StudyDate = userCourse.StudyDate
                                                         .ToShortDateString();
                    }

                    result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: courseData);
                }
            }

            return result;
        }

        public async Task<ApiResult> CreateCourseAsync(TrainingCourseDTO courseDTO)
        {
            var result = default(ApiResult);
            var course = await courses.GetEntityByConditionAsync(x => x.Title.Equals(courseDTO.Title));

            if (course != null)
            {
                var message = "Course creation has failed";
                var loggerMessage = $"Course with title {courseDTO.Title} already exists";
                var errors = new string[] { loggerMessage };
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }
            else
            {
                var image = courseDTO.Cover;
                if (image.Contains("share/img/courses"))
                {
                    courseDTO.Cover = imageWorker.CutImageToName(image);
                }
                course = mapperWrapper.MapEntity(courseDTO);
                await courses.CreateAsync(course);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, "Course created");
            }

            return result;
        }

        public async Task<ApiResult> GetAllCoursesAsync()
        {
            var result = default(ApiResult);
            var coursesData = await courses.Read()
                                           .ToArrayAsync();

            var data = mapperWrapper.MapModels(coursesData);

            imageWorker.SetCoursesImages(data);

            result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

            return result;
        }

        public async Task<ApiResult> GetByIdAsync(int id)
        {
            var result = default(ApiResult);

            var course = await courses.GetByIdAsync(id);

            if (course == null)
            {
                var message = "Course not found";
                var errors = new string[] { $"Course with id = {id} was not found" };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, message, message, errors);
            }
            else
            {
                var data = mapperWrapper.MapModel(course);
                data.Cover = imageWorker.GetImageURL("courses", data.Cover);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);
            }

            return result;
        }

        public async Task<ApiResult> GetPagedAsync(CoursesPagination coursesPagination)
        {
            var amount = await courses.GetCountAsync();

            var pagination = new Pagination();
            pagination.SetDefaults(amount, pageSize: 3);

            coursesPagination.Pagination ??= pagination;

            var skip = coursesPagination.Pagination
                                        .GetSkip();

            var take = coursesPagination.Pagination
                                        .GetTake();

            var coursesData = await courses.GetPortion(skip, take)
                                           .ToArrayAsync();

            var models = mapperWrapper.MapModels(coursesData);

            imageWorker.SetCoursesImages(models);

            var data = new { pagination = coursesPagination.Pagination, courses = models };

            var result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: data);

            return result;
        }

        public async Task<ApiResult> RemoveCourseAsync(int id)
        {
            var result = default(ApiResult);

            var course = await courses.GetByIdAsync(id);

            if (course == null)
            {
                var message = "Remove course error";
                var loggerMessage = $"Course id = {id} was not found in DELETE";
                var errors = new string[] { "Course was not found" };
                result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, loggerMessage, message, errors);
            }
            else
            {
                await courses.DeleteAsync(course);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, "Course was deleted!");
            }

            return result;
        }

        public ApiResult SaveImage(IFormFile image)
        {
            var result = default(ApiResult);
            var filename = imageWorker.ImageUploader.SaveImage(image, "courses", 1200, 1200);
            if (string.IsNullOrWhiteSpace(filename))
            {
                var message = "An error occured while saving file";
                var errors = new string[] { message };
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, message, message, errors);
            }
            else
            {
                var fileURL = imageWorker.GetImageURL("courses", filename);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: fileURL);
            }

            return result;
        }

        public async Task<ApiResult> UpdateCourseAsync(TrainingCourseDTO courseDTO)
        {
            var result = default(ApiResult);
            var course = await courses.GetByIdAsync(courseDTO.Id);

            if (course == null)
            {
                var message = "Course was not found";
                var loggerMessage = $"Course with title {courseDTO.Title} was not found";
                var errors = new string[] { loggerMessage };
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, loggerMessage, message, errors);
            }
            else
            {
                var newCourse = mapperWrapper.MapEntity(courseDTO);
                newCourse.Cover = imageWorker.CutImageToName(newCourse.Cover);
                course = await courses.UpdateAsync(course, newCourse);

                var model = mapperWrapper.MapModel(course);
                model.Cover = imageWorker.GetImageURL("courses", model.Cover);
                result = ApiResult.GetOkResult(ApiResultStatus.Ok, data: model);
            }

            return result;
        }

        public async Task<ApiResult> UploadImageAsync(IFormFile image, int id)
        {
            var result = default(ApiResult);

            var fileName = imageWorker.ImageUploader.UploadImage(image, "courses", 1200, 1200);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                var message = "An error occured during file upload";
                result = ApiResult.GetErrorResult(ApiResultStatus.BadRequest, "Error in upload file for USER", message, new string[] { message });
            }
            else
            {
                var course = await courses.GetByIdAsync(id);

                if (course == null)
                {
                    var message = "Course was not found";
                    result = ApiResult.GetErrorResult(ApiResultStatus.NotFound, $"Error: course id={id} was not found in UPLOAD FILE", message, new string[] { message });
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(course.Cover))
                    {
                        var imagePath = imageWorker.GetImageRootPath("courses", course.Cover);
                        imageWorker.DeleteImage(imagePath);
                    }

                    course.Cover = fileName;
                    await context.SaveChangesAsync();
                    var model = mapperWrapper.MapModel(course);

                    var cover = imageWorker.GetImageURL("courses", fileName);
                    model.Cover = cover;

                    result = ApiResult.GetOkResult(ApiResultStatus.Ok, "Image was uploaded", model);
                }
            }

            return result;
        }
    }
}
