using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        
        private static List<Student> students = new List<Student>
        {
            new Student { Id = "1", Name = "Raj Sharma", Age = 20, Course = "Computer Science" },
            new Student { Id = "2", Name = "Priya Paudel", Age = 21, Course = "Information Technology" },
            new Student { Id = "3", Name = "Roshan Thapa", Age = 19, Course = "Software Engineering" },
            new Student { Id = "4", Name = "Anita Rai", Age = 22, Course = "Data Science" }
        };

    
        [HttpGet("getall")]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            return Ok(students);
        }




    
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudentById(string id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            return Ok(student);
        }





        [HttpPost("add")]
        public ActionResult<Student> AddStudent([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest(new { message = "Student object cannot be null" });
            }

            if (string.IsNullOrWhiteSpace(student.Id))
            {
                return BadRequest(new { message = "Student ID is required" });
            }

            if (string.IsNullOrWhiteSpace(student.Name))
            {
                return BadRequest(new { message = "Student Name is required" });
            }

            if (student.Age <= 0)
            {
                return BadRequest(new { message = "Student Age must be greater than 0" });
            }

            if (string.IsNullOrWhiteSpace(student.Course))
            {
                return BadRequest(new { message = "Student Course is required" });
            }

            if (students.Any(s => s.Id == student.Id))
            {
                return BadRequest(new { message = $"Student with ID {student.Id} already exists" });
            }

            students.Add(student);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }




    
        [HttpPut("update")]
        public ActionResult<Student> UpdateStudent([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest(new { message = "Student object cannot be null" });
            }

            if (string.IsNullOrWhiteSpace(student.Id))
            {
                return BadRequest(new { message = "Student ID is required" });
            }

            if (string.IsNullOrWhiteSpace(student.Name))
            {
                return BadRequest(new { message = "Student Name is required" });
            }

            if (student.Age <= 0)
            {
                return BadRequest(new { message = "Student Age must be greater than 0" });
            }

            if (string.IsNullOrWhiteSpace(student.Course))
            {
                return BadRequest(new { message = "Student Course is required" });
            }

            var existingStudent = students.FirstOrDefault(s => s.Id == student.Id);
            
            if (existingStudent == null)
            {
                return NotFound(new { message = $"Student with ID {student.Id} not found" });
            }

            existingStudent.Name = student.Name;
            existingStudent.Age = student.Age;
            existingStudent.Course = student.Course;

            return Ok(new { message = "Student updated successfully", student = existingStudent });
        }

        
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteStudent(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { message = "Student ID is required" });
            }

            var student = students.FirstOrDefault(s => s.Id == id);
            
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found" });
            }

            students.Remove(student);
            return Ok(new { message = $"Student with ID {id} deleted successfully" });
        }
    }
}
