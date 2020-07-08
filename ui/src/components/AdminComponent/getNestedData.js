function getNestedData(course) {
    return {
        key: course.trainingCourseId,
        id: course.trainingCourseId,
        title: course.title,
        studydate: course.studyDate
    }
};

export default getNestedData;