import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import TaskListPage from "./pages/TaskListPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<TaskListPage />} />
      </Routes>
    </Router>
  );
}

export default App;
