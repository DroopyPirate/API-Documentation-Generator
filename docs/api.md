# Exam Portal API Documentation

This documentation describes the publicly available API for the Exam Portal, allowing developers to interact with administrative, faculty, group, and test management functionalities.

---

## Admin API

### GET /
Retrieves the page for adding a new faculty member.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for adding faculty members using an Excel file.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `model` (List<ApplicationUser>): Not specified for complex types in query string.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to view existing faculty members.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to edit a specific faculty member.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for adding a new student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for adding students using an Excel file.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `model` (List<ApplicationUser>): Not specified for complex types in query string.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to view existing students.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to edit a specific student.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to view available tests.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Creates a new faculty member.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Role` (string) - Required
  - `Email` (string) - Required
  - `Password` (string)
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `PhoneNumber` (string) - Required
  - `Address` (string) - Required
  - `Branch` (BranchEnum) - Required
  - `DOB` (string) - Required
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Role=Faculty&Email=new.faculty@example.com&Password=secure_pass&Name=John&LastName=Doe&PhoneNumber=1234567890&Address=123%20Main%20St&Branch=ComputerScience&DOB=1980-01-01" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Adds faculty members via an uploaded Excel file.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (multipart/form-data)**:
  - `file` (IFormFile)
  - `hostingEnvironment` (IHostingEnvironment): Not specified.
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: multipart/form-data" \
  -F "file=@/path/to/faculty.xlsx" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Deletes an existing faculty member.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Retrieves details for a specific faculty member.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Updates details for an existing faculty member.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Role` (string)
  - `Id` (int)
  - `Email` (string) - Required
  - `Password` (string) - Required
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `PhoneNumber` (string) - Required
  - `Address` (string) - Required
  - `Branch` (BranchEnum) - Required
  - `DOB` (string) - Required
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Id=123&Email=updated.faculty@example.com&Password=new_pass&Name=Jane&LastName=Doe&PhoneNumber=0987654321&Address=456%20Oak%20Ave&Branch=InformationTechnology&DOB=1985-05-05" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Creates a new student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Role` (string) - Required
  - `Email` (string) - Required
  - `Password` (string)
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `PhoneNumber` (string) - Required
  - `Address` (string) - Required
  - `Branch` (BranchEnum) - Required
  - `Semester` (int) - Required
  - `Division` (string) - Required
  - `DOB` (string) - Required
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Role=Student&Email=new.student@example.com&Password=student_pass&Name=Alice&LastName=Smith&PhoneNumber=1112223333&Address=789%20Pine%20Ln&Branch=ComputerScience&Semester=3&Division=A&DOB=2000-11-11" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Adds students via an uploaded Excel file.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (multipart/form-data)**:
  - `file` (IFormFile)
  - `hostingEnvironment` (IHostingEnvironment): Not specified.
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: multipart/form-data" \
  -F "file=@/path/to/students.xlsx" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Deletes an existing student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=456" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Retrieves details for a specific student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=456" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Updates details for an existing student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Role` (string)
  - `Id` (int)
  - `Email` (string) - Required
  - `Password` (string) - Required
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `PhoneNumber` (string) - Required
  - `Address` (string) - Required
  - `Branch` (BranchEnum) - Required
  - `Semester` (int) - Required
  - `Division` (string) - Required
  - `DOB` (string) - Required
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Id=456&Email=updated.student@example.com&Password=new_student_pass&Name=Bob&LastName=Johnson&PhoneNumber=9998887777&Address=321%20Maple%20Rd&Branch=ElectricalEngineering&Semester=5&Division=B&DOB=1999-09-09" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

---

## Faculty API

### GET /
Retrieves the page for adding a new student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for adding students using an Excel file.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `model` (List<ApplicationUser>): Not specified for complex types in query string.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to view existing students.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to edit a specific student.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to view available tests.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Creates a new student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Role` (string) - Required
  - `Email` (string) - Required
  - `Password` (string)
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `PhoneNumber` (string) - Required
  - `Address` (string) - Required
  - `Branch` (BranchEnum) - Required
  - `Semester` (int) - Required
  - `Division` (string) - Required
  - `DOB` (string) - Required
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Role=Student&Email=new.student@example.com&Password=student_pass&Name=Alice&LastName=Smith&PhoneNumber=1112223333&Address=789%20Pine%20Ln&Branch=ComputerScience&Semester=3&Division=A&DOB=2000-11-11" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Adds students via an uploaded Excel file.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (multipart/form-data)**:
  - `file` (IFormFile)
  - `hostingEnvironment` (IHostingEnvironment): Not specified.
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: multipart/form-data" \
  -F "file=@/path/to/students.xlsx" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Deletes an existing student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=456" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Retrieves details for a specific student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=456" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Updates details for an existing student.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Role` (string)
  - `Id` (int)
  - `Email` (string) - Required
  - `Password` (string) - Required
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `PhoneNumber` (string) - Required
  - `Address` (string) - Required
  - `Branch` (BranchEnum) - Required
  - `Semester` (int) - Required
  - `Division` (string) - Required
  - `DOB` (string) - Required
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Id=456&Email=updated.student@example.com&Password=new_student_pass&Name=Bob&LastName=Johnson&PhoneNumber=9998887777&Address=321%20Maple%20Rd&Branch=ElectricalEngineering&Semester=5&Division=B&DOB=1999-09-09" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

---

## Group API

### GET /
Retrieves the page for creating a new group.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page with details for a specific group.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for adding students to a specific group.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to view groups associated with the current user.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to view other available groups.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to edit a specific group.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Creates a new group.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Creator_id` (int) - Required
  - `Name` (string) - Required
  - `Branch` (BranchEnum)
  - `Division` (DivisionEnum)
  - `Semester` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Creator_id=1&Name=MyNewGroup&Branch=ComputerScience&Division=A&Semester=5" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Adds students to a group.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (raw)**:
  - `array` (array of strings)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/json" \
  -d "[\"student_id_1\", \"student_id_2\"]" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (unknown content).

### POST /
Removes students from a group.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (raw)**:
  - `array` (array of strings)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/json" \
  -d "[\"student_id_to_remove_1\", \"student_id_to_remove_2\"]" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (unknown content).

### POST /
Updates details for an existing group.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
  - `Creator_id` (int) - Required
  - `Name` (string) - Required
  - `Branch` (BranchEnum)
  - `Division` (DivisionEnum)
  - `Semester` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123&Creator_id=1&Name=UpdatedGroupName&Branch=InformationTechnology&Division=B&Semester=6" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Deletes an existing group.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

---

## Home API

### GET /
Retrieves the home or index page.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for changing the user's password.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the user's profile page.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Authenticates a user and logs them in.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Email` (string) - Required
  - `Password` (string) - Required
  - `RememberMe` (bool)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Email=user@example.com&Password=secure_password&RememberMe=true" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Logs out the current user.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X POST "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Changes the current user's password.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `CurrentPassword` (string) - Required
  - `NewPassword` (string) - Required
  - `ConfirmPassword` (string)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "CurrentPassword=old_password&NewPassword=new_strong_password&ConfirmPassword=new_strong_password" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Updates the current user's profile information.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Password` (string) - Required
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `Address` (string) - Required
  - `Branch` (string)
  - `Semester` (int)
  - `Division` (string)
  - `DOB` (string) - Required
  - `InitialLogin` (bool)
  - `Tags` (List<Tag>)
  - `Tests` (ICollection<Test>)
  - `Groups` (ICollection<Groups>)
  - `UserGroups` (ICollection<UserGroup>)
  - `QuestionResults` (ICollection<QuestionResult>)
  - `DescriptiveResults` (ICollection<DescriptiveResult>)
  - `TotalResults` (ICollection<TotalResult>)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Password=user_pass&Name=ProfileName&LastName=ProfileLastName&Address=ProfileAddress&DOB=1990-10-10" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

---

## SuperAdmin API

### GET /
Retrieves the page for adding a new administrator.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to view existing administrators.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page to edit a specific administrator.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Creates a new administrator.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Role` (string) - Required
  - `Email` (string) - Required
  - `Password` (string)
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `PhoneNumber` (string) - Required
  - `Address` (string) - Required
  - `Branch` (BranchEnum) - Required
  - `DOB` (string) - Required
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Role=Admin&Email=new.admin@example.com&Password=admin_pass&Name=AdminName&LastName=AdminLastName&PhoneNumber=1231231234&Address=AdminAddress&Branch=ComputerScience&DOB=1975-01-01" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Retrieves details for a specific administrator.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Updates details for an existing administrator.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Role` (string)
  - `Id` (int)
  - `Email` (string) - Required
  - `Password` (string) - Required
  - `Name` (string) - Required
  - `MiddleName` (string)
  - `LastName` (string) - Required
  - `PhoneNumber` (string) - Required
  - `Address` (string) - Required
  - `Branch` (BranchEnum) - Required
  - `DOB` (string) - Required
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Id=123&Email=updated.admin@example.com&Password=new_admin_pass&Name=UpdatedAdmin&LastName=UpdatedAdminLastName&PhoneNumber=9876543210&Address=UpdatedAdminAddress&Branch=ElectricalEngineering&DOB=1976-02-02" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

### POST /
Deletes an existing administrator.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view) or HTTP 302 (redirect).

---

## Tag API

### GET /
Retrieves the page to view existing tags.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for renaming a specific tag.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Creates a new tag.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Create_Tag_Name` (string) - Required
  - `Tags` (List<Tag>)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Create_Tag_Name=NewTag" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Deletes an existing tag.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Renames an existing tag.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
  - `Create_Tag_Name` (string) - Required
  - `Tags` (List<Tag>)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123&Create_Tag_Name=RenamedTag" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

---

## Test API

### GET /
Retrieves the page to view existing tests.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page with details for a specific test.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for adding questions to a specific test.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for assigning a test to users or groups.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Creates a new test.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Title` (string) - Required
  - `Type_name` (string) - Required
  - `Duration` (TimeSpan) - Required
  - `StartDate` (string)
  - `EndDate` (string)
  - `PassingMarks` (int) - Required
  - `TestTypes` (List<TestType>)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Title=NewTest&Type_name=Quiz&Duration=01:00:00&StartDate=2023-01-01&EndDate=2023-01-05&PassingMarks=60" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Adds questions to a test.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Question` (array of strings) - Required
  - `Option1` (array of strings) - Required
  - `Option2` (array of strings) - Required
  - `Option3` (array of strings) - Required
  - `Option4` (array of strings) - Required
  - `Answer` (array of strings) - Required
  - `Description` (array of strings) - Required
  - `Marks` (array of int) - Required
  - `Tag_id` (array of int) - Required
  - `Test_id` (int)
  - `Tags` (List<Tag>)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Question[]=Q1&Option1[]=A1&Option2[]=A2&Option3[]=A3&Option4[]=A4&Answer[]=A1&Description[]=Desc1&Marks[]=5&Tag_id[]=1&Test_id=123" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Assigns a test to users or groups.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (raw)**:
  - `array` (array of strings)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/json" \
  -d "[\"user_id_1\", \"group_id_2\"]" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (unknown content).

---

## TestType API

### GET /
Retrieves the page to view existing test types.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### GET /
Retrieves the page for renaming a specific test type.
- **Route parameters**: Not specified.
- **Query parameters**:
  - `id` (int)
- **Request body**: Not specified.
- **Example curl command**:
  ```bash
  curl -X GET "http://localhost:5000/?id=123"
  ```
- **Response notes**: Responds with HTTP 200 (view).

### POST /
Creates a new test type.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `Create_TestType_Name` (string) - Required
  - `TestTypes` (List<TestType>)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Create_TestType_Name=NewTestType" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Deletes an existing test type.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).

### POST /
Renames an existing test type.
- **Route parameters**: Not specified.
- **Query parameters**: Not specified.
- **Request body (form-urlencoded)**:
  - `id` (int)
  - `Create_TestType_Name` (string) - Required
  - `TestTypes` (List<TestType>)
- **Example curl command**:
  ```bash
  curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
  -d "id=123&Create_TestType_Name=RenamedTestType" \
  "http://localhost:5000/"
  ```
- **Response notes**: Responds with HTTP 302 (redirect).