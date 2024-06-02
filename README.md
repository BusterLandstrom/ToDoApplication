# ToDoApplication

A simple and interactive To-Do List application built using WPF (Windows Presentation Foundation). This application allows users to create, edit, and manage their tasks and steps within each task.

## Features

- **Add new tasks**: Easily create new tasks using the input field.
- **Mark tasks as done**: Click on the checkmark to mark a task as complete.
- **Edit task names**: Click on a task name to edit it.
- **Remove steps**: Click on the trash icon to remove a step from a task.
- **Animated interactions**: Smooth animations when hovering over task items.

## Technologies Used

- **WPF**: For creating the user interface.
- **C#**: For backend logic.
- **XAML**: For designing the UI components and layouts.

## Installation

1. **Download software from releases** [Releases](https://github.com/BusterLandstrom/ToDoApplication/releases)

2. **Add json file to AppData/Local/ToDo folder called account.json**
**Format like this**:
```json
    {
        "user": "mongodb-username",
        "password": "mongodb-password",
        "mongodbpath": "@link.path.mongodb.net/?retryWrites=true&w=majority&appName=mongodb-appname"
    }
```


## Usage

- **Adding a new task**:
  - Enter the task name in the input field with the watermark "New task".
  - Press `Enter` to create the task.

- **Marking a task as done**:
  - Click on the white checkmark circle to mark the task as done.
  - The checkmark inside the circle will appear when the task is marked as done.

- **Removing a step**:
  - Click on the trash icon (🗑) next to the step to remove it.

## License

This project is licensed under the GNU General Public License v3.0.

## Acknowledgments

- [Microsoft WPF Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- [Xceed WPF Toolkit](https://github.com/xceedsoftware/wpftoolkit) for additional UI controls.

