using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;


namespace Lab2 
{
    interface IDateAndCopy
    {
        object DeepCopy();
        DateTime Date { get; set; }
    }

    //-----------------------------------------


    public class person : IDateAndCopy
    {
        protected string name;
        protected string sname;
        protected DateTime birthday;

        public person()
        {
            name = "Alex";
            sname = "Sidorov";
            birthday = DateTime.Now;
        }

        public person(string _name, string _sname, DateTime _birthDate)
        {
            name = _name;
            sname = _sname;
            birthday = _birthDate;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }

        }

        public string Sname
        {
            get { return sname; }
            set { sname = value; }
        }

        public int Years
        {
            get { return birthday.Year; }
            set { birthday = new DateTime(value, birthday.Month, birthday.Day); }
        }


        public override string ToString()
        {
            return name + " " + sname + " " + birthday.ToShortDateString() + "\n";

        }

        public virtual string ToShortString()
        {
            return Name + sname + "\n";
        }
        //*********************************************************
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var p = obj as person;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (name == p.name) && (sname == p.sname) &&
             (birthday == p.birthday);

        }

        public override int GetHashCode()
        {
            return name.GetHashCode() + sname.GetHashCode() + birthday.GetHashCode();
        }


        public static bool operator ==(person a, person b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(person a, person b)
        {
            return !(a == b);
        }

        public virtual object DeepCopy()
        {
            return new person(name, sname, birthday);
        }

        DateTime IDateAndCopy.Date { get; set; }

    }
    public enum Education
    {
        Specialist,//0
        Bachelor,
        SecondEducation
    }

    public class Exam : IDateAndCopy
    {
        public string Subject { get; set; }
        public int Mark { get; set; }
        public DateTime ExamDate { get; set; }

        public Exam()
        {
            Subject = "Default";
            Mark = 5;
            ExamDate = DateTime.Now;
        }

        public Exam(string subject, int mark, DateTime examDate)
        {
            Subject = subject;
            Mark = mark;
            ExamDate = examDate;
        }

        public object DeepCopy()
        {
            return new Exam(Subject, Mark, ExamDate);
        }

        DateTime IDateAndCopy.Date
        {
            get { return ExamDate; }
            set { ExamDate = value; }

        }


        public override string ToString()
        {
            return Subject + ", mark: " + Mark + ", date: " + ExamDate.ToShortDateString() + "\n";
        }

    }
    //*********************************************************

    public class Student : person, IDateAndCopy

    {

        private ArrayList tests;
        private ArrayList exams;
        private Education educationInfo;
        private int groupNumber;
        private person personalInfo;

        public person Personal
        {
            get { return personalInfo; }
            set { personalInfo = value; }
        }

        public Education Educational
        {
            get { return educationInfo; }
            set { educationInfo = value; }
        }

        public int GroupNumber
        {
            get { return groupNumber; }
            set
            {
                if (value <= 100 || value > 599)
                {
                    throw new ArgumentOutOfRangeException("Error! GroupNumber out of range(100, 599).");
                }
                groupNumber = value;
            }
        }

        public ArrayList Exams
        {
            get { return exams; }
            set { exams = value; }
        }


        public Student(person person, Education info, int _groupNumber)
        {
            educationInfo = info;
            personalInfo = person;
            groupNumber = _groupNumber;
            exams = new ArrayList();
            tests = new ArrayList();
        }

        public Student()
        {
            groupNumber = 481063;
            personalInfo = new person();
            educationInfo = Education.Specialist;
            exams = new ArrayList();
            tests = new ArrayList();
        }
        public double AverageMark
        {
            get
            {
                int sum = 0;
                foreach (Exam exam in exams)
                {
                    sum = sum + exam.Mark;
                }
                return (double)sum / exams.Count;
            }
        }


        public bool this[Education _education]
        {
            get { return educationInfo == _education; }
        }

        public void AddExam(Exam exam)
        {
            exams.Add(exam);
        }

        public void AddTest(Test _test)
        {
            tests.Add(_test);
        }

        public override string ToString()
        {
            StringBuilder strineg = new StringBuilder();
            strineg.AppendFormat("{0} {1} {2}", base.ToString(), groupNumber, educationInfo);
            foreach (Exam exam in exams)
                strineg.AppendLine(exam.ToString());
            foreach (Test test in tests)
                strineg.AppendLine(test.ToString());
            return strineg.ToString();

        }

        public override string ToShortString()
        {
            return
                base.ToShortString() +
                string.Format(
                "{0}, {1}, {2}, AVG Mark = {3}",
                Personal,
                Educational,
                GroupNumber,
                AverageMark
            );
        }

        public object DeepCopy()
        {
            var stud = new Student(personalInfo, educationInfo, groupNumber);
            foreach (Exam exam in this.exams)
            {
                stud.AddExam(exam);
            }
            foreach (Test test in this.tests)
            {
                stud.AddTest(test);
            }
            return stud;
        }


        public IEnumerable GetResults()
        {
            foreach (var exam in exams)
                yield return exam;
            foreach (var test in tests)
                yield return test;
        }

        public IEnumerable ExamsOver(int minRate)
        {
            foreach (var exam in exams)
            {
                Exam ex = (Exam)exam;
                if (ex.Mark > minRate)
                    yield return exam;
            }
        }

    }

    public class Test
    {
        public string subject { get; set; }
        public bool isPassed { get; set; }

        public Test(string _subject, bool _isPassed)
        {
            subject = _subject;
            isPassed = _isPassed;
        }

        public Test()
        {
            subject = "Math";
            isPassed = true;
        }

        public override string ToString()
        {
            return string.Format("Test subj = {0}, ispassed = {1}", subject, isPassed);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            var person1 = new person("Ivanov", "Ivan", new DateTime(1950, 01, 01));
            var person2 = new person("Ivanov", "Ivan", new DateTime(1950, 01, 01));

            Console.WriteLine(Object.ReferenceEquals(person2, person1));
            Console.WriteLine(person1 == person2);
            Console.WriteLine("хэш: \n{0}  \n{1}", person1.GetHashCode(), person2.GetHashCode());
            Console.WriteLine();
            Console.WriteLine("\n");

            var student = new Student(new person("Petrov", "Petr", new DateTime(2000, 01, 01)), Education.Specialist, 151);
            student.AddExam(new Exam("\n ObjectOrientedProgramming", 5, new DateTime(2018, 9, 21)));
            student.AddTest(new Test("CPP", true));
            Console.WriteLine(student.ToString());
            Console.WriteLine(student.Personal);
            Console.WriteLine("\n");
            var studentClone = (Student)student.DeepCopy();
            student.Name = "Sergey";
            student.Sname = "Sergeev";
            Console.WriteLine(student.ToString());
            Console.WriteLine(studentClone.ToString());
            Console.WriteLine("\n");
            try
            {
                student.GroupNumber = 600;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\n");

            foreach (var task in student.GetResults())
                Console.WriteLine(task.ToString());

            Console.WriteLine("----------------------------------------------------------");

            foreach (var task in student.ExamsOver(3))
                Console.WriteLine(task.ToString());
            Console.ReadKey(); } } }
