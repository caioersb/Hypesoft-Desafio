import axios from "axios";
import { getKeycloak } from "@/providers/AuthProvider";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL as string,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: false,
});

api.interceptors.request.use((config) => {
  const keycloak = getKeycloak();
  if (keycloak?.token) {
    config.headers.Authorization = `Bearer ${keycloak.token}`;
  }
  return config;
});

export default api;
