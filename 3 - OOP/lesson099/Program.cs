public class Program
{
    static void Main()
    {
        Console.WriteLine("=== University Course Enrollment System ===\n");

        University university = new University();

        // Create courses
        Course csharp = new Course("C# Programming", 101, 2);
        Course java = new Course("Java Fundamentals", 102, 3);

        university.AddCourse(csharp);
        university.AddCourse(java);

        // Create students
        Student s1 = new Student("Alice", 20, 1);
        Student s2 = new Student("Bob", 19, 2);
        Student s3 = new Student("Charlie", 22, 3);

        // Enroll students
        university.EnrollStudentInCourse(s1, 101);
        university.EnrollStudentInCourse(s2, 101);

        // This will fail (course is full)
        bool result = university.EnrollStudentInCourse(s3, 101);
        Console.WriteLine(result ? "Enrolled successfully" : "Enrollment failed: Course is full");
        Console.WriteLine();

        // Enroll student in another course
        university.EnrollStudentInCourse(s3, 102);

        // Print all courses and enrolled students
        university.ListAllCoursesAndStudents();

        Console.ReadKey();
    }
}

public class Course
{
    public string CourseName { get; set; }
    public int CourseCode { get; set; }
    public int MaxStudents { get; set; }
    private readonly List<Student> _students = new List<Student>();

    public Course(string courseName, int courseCode, int maxStudents)
    {
        CourseName = courseName;
        CourseCode = courseCode;
        MaxStudents = maxStudents;
    }

    public bool EnrollStudent(Student student)
    {
        if (_students.Count >= MaxStudents)
        {
            return false;
        }
        _students.Add(student);
        return true;
    }

    public List<string> ListStudents()
    {
        var studentNames = new List<string>();
        foreach (var student in _students)
        {
            studentNames.Add(student.Name);
        }
        return studentNames;
    }
}

public class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public int StudentID { get; set; }

    public Student(string name, int age, int studentID)
    {
        if (age < 18)
        {
            throw new ArgumentException("Student must be at least 18 years old.");
        }
        Name = name;
        Age = age;
        StudentID = studentID;
    }
}

public class University
{
    private readonly List<Course> _courses = new List<Course>();

    public void AddCourse(Course course)
    {
        _courses.Add(course);
    }

    public bool EnrollStudentInCourse(Student student, int courseCode)
    {
        var course = _courses.Find(c => c.CourseCode == courseCode);
        return course != null && course.EnrollStudent(student);
    }

    public void ListAllCoursesAndStudents()
    {
        foreach (var course in _courses)
        {
            Console.WriteLine($"Course: {course.CourseName}");
            foreach (var studentName in course.ListStudents())
            {
                Console.WriteLine($"- {studentName}");
            }
        }
    }
}

/*
* 1.Course Class:

    EnrollStudent checks if the maximum capacity is reached, only adding students when there’s room.

    ListStudents returns a list of names of enrolled students, encapsulating the _students list.

* 2.Student Class:

    Constructor throws an ArgumentException if Age is below 18, ensuring data integrity upon creation.

* 3.University Class:

    AddCourse allows courses to be added to the university.

    EnrollStudentInCourse finds the course by CourseCode and enrolls the student if there’s room.

    ListAllCoursesAndStudents iterates over each course, printing each course’s name and its enrolled students.
 */