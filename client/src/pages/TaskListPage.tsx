import {
  useGetTasksQuery,
  useAddTaskMutation,
  useUpdateTaskMutation,
  useDeleteTaskMutation,
} from "../api/taskApi";
import { useState } from "react";

export default function TaskListPage() {
  const { data: tasks = [], isLoading, isError } = useGetTasksQuery();
  const [addTask] = useAddTaskMutation();
  const [updateTask] = useUpdateTaskMutation();
  const [deleteTask] = useDeleteTaskMutation();
  const [title, setTitle] = useState("");

  const handleAdd = async () => {
    if (!title.trim()) return;
    await addTask({ title, isCompleted: false });
    setTitle("");
  };

  const handleToggle = async (taskId: number, isCompleted: boolean) => {
    const task = tasks.find((t) => t.id === taskId);
    if (task) {
      await updateTask({ ...task, isCompleted: !isCompleted });
    }
  };

  const handleDelete = async (id: number) => {
    await deleteTask(id);
  };

  if (isLoading) return <p>Loading...</p>;
  if (isError) return <p>Error loading tasks.</p>;

  return (
    <div>
      <h2>Task List</h2>
      <input
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="New task"
      />
      <button onClick={handleAdd}>Add</button>
      <ul>
        {tasks.map((task) => (
          <li key={task.id}>
            <label
              style={{
                textDecoration: task.isCompleted ? "line-through" : "none",
              }}
            />
            <input
              type="checkbox"
              checked={task.isCompleted}
              onChange={() => handleToggle(task.id, task.isCompleted)}
            />
            {task.title}

            <button onClick={() => handleDelete(task.id)}>❌</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
