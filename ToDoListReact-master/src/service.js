// src/service.js
import axios from 'axios';

// **** התיקון: עדכון ה-URL לכתובת הציבורית ב-Render ****
const apiUrl = "https://naama-todo-api.onrender.com/api"

// *** התיקון ל-415: חובה לשלוח Content-Type ***
axios.defaults.headers.common['Content-Type'] = 'application/json';

export default {
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/Items`)
    return result.data;
  },

  addTask: async (name) => {
    const result = await axios.post(`${apiUrl}/Items`, {
      name: name,
      isComplete: false 
    });
    return result.data;
  },

  setCompleted: async (item) => {
    // Item הוא אובייקט מלא כעת (id, name, isComplete)
    const result = await axios.put(`${apiUrl}/Items/${item.id}`, item);
    return result.data; 
  },

  deleteTask: async (id) => {
    const result = await axios.delete(`${apiUrl}/Items/${id}`);
    return result.data; 
  },
  
  setupInterceptors: () => {
    axios.interceptors.response.use(
      (response) => response,
      (error) => {
        console.error("API Error:", error.response);
        return Promise.reject(error);
      }
    );
  }
};