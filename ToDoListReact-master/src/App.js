import React, { useEffect, useState } from 'react';
import service from './service.js';

function App() {
  const [newTodo, setNewTodo] = useState("");
  const [todos, setTodos] = useState([]);

  // ודא שהאינטרספטורים מוגדרים (לצורך דרישת המטלה של ה-logger)
  useEffect(() => {
    service.setupInterceptors();
  }, []);

  async function getTodos() {
    const todos = await service.getTasks();
    setTodos(todos);
  }

  async function createTodo(e) {
    e.preventDefault();
    if (!newTodo.trim()) return;
    await service.addTask(newTodo);
    setNewTodo("");// clear input
    await getTodos();// refresh tasks list
  }

  // *** פונקציה מתוקנת שפותרת את שגיאת 400 Bad Request ב-PUT ***
  async function updateCompleted(todo, isComplete) {
    // יוצרים את האובייקט המלא הנדרש עבור ה-Controller ב-ASP.NET
    const updatedItem = {
      id: todo.id,
      name: todo.name,
      isComplete: isComplete, // הערך החדש
    };

    try {
        await service.setCompleted(updatedItem); 
        await getTodos();// refresh tasks list
    } catch (error) {
        // ה-Interceptor אמור לתפוס ולרשום שגיאות
        console.error("Update failed:", error);
    }
  }

  async function deleteTodo(id) {
    await service.deleteTask(id);
    await getTodos();// refresh tasks list
  }

  useEffect(() => {
    getTodos();
  }, []);

  return (
    <section className="todoapp">
      <header className="header">
        <h1>todos</h1>
        <form onSubmit={createTodo}>
          <input className="new-todo" placeholder="Well, let's take on the day" value={newTodo} onChange={(e) => setNewTodo(e.target.value)} />
        </form>
      </header>
      <section className="main" style={{ display: "block" }}>
        <ul className="todo-list">
          {todos.map(todo => {
            return (
              <li className={todo.isComplete ? "completed" : ""} key={todo.id}>
                <div className="view">
                  {/* קורא לפונקציה updateCompleted עם האובייקט המלא */}
                  <input className="toggle" type="checkbox" checked={todo.isComplete} onChange={(e) => updateCompleted(todo, e.target.checked)} />
                  <label>{todo.name}</label>
                  <button className="destroy" onClick={() => deleteTodo(todo.id)}></button>
                </div>
              </li>
            );
          })}
        </ul>
      </section>
    </section >
  );
}

export default App;