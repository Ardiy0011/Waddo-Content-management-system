This is a content management system designed to handle their text-based projects efficiently.

## Getting Started

1. **Register and Login:** You would need to register for an account and then log in with your given credentials. Once logged in, you will have access to the dashboard.

2. **Create a Project:** You would then be able to create new projects from the dashboard. After creating a project, you can add field names and descriptions. Each project can have up to 20 fields.


### Endpoints

1. **GetFieldDescription**
   - **URL:** `http://localhost:44306/Project/GetFieldDescription`
   - **Method:** `GET`
   - **Parameters:**
     - `projectName` (query parameter): The name of the project.
     - `fieldName` (query parameter): The name of the field.
   - **Description:** Retrieves the description of a specific field within a specified project.

2. **GetFields**
   - **URL:** `http://localhost:44306/Project/GetFields`
   - **Method:** `GET`
   - **Parameters:**
     - `projectName` (query parameter): The name of the project.
   - **Description:** Retrieves all fields' names and descriptions for a specified project in a JSON array format.


Apply to your projects as needed. -->
