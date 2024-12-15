import "@/App.css";
import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import PostListPage from "@/pages/PostListPage";
import CreatePostPage from "@/pages/CreatePostPage";
import PostPage from "@/pages/PostPage";
import Header from "@/components/Header";
import NotFoundPage from "@/pages/NotFoundPage";

const App: React.FC = () => {
  return (
    <Router>
      <Header />
      <div className="max-w-6xl mx-auto p-6">
        <Routes>
          <Route path="/" element={<PostListPage />} />
          <Route path="/create" element={<CreatePostPage />} />
          <Route path="/post/:postId" element={<PostPage />} />
          <Route path="*" element={<NotFoundPage />} />{" "}
          {/* Catch-all route for 404 */}
        </Routes>
      </div>
    </Router>
  );
};

export default App;
