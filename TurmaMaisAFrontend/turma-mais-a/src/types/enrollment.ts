export interface EnrollmentState {
    enrollments: EnrollmentDto[];
    error: string;
    loading: boolean;
}

export interface EnrollmentDto {
    id: string;
    studentId: string;
    courseId: string;
    courseName: string;
}

export interface SetStudentEnrollmentsDto {
    studentId: string;
    coursesIds: string[];
}