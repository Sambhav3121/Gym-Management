import axios from "axios";

const API = axios.create({
  baseURL: "http://localhost:5221/api",
});

API.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export const registerUser = (data) => API.post("/User/register", data);
export const loginUser = (data) => API.post("/User/login", data);
export const logoutUser = () => API.post("/User/logout");
export const getProfile = () => API.get("/User/profile");
export const updateProfile = (data) => API.put("/User/profile", data);

export const getCurrentMembership = () =>
  API.get("/UserMembership/current");

export const markAttendance = () =>
  API.post("/UserMembership/attendance/mark");

export const getAttendanceSummary = () =>
  API.get("/UserMembership/attendance/summary");

export const getAttendanceRecent = (take = 5) =>
  API.get(`/UserMembership/attendance/recent?take=${take}`);

export const getClassesCount = () =>
  API.get("/UserMembership/classes/count");

export const getWorkoutActiveCount = () =>
  API.get("/UserMembership/workout/active");
