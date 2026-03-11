// Configuration
const API_BASE_URL = 'http://localhost:5033/api/student';

// DOM Elements
const studentForm = document.getElementById('studentForm');
const studentIdInput = document.getElementById('studentId');
const studentNameInput = document.getElementById('studentName');
const studentAgeInput = document.getElementById('studentAge');
const studentCourseInput = document.getElementById('studentCourse');
const submitBtn = document.getElementById('submitBtn');
const clearBtn = document.getElementById('clearBtn');
const messageDiv = document.getElementById('message');
const studentsContainer = document.getElementById('studentsContainer');

let isEditMode = false;
let editingStudentId = null;

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    loadStudents();
    studentForm.addEventListener('submit', handleFormSubmit);
    clearBtn.addEventListener('click', clearForm);
});

// Load all students
async function loadStudents() {
    try {
        const response = await fetch(`${API_BASE_URL}/getall`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const students = await response.json();
        displayStudents(students);
    } catch (error) {
        console.error('Error loading students:', error);
        studentsContainer.innerHTML = `<div class="empty-state"><p>Error loading students. Please refresh the page.</p></div>`;
    }
}

// Display students in the container
function displayStudents(students) {
    if (students.length === 0) {
        studentsContainer.innerHTML = `
            <div class="empty-state">
                <p>No students found</p>
                <p>Add a new student using the form above</p>
            </div>
        `;
        return;
    }

    studentsContainer.innerHTML = students.map(student => `
        <div class="student-card">
            <div class="student-id">ID: ${student.id}</div>
            <div class="student-info">
                <p><strong>Name:</strong> ${student.name}</p>
                <p><strong>Age:</strong> ${student.age}</p>
                <p><strong>Course:</strong> ${student.course}</p>
            </div>
            <div class="student-actions">
                <button class="btn btn-edit" onclick="editStudent('${student.id}', '${student.name}', ${student.age}, '${student.course}')">
                    Edit
                </button>
                <button class="btn btn-danger" onclick="deleteStudent('${student.id}')">
                    Delete
                </button>
            </div>
        </div>
    `).join('');
}

// Handle form submission
async function handleFormSubmit(e) {
    e.preventDefault();

    const studentData = {
        id: studentIdInput.value.trim(),
        name: studentNameInput.value.trim(),
        age: parseInt(studentAgeInput.value),
        course: studentCourseInput.value.trim()
    };

    // Validation
    if (!studentData.id || !studentData.name || !studentData.age || !studentData.course) {
        showMessage('Please fill in all fields', 'error');
        return;
    }

    if (studentData.age <= 0) {
        showMessage('Age must be greater than 0', 'error');
        return;
    }

    try {
        let response;
        let endpoint;

        if (isEditMode) {
            // Update existing student
            endpoint = `${API_BASE_URL}/update`;
            response = await fetch(endpoint, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(studentData)
            });
        } else {
            // Add new student
            endpoint = `${API_BASE_URL}/add`;
            response = await fetch(endpoint, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(studentData)
            });
        }

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || `Error: ${response.status}`);
        }

        const successMessage = isEditMode ? 'Student updated successfully!' : 'Student added successfully!';
        showMessage(successMessage, 'success');

        clearForm();
        loadStudents();
    } catch (error) {
        console.error('Error:', error);
        showMessage(error.message || 'An error occurred', 'error');
    }
}

// Edit student
function editStudent(id, name, age, course) {
    isEditMode = true;
    editingStudentId = id;
    
    studentIdInput.value = id;
    studentIdInput.disabled = true; // Prevent changing ID
    studentNameInput.value = name;
    studentAgeInput.value = age;
    studentCourseInput.value = course;
    
    submitBtn.textContent = 'Update Student';
    submitBtn.style.background = 'linear-gradient(135deg, #28a745 0%, #20c997 100%)';
    
    // Scroll to form
    studentForm.scrollIntoView({ behavior: 'smooth' });
}

// Delete student
async function deleteStudent(id) {
    if (!confirm(`Are you sure you want to delete the student with ID: ${id}?`)) {
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/delete/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || `Error: ${response.status}`);
        }

        showMessage('Student deleted successfully!', 'success');
        loadStudents();
    } catch (error) {
        console.error('Error:', error);
        showMessage(error.message || 'An error occurred while deleting', 'error');
    }
}

// Clear form
function clearForm() {
    studentForm.reset();
    isEditMode = false;
    editingStudentId = null;
    studentIdInput.disabled = false;
    submitBtn.textContent = 'Add Student';
    submitBtn.style.background = '';
    messageDiv.classList.remove('show');
}

// Show message
function showMessage(text, type) {
    messageDiv.textContent = text;
    messageDiv.className = `message show ${type}`;
    
    // Auto-hide after 5 seconds
    setTimeout(() => {
        messageDiv.classList.remove('show');
    }, 5000);
}
