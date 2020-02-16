# TMS
Software Challenge
To run application localy:
1. Restore database and change connection string in appsettings.json.
2. Open Windows Command Prompt.
3. Navigate in Command Prompt to TaskManagementSystem folder.
4. Enter command: "dotnet run".
5. Navigate to http://localhost:5001 to see Web API documentation.

Requirements:
1. TMS contains information about tasks. Task can contain one or more subtasks.
2. For every task following attributes are stored in TMS:
a. Name
b. Description
c. Start date
d. Finish date
e. State
3. If task doesn’t have subtasks, then state of task is set when task is updated and can be one of the following
values: Planned, inProgress, Completed
4. If task has subtasks, then its state is calculated by the following rules:
a. Completed, if all subtasks have state Completed
b. inProgress, if there is at least one subtask with state inProgress
c. Planned in all other cases
