// src/App.tsx
import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import PostListPage from "./pages/PostListPage";
import CreatePostPage from "./pages/CreatePostPage";
import PostPage from "./pages/PostPage";
import Header from "@/components/Header"; // Import the Header component

const App: React.FC = () => {
  return (
    <Router>
      <Header /> {/* Include Header here */}
      <div className="max-w-6xl mx-auto p-6">
        <Routes>
          <Route path="/" element={<PostListPage />} />
          <Route path="/create" element={<CreatePostPage />} />
          <Route path="/post/:postId" element={<PostPage />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;
