using Mimo.Common.DataAccess.IServices;
using Mimo.Common.DataModels;
using Mimo.Common.DBEntities.Courses;
using Mimo.Common.DBEntities.Results;
using Mimo.Common.Enums;
using Mimo.Common.IManagers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Managers.Managers
{
    public class ResultsManager : IResultsManager
    {
        private readonly IResultsService _resultsService;
        private readonly IAchievementsService _achievementsService;
        private readonly ICoursesService _coursesService;

        public ResultsManager(IResultsService resultsService, IAchievementsService achievementsService, ICoursesService coursesService)
        {
            _resultsService = resultsService;
            _achievementsService = achievementsService;
            _coursesService = coursesService;
        }

        public DataResponceModel<List<LessonProgressModel>> GetCompletedLessons(string userGuid)
        {
            var result = new DataResponceModel<List<LessonProgressModel>>();
            if (!CheckUser(userGuid))
            {
                result.Message = "It is problems with userGuid";
                return result;
            }
            result.Model = GetFinishedUniqueUserLessons(userGuid)
                .Select(x => new LessonProgressModel { LessonId = x.LessonId, LessonText = x.Lesson?.Text, EndDate = x.EndDate.Value, StartDate = x.StartDate }).ToList();
            result.Success = true;
            return result;

            //add Mapper to map models!!!
        }

        public DataResponceModel<List<AchievementModel>> GetUserAchievement(string userGuid)
        {
            var result = new DataResponceModel<List<AchievementModel>>();
            if (!CheckUser(userGuid))
            {
                result.Message = "It is problems with userGuid";
                return result;
            }
            var userAchievements = new List<AchievementModel>();
            var achievementList = _achievementsService.GetAllAchievements().ToList();
            var finishedLessons = GetFinishedUniqueUserLessons(userGuid);
            var finishedChapter = GetFinishedChapters(finishedLessons);
            var finishedCourse = GetFinishedCourses(finishedLessons, finishedChapter);

            foreach (var achievement in achievementList)
            {
                switch (achievement.AchievementType)
                {
                    case AchievementType.GotLessonsQuantity:
                        var countFinishedLessons = finishedLessons.Count();
                        if (countFinishedLessons >= achievement.AchievementObjectIntData)
                        {
                            userAchievements.Add(new AchievementModel
                            {
                                AchievementId = achievement.Id,
                                AchievementName = achievement.Name,
                                IsCompleted = true
                            });
                        }
                        else
                        {
                            userAchievements.Add(new AchievementModel
                            {
                                AchievementId = achievement.Id,
                                AchievementName = achievement.Name,
                                Progress = countFinishedLessons.ToString()
                            });
                        }
                        break;
                    case AchievementType.GotChapterQuantity:
                        var countFinishedChapter = finishedChapter.Count();
                        if (countFinishedChapter >= achievement.AchievementObjectIntData)
                        {
                            userAchievements.Add(new AchievementModel
                            {
                                AchievementId = achievement.Id,
                                AchievementName = achievement.Name,
                                IsCompleted = true
                            });
                        }
                        else
                        {
                            userAchievements.Add(new AchievementModel
                            {
                                AchievementId = achievement.Id,
                                AchievementName = achievement.Name,
                                Progress = countFinishedChapter.ToString()
                            });
                        }
                        break;
                    case AchievementType.GotCourse:
                        var course = _coursesService.GetCorseById(achievement.AchievementObjectId);
                        if (course == null)
                        {
                            result.Message = $"Problem with data achievement.Id[{achievement.Id}], achievement.AchievementObjectId[{achievement.AchievementObjectId}]";
                            return result;
                        }
                        if (finishedCourse.FirstOrDefault(x => x.Id == achievement.AchievementObjectId) != null)
                        {
                            userAchievements.Add(new AchievementModel
                            {
                                AchievementId = achievement.Id,
                                AchievementName = achievement.Name,
                                IsCompleted = true
                            });
                        }
                        else
                        {
                            userAchievements.Add(new AchievementModel
                            {
                                AchievementId = achievement.Id,
                                AchievementName = achievement.Name,
                                Progress = (course.Chapters?.Count - finishedChapter.Where(x => x.CourseId == course.Id)?.Count()).ToString()
                            });
                        }
                        break;
                    case AchievementType.GotTimeFrame:
                        break;
                    default:
                        break;
                }
            }
            result.Model = userAchievements;
            result.Success = true;
            return result;
        }

        public bool CheckUser(string userGuid)
        {
            if (string.IsNullOrEmpty(userGuid))
            {
                return false;
            }
            return _resultsService.GetUserByGuid(userGuid) != null;
        }

        public List<UserLesson> GetFinishedUniqueUserLessons(string userGuid)
        {
            return _resultsService.GetUserLessonsByUserGuid(userGuid).Where(x => x.EndDate.HasValue && x.IsCompleted).OrderByDescending(x => x.EndDate).ToList()
                .GroupBy(x => x.LessonId).Select(x => x.First()).ToList();
        }

        public List<Chapter> GetFinishedChapters(List<UserLesson> lessons)
        {
            var finishedChapters = new List<Chapter>();
            if (lessons == null || lessons?.Count == 0)
            {

                return finishedChapters;
            }
            var startChaptersIds = lessons.GroupBy(x => x.Lesson.ChapterId).Select(x => x.First()).Select(x => x.Lesson.ChapterId).ToList();
            var startChapters = _coursesService.GetChapters().Where(x => startChaptersIds.Contains(x.Id)).ToList();
            foreach (var chapter in startChapters)
            {
                var lessonsForChapter = lessons.Where(x => x.Lesson.ChapterId == chapter.Id).ToList();
                if (lessonsForChapter.Count() == chapter.Lessons?.Count)
                {
                    finishedChapters.Add(chapter);
                }
            }
            return finishedChapters;
        }

        public List<Course> GetFinishedCourses(List<UserLesson> lessons, List<Chapter> finishedChapters)
        {
            var finishedCourses = new List<Course>();
            if (lessons == null || lessons?.Count == 0 || finishedChapters == null || finishedChapters?.Count == 0)
            {
                return finishedCourses;
            }
            var startCoursesIds = lessons.GroupBy(x => x.Lesson.ChapterId).Select(x => x.First()).Select(x => x.Lesson.Chapter).GroupBy(x => x.CourseId).Select(x => x.First()).Select(x => x.CourseId).ToList();
            var startCourses = _coursesService.GetCorses().Where(x => startCoursesIds.Contains(x.Id)).ToList();
            foreach (var course in startCourses)
            {
                var chaptersForCourse = finishedChapters.Where(x => x.CourseId == course.Id).ToList();
                if (chaptersForCourse.Count == course.Chapters.Count)
                {
                    finishedCourses.Add(course);
                }
            }
            return finishedCourses;
        }
    }
}
