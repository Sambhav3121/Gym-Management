import axios from "axios";

const API = axios.create({
  baseURL: "http://localhost:5221/api", // Update if your backend port differs
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
