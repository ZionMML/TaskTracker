import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { TaskItem } from "../types/TaskItem";

export const taskApi = createApi({
  reducerPath: "taskApi",
  baseQuery: fetchBaseQuery({ baseUrl: "http://localhost:5249/api/" }),
  tagTypes: ["Tasks"],
  endpoints: (builder) => ({
    getTasks: builder.query<TaskItem[], void>({
      query: () => "tasks",
      providesTags: ["Tasks"],
    }),
    addTask: builder.mutation<TaskItem, Partial<TaskItem>>({
      query: (task) => ({
        url: "tasks",
        method: "POST",
        body: task,
      }),
      invalidatesTags: ["Tasks"],
    }),
    updateTask: builder.mutation<void, TaskItem>({
      query: (task) => ({
        url: `tasks/${task.id}`,
        method: "PUT",
        body: task,
      }),
      invalidatesTags: ["Tasks"],
    }),
    deleteTask: builder.mutation<void, number>({
      query: (id) => ({
        url: `tasks/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: ["Tasks"],
    }),
  }),
});

export const {
  useGetTasksQuery,
  useAddTaskMutation,
  useUpdateTaskMutation,
  useDeleteTaskMutation,
} = taskApi;
